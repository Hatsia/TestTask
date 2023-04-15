using Microsoft.AspNetCore.Identity;
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

        public UserService(UserManager<User> userManager, IRoleService roleService)
        {
            _userManager = userManager;
            _roleService = roleService;
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
    }
}
