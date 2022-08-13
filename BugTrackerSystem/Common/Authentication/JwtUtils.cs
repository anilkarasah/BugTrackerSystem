using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BugTrackerAPI.Common.Authentication;

public class JwtUtils : IJwtUtils
{
	private readonly IConfiguration _configuration;
	public JwtUtils(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public string GenerateToken(Guid userID, string name, string email, string role)
	{
		var signingCredentials = new SigningCredentials(
			new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"])),
			SecurityAlgorithms.HmacSha256);

		var claims = new[]
		{
			new Claim(JwtRegisteredClaimNames.Sub, userID.ToString()),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			new Claim(ClaimTypes.Role, role)
		};

		var securityToken = new JwtSecurityToken(
			issuer: _configuration["Jwt:Issuer"],
			audience: _configuration["Jwt:Audience"],
			expires: DateTime.Now.AddDays(10),
			claims: claims,
			signingCredentials: signingCredentials
			);

		return new JwtSecurityTokenHandler().WriteToken(securityToken);
	}

	public Guid? ValidateToken(string token)
	{
		if (token is null)
			return null;

		try
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			tokenHandler.ValidateToken(token, new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = _configuration["Jwt:Issuer"],
				ValidAudience = _configuration["Jwt:Audience"],
				IssuerSigningKey = new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]))
			}, out SecurityToken validatedToken);

			var jwtToken = (JwtSecurityToken)validatedToken;
			string idAsString = jwtToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;
			Guid userID = Guid.Parse(idAsString);

			return userID;
		}
		catch (Exception)
		{
			return null;
		}
	}
}
