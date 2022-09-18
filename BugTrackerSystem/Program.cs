global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Http;
global using BugTrackerAPI.Models;
global using BugTrackerAPI.Data;
global using BugTrackerAPI.Services;

using System.Text.Json.Serialization;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
{
	var port = Environment.GetEnvironmentVariable("PORT") ?? "8000";
	builder.WebHost.UseKestrel(options => 
		options.Listen(IPAddress.Parse("0.0.0.0"), int.Parse(port))
	);
	
	builder.Services.AddControllers().AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
	});

	// Integrate services
	builder.Services.AddServices(builder.Configuration);
}

var app = builder.Build();
{
	app.UseHttpsRedirection();
	app.UseCors();
	app.UseExceptionHandler("/error");
	app.UseAuthentication();
	app.UseAuthorization();
	app.MapControllers();
	app.Run();
}
