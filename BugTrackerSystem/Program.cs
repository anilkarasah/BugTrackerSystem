global using BugTrackerAPI.Data;
global using BugTrackerAPI.Models;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Http;
global using BugTrackerAPI.Services;
global using ErrorOr;

var builder = WebApplication.CreateBuilder(args);
{
	builder.Services.AddControllers();
	builder.Services.AddDbContext<DataContext>(options =>
	{
		options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
	});
	builder.Services.AddTransient<IBugService, BugService>();
}

var app = builder.Build();
{
	app.UseExceptionHandler("/error");
	app.UseHttpsRedirection();
	app.UseAuthorization();
	app.MapControllers();
	app.Run();
}
