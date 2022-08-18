global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Http;
global using BugTrackerAPI.Models;
global using BugTrackerAPI.Data;
global using BugTrackerAPI.Services;

using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BugTrackerAPI.Common.Authentication.Hash;
using BugTrackerAPI.Common.Authentication.Jwt;

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

	builder.Services.AddServices();
	builder.Services.AddSingleton<IHashUtils, HashUtils>();
	builder.Services.AddSingleton<IJwtUtils, JwtUtils>();

	builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
	{
		options.RequireHttpsMetadata = false;
		options.SaveToken = true;
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = builder.Configuration["Jwt:Issuer"],
			ValidAudience = builder.Configuration["Jwt:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
		};
	});
}

var app = builder.Build();
{
	app.UseExceptionHandler("/error");
	app.UseHttpsRedirection();
	app.UseAuthentication();
	app.UseAuthorization();
	app.UseMiddleware<JwtMiddleware>();
	app.MapControllers();
	app.Run();
}
