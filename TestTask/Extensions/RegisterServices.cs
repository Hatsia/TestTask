using Microsoft.Extensions.DependencyInjection;
using TestTask.Interfaces;
using TestTask.Services;

namespace TestTask.Extensions
{
    internal static class RegisterServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITeamService, TeamService>();

            return services;
        }
    }
}
