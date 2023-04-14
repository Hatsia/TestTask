using Microsoft.AspNetCore.Identity;

namespace TestTask.Models
{
    public class User : IdentityUser
    {
        public int TeamId { get; set; }

        public Team Team { get; set; }
    }
}
