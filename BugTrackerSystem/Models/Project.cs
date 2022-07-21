namespace BugTrackerAPI.Models
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<User> Contributors { get; set; }
    }
}
