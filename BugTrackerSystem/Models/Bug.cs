namespace BugTrackerAPI.Models
{
    public class Bug
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public Status TrackStatus { get; set; }
        public Project RelatedProject { get; set; }
        public string? LogFile { get; set; }
    }
}
