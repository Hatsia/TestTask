using System.Collections.Generic;

namespace TestTask.Models.ViewModels
{
    public class TeamViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
#nullable enable
        public ICollection<UserViewModel>? Users { get; set; }
#nullable disable
    }
}
