using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestTask.Models.Entities;

namespace TestTask.Interfaces
{
    public interface IRoleService
    {
        Task<List<IdentityRole>> GetAllRolesAsync();

        Task CreateRoleAsync(string roleName);

        Task DeleteRoleByIdAsync(string id);

        Task<bool> AddOrCreateRoleForUserAsync(User user, string role = "Basic");

    }
}
