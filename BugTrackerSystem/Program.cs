global using BugTrackerAPI.Data;
global using BugTrackerAPI.Models;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Http;
global using BugTrackerAPI.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
{
	builder.Services.AddControllers().AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
	});

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
