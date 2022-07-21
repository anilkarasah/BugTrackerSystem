namespace BugTrackerAPI.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public List<Project> Projects { get; set; }
    }
}
