global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Http;
global using BugTrackerAPI.Models;
global using BugTrackerAPI.Data;
global using BugTrackerAPI.Services;

using System.Text.Json.Serialization;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);
{
	// builder.WebHost.UseKestrel(options =>
	// {
	// 	options.Listen(System.Net.IPAddress.Parse("0.0.0.0"), port);
	// 	options.ListenLocalhost(8000);
	// });

	builder.Services.AddControllers().AddJsonOptions(options =>
		options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
	);

	builder.Services.Configure<ForwardedHeadersOptions>(options =>
	{
		options.ForwardedHeaders = ForwardedHeaders.XForwardedProto;
		options.ForwardedProtoHeaderName = "x-forwarded-proto";
	});

	// Integrate services from DependencyInjection class
	builder.Services.AddServices(builder.Configuration);
}

var app = builder.Build();
{
	// app.UseHttpsRedirection();
	app.UseForwardedHeaders();
	app.UseCors();
	app.UseExceptionHandler("/error");
	app.UseAuthentication();
	app.UseAuthorization();
	app.MapControllers();
	app.Run();
}
