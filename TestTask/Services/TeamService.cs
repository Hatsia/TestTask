using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestTask.Data;
using TestTask.Interfaces;
using TestTask.Models.Entities;
using TestTask.Models.RequestModels;
using TestTask.Models.ViewModels;

namespace TestTask.Services
{
    internal class TeamService : ITeamService
    {
        private readonly ApplicationDbContext _context;

        public TeamService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TeamViewModel> GetTeamByIdAsync(int id)
        {
            var team = await _context.Teams.AsNoTracking()
                                           .Include(x => x.Users)
                                           .FirstOrDefaultAsync(x => x.Id == id);

            return ParseTeamToTeamVM(team);
        }

        public async Task<List<TeamViewModel>> GetAllTeamsAsync()
        {
            var teams = await _context.Teams.AsNoTracking().Include(x => x.Users).ToListAsync();

            return ParseTeamsToTeamsVM(teams);
        }

        public async Task<TeamViewModel> CreateTeamAsync(BaseTeamRequest request)
        {
            var team = new Team { Name = request.Name, Users = new List<User>() };

            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();

            return ParseTeamToTeamVM(team);
        }

        public async Task<TeamViewModel> UpdateTeamAsync(UpdateTeamRequest request)
        {
            var team = new Team { Id = request.Id, Name = request.Name};
            
            if(request.Users != null)
            {
                team.Users = request.Users;
            }

            await Task.Run(() => _context.Teams.Update(team));
            await _context.SaveChangesAsync();

            return ParseTeamToTeamVM(team);
        }

        public async Task<bool> DeleteTeamByIdAsync(int id)
        {
            var team = await _context.Teams.FindAsync(id);

            var result = await Task.Run(() => _context.Teams.Remove(team));

            if (result.State == EntityState.Deleted)
            {
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> IsTeamExistAsync(int id)
        {
            return await _context.Teams.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> AttachUserToTeamAsync(string userId, int teamId)
        {
            var user = await _context.Users.Include(x => x.Team).FirstOrDefaultAsync(x => x.Id == userId);

            if (user.Team != null)
                return false;

            var team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == teamId);

            user.Team = team;

            await Task.Run(() => _context.Update(user));
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task DeleteUserFromTeamAsync(string userId)
        {
            var user = await _context.Users.Include(x => x.Team).FirstOrDefaultAsync(x => x.Id == userId);

            if (user.Team == null)
                return;

            user.Team = null;

            await Task.Run(() => _context.Update(user));
            await _context.SaveChangesAsync();
        }

        private static List<TeamViewModel> ParseTeamsToTeamsVM(List<Team> teams)
        {
            var teamsVM = new List<TeamViewModel>();

            foreach (var team in teams)
            {
                teamsVM.Add(ParseTeamToTeamVM(team));
            }

            return teamsVM;
        }

        private static TeamViewModel ParseTeamToTeamVM(Team team)
        {
            var usersVM = new List<UserViewModel>();
            if(team.Users != null)
            {
                foreach (var user in team.Users)
                {
                    usersVM.Add(new UserViewModel { Email = user.Email, UserName = user.UserName });
                }
            }

            var teamVM = new TeamViewModel
            {
                Id = team.Id,
                Name = team.Name,
                Users = usersVM
            };

            return teamVM;
        }
    }
}
