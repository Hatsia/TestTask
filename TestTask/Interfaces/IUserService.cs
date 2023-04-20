﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestTask.Models.Entities;
using TestTask.Models.RequestModels;

namespace TestTask.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegistrationAsync(RegistrationRequest request);
        Task<SignInResult> LoginAsync(LoginRequest request, HttpContext context);
        Task LogoutAsync();
        Task<List<User>> GetAllUserAsync();
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(string id);
        Task<User> EditUserAsync(EditUserRequest request);
        Task<string> DeleteUserByIdAsync(string id);
        Task<bool> IsInRoleAsync(User user, string role);
        Task<IList<string>> GetUserRolesAsync(User user);
        Task<IdentityResult> AddToRolesAsync(User user, IEnumerable<string> roles);
        Task<IdentityResult> RemoveFromRolesAsync(User user, IList<string> roles);
    }
}
