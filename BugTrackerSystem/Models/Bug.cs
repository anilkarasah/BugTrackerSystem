namespace BugTrackerAPI.Models
{
    public class Bug
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Status TrackStatus { get; set; }
        public string LogFile { get; set; } = string.Empty;
        public Project Project { get; set; }
    }
}
