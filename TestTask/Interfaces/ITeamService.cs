using System.Collections.Generic;
using System.Threading.Tasks;
using TestTask.Models.RequestModels;
using TestTask.Models.ViewModels;

namespace TestTask.Interfaces
{
    public interface ITeamService
    {
        Task<TeamViewModel> GetTeamByIdAsync(int id);

        Task<List<TeamViewModel>> GetAllTeamsAsync();

        Task<TeamViewModel> CreateTeamAsync(BaseTeamRequest request);

        Task<TeamViewModel> UpdateTeamAsync(UpdateTeamRequest request);

        Task<bool> DeleteTeamByIdAsync(int id);

        Task<bool> IsTeamExistAsync(int id);

        Task<bool> AttachUserToTeamAsync(string userId, int teamId);

        Task DeleteUserFromTeamAsync(string userId);
    }
}
