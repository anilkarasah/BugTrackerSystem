namespace BugTrackerAPI.Data;

public class DataContext : DbContext
{
	public DataContext(DbContextOptions<DataContext> options) : base(options) { }

	public DbSet<Bug> Bugs { get; set; }
	public DbSet<Project> Projects { get; set; }
	public DbSet<User> Users { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Bug>()
			.Property(b => b.IsFixed)
			.HasDefaultValue(false);

		modelBuilder.Entity<Bug>()
			.HasOne(b => b.User)
			.WithMany(u => u.ReportedBugs)
			.HasForeignKey(b => b.UserID);

		modelBuilder.Entity<Project>()
			.HasMany(p => p.Bugs)
			.WithOne(b => b.Project)
			.HasForeignKey(b => b.ProjectID)
			.OnDelete(DeleteBehavior.Cascade)
			.IsRequired();

		modelBuilder.Entity<User>()
			.Property(u => u.Role)
			.HasDefaultValue("user");

		modelBuilder.Entity<User>()
			.HasIndex(u => u.Email)
			.IsUnique();

		modelBuilder.Entity<ProjectUser>()
			.HasKey(t => new { t.UserID, t.ProjectID });

		modelBuilder.Entity<ProjectUser>()
			.HasOne(pu => pu.Project)
			.WithMany(pc => pc.Contibutors)
			.HasForeignKey(pu => pu.ProjectID)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<ProjectUser>()
			.HasOne(pu => pu.User)
			.WithMany(pc => pc.ProjectsList)
			.HasForeignKey(pu => pu.UserID)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
