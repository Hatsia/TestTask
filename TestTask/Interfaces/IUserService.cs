using Microsoft.AspNetCore.Http;
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

        Task<User> GetUserByEmailAsync(string email);
        Task<User> EditUserAsync(EditUserRequest request);
        Task<string> DeleteUserByIdAsync(string id);
        Task<List<User>> GetAllAsync();

        Task LogoutAsync();
    }
}
