using System.Threading.Tasks;
using TestTask.Models.Entities;

namespace TestTask.Interfaces
{
    public interface IRoleService
    {
        Task<bool> AddOrCreateRoleForUserAsync(User user, string role = "Basic");
    }
}
