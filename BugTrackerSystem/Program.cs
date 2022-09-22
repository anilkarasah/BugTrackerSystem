global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Http;
global using BugTrackerAPI.Models;
global using BugTrackerAPI.Data;
global using BugTrackerAPI.Services;

using System.Text.Json.Serialization;

var envPort = Environment.GetEnvironmentVariable("PORT");
var port = string.IsNullOrWhiteSpace(envPort) ? 8001 : int.Parse(envPort);

var builder = WebApplication.CreateBuilder(args);
{
	builder.WebHost.UseKestrel(options => 
		options.Listen(System.Net.IPAddress.Parse("0.0.0.0"), port, listen => listen.UseHttps())
	);

	builder.Services.AddControllers().AddJsonOptions(options =>
		options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
	);

	// Integrate services from DependencyInjection class
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
