using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestTask.Models;

namespace TestTask.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public override DbSet<User> Users { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
    }
}
