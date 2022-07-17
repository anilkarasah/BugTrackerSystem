namespace BugTrackerSystem
{
    public class Bug
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public Status Status { get; set; }
    }
}
