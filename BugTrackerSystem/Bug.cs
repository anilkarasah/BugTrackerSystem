namespace BugTrackerAPI
{
    public class Bug
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Status TrackStatus { get; set; }
        public string LogFile { get; set; } = string.Empty;

        public Bug(int id, string title, string description, Status trackStatus = Status.Listed)
        {
            Id = id;
            Title = title;
            Description = description;
            TrackStatus = trackStatus;
            CreatedAt = DateTime.Now;
        }
    }
}
