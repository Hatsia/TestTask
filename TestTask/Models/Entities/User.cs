using Microsoft.AspNetCore.Identity;

namespace TestTask.Models.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public override string UserName => FirstName + " " + LastName;

        public int? TeamId { get; set; }

        public Team Team { get; set; }
    }
}
