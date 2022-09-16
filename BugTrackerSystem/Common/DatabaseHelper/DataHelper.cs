namespace BugTrackerAPI.Common.DatabaseHelper;

public static class DataHelper
{
	public static async Task MigrateDbAsync(IServiceProvider provider)
	{
		var dbContextService = provider.GetRequiredService<DataContext>();
		
		await dbContextService.Database.MigrateAsync();
	}
}