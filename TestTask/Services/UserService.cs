using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TestTask.Data;
using TestTask.Interfaces;
using TestTask.Models.Entities;
using TestTask.Models.FilterModels;
using TestTask.Models.RequestModels;
using TestTask.Models.ViewModels;

namespace TestTask.Services
{
    public class UserService : IUserService
    {
        private readonly IRoleService _roleService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;

        public UserService(UserManager<User> userManager, IRoleService roleService, SignInManager<User> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleService = roleService;
            _signInManager = signInManager;
            _context = context;
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

        public async Task<SignInResult> LoginAsync(LoginRequest request, HttpContext context)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
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

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),
                    IsPersistent = request.RememberMe
                };

                await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            }

            return result;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<List<User>> GetAllUserAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            return users;
        }
        public async Task<List<UserFilterModel>> GetAllUsersFMAsync()
        {
            var users = await _context.Users.AsNoTracking().Include(x => x.Team).ToListAsync();

            return ParseUsersToUsersFM(users);
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
            var isResult = await _userManager.DeleteAsync(await _userManager.FindByIdAsync(id));

            if (isResult.Succeeded)
            {
                return "User deleted";
            }

            return "Error";
        }

        public async Task<User> GetUserByEmailAsync(string email) => await _userManager.FindByEmailAsync(email);

        public async Task<User> GetUserByIdAsync(string id) => await _userManager.FindByIdAsync(id);

        public async Task<bool> IsInRoleAsync(User user, string role) => await _userManager.IsInRoleAsync(user, role);

        public async Task<IList<string>> GetUserRolesAsync(User user) => await _userManager.GetRolesAsync(user);

        public async Task<IdentityResult> AddRolesToUserAsync(User user, IEnumerable<string> roles) => await _userManager.AddToRolesAsync(user, roles);

        public async Task<IdentityResult> RemoveRolesFromUserAsync(User user, IList<string> roles) => await _userManager.RemoveFromRolesAsync(user, roles);

        public async Task<List<UserFilterModel>> GetUsersByFilterAsync(UserFilterModel filter)
        {
            var users = _context.Users.Include(x => x.Team).AsQueryable();

            if (filter != null)
                users = GetFiltredUsers(users, filter);

            return ParseUsersToUsersFM(await users.ToListAsync());
        }

        private async Task<bool> CreateUserAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
                return true;

            return false;
        }

        private IQueryable<User> GetFiltredUsers(IQueryable<User> users, UserFilterModel filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Email))
                users = users.Where(x => x.Email == filter.Email);
            if (!string.IsNullOrWhiteSpace(filter.FirstName))
                users = users.Where(x => x.FirstName == filter.FirstName);
            if (!string.IsNullOrWhiteSpace(filter.LastName))
                users = users.Where(x => x.LastName == filter.LastName);
            if (filter.TeamId != null)
                users = users.Where(x => x.TeamId == filter.TeamId);

            return users;
        }

        private static List<UserFilterModel> ParseUsersToUsersFM(List<User> users)
        {
            var usersVM = new List<UserFilterModel>();

            foreach (var user in users)
            {
                usersVM.Add(ParseUserToUserFilterFM(user));
            }

            return usersVM;
        }

        private static UserFilterModel ParseUserToUserFilterFM(User user)
        {
            var userVM = new UserFilterModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                TeamId = user.TeamId
            };

            return userVM;
        }
    }
}
