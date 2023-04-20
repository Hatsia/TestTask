using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TestTask.Interfaces;
using TestTask.Models.Entities;
using TestTask.Models.RequestModels;

namespace TestTask.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IRoleService _roleService;
        private readonly SignInManager<User> _signInManager;

        public UserService(UserManager<User> userManager, IRoleService roleService, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleService = roleService;
            _signInManager = signInManager;
        }

        public async Task<bool> RegistrationAsync(RegistrationRequest request)
        {
            var user = new User
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            var isSuccessful = await CreateUserAsync(user, request.Password);

            if (isSuccessful)
            {
                await _signInManager.SignInAsync(user, false);
                return await _roleService.AddOrCreateRoleForUserAsync(user);
            }

            return false;
        }

        private async Task<bool> CreateUserAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
                return true;

            return false;
        }

        public async Task<SignInResult> LoginAsync(LoginRequest request, HttpContext context)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            
            if(user == null)
            {
                return SignInResult.Failed;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, request.RememberMe, false);
            
            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                };

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),
                    IsPersistent = request.RememberMe
                };

                await context.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
            }

            return result;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }


        public async Task<User> EditUserAsync(EditUserRequest request)
        {
            var currentUser = await _userManager.FindByEmailAsync(request.Email);

            currentUser.Email = request.Email;
            currentUser.FirstName = request.FirstName;
            currentUser.LastName = request.LastName;
            
            await _userManager.RemovePasswordAsync(currentUser);
            await _userManager.AddPasswordAsync(currentUser, request.Password);
            await _userManager.UpdateAsync(currentUser);
            
            return currentUser;
        }

        public async Task<string> DeleteUserByIdAsync(string id)
        {
            var isResult = await _userManager.DeleteAsync(_userManager.FindByIdAsync(id).Result);
            if (isResult.Succeeded)
            {
                return "User deleted";
            }

            return "Error";
        }

        public async Task<User> GetUserByEmailAsync(string email) => await _userManager.FindByEmailAsync(email);
        public async Task<List<User>> GetAllAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return users;
        }

    }
}
