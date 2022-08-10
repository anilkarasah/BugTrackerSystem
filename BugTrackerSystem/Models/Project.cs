using System.Text.Json.Serialization;

namespace BugTrackerAPI.Models
{
    public class Project
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        
        public virtual ICollection<ProjectUser> Contibutors { get; set; }
		
		[JsonIgnore]
        public virtual ICollection<Bug> Bugs { get; set; }
    }
}
