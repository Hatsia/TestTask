namespace TestTask.Models.FilterModels
{
    public class UserFilterModel
    {
        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? TeamId { get; set; }
    }
}
