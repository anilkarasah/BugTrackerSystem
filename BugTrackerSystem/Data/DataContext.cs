using Microsoft.EntityFrameworkCore;

namespace BugTrackerAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Bug> Bugs { get; set; }
    }
}
