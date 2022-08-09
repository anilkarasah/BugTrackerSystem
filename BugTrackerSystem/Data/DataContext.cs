namespace BugTrackerAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Bug> Bugs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bug>()
                .Property(b => b.LogFile)
                .IsRequired(false);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Bugs)
                .WithOne(b => b.Project)
                .HasForeignKey(b => b.ProjectID)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Roles)
                .HasDefaultValue("U");

            modelBuilder.Entity<ProjectUser>()
                .HasKey(t => new { t.UserID, t.ProjectID });

            modelBuilder.Entity<ProjectUser>()
                .HasOne(pu => pu.Project)
                .WithMany(pc => pc.Contibutors)
                .HasForeignKey(pu => pu.ProjectID);

            modelBuilder.Entity<ProjectUser>()
                .HasOne(pu => pu.User)
                .WithMany(pc => pc.ProjectsList)
                .HasForeignKey(pu => pu.UserID);
        }
    }
}
