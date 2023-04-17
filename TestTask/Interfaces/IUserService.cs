using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using TestTask.Models.RequestModels;

namespace TestTask.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegistrationAsync(RegistrationRequest request);
        Task<SignInResult> LoginAsync(LoginRequest request, HttpContext context);

        Task LogoutAsync();
    }
}
