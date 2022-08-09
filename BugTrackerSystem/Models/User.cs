namespace BugTrackerAPI.Models
{
    public class User
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Roles { get; set; }

        public virtual ICollection<ProjectUser> ProjectsList { get; set; }
    }
}
