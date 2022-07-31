namespace BugTrackerAPI.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public List<Project> Projects { get; set; }

        public User()
        {
            this.Projects = new List<Project>();
        }
    }
}
