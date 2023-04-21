using System.Collections.Generic;
using TestTask.Models.Entities;

namespace TestTask.Models.RequestModels
{
    public class BaseTeamRequest
    {
        public string Name { get; set; }
    }

    public class UpdateTeamRequest : BaseTeamRequest
    {
        public int Id { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
