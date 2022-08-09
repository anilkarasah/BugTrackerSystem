namespace BugTrackerAPI.Models
{
    public class Bug
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? LogFile { get; set; }

        public Guid ProjectID { get; set; }
        public virtual Project Project { get; set; }
    }
}
