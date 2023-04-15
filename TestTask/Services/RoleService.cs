using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using TestTask.Enums;
using TestTask.Interfaces;
using TestTask.Models.Entities;

namespace TestTask.Services
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> AddOrCreateRoleForUserAsync(User user, string role = "Basic")
        {
            if (Enum.IsDefined(typeof(RoleTypes), role) == false)
            {
                return false;
            }
            if (await _roleManager.RoleExistsAsync(role) == false)
            {
                var identityRole = new IdentityRole { Name = role };

                await _roleManager.CreateAsync(identityRole);
            }

            await _userManager.AddToRoleAsync(user, role);

            return true;
        }
    }
}
