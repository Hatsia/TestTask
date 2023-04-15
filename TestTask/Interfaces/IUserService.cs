using System.Threading.Tasks;
using TestTask.Models.RequestModels;

namespace TestTask.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegistrationAsync(RegistrationRequest request);
    }
}
