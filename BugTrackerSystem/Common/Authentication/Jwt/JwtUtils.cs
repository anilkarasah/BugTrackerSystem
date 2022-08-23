using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BugTrackerAPI.Common.Authentication.Jwt;

public class JwtUtils : IJwtUtils
{
	private readonly IConfiguration _configuration;
	public JwtUtils(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public string GenerateToken(Guid ID, string Name, string Role)
	{
		var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
		var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

		var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Sub , ID.ToString()),
				new Claim(ClaimTypes.Name, Name),
				new Claim(ClaimTypes.Role, Role)
			};

		var tokenOptions = new JwtSecurityToken(
			issuer: _configuration["Jwt:Issuer"],
			audience: _configuration["Jwt:Audience"],
			claims: claims,
			notBefore: DateTime.UtcNow,
			expires: DateTime.UtcNow.AddHours(1),
			signingCredentials: credentials
		);

		var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
		return token;
	}

	public Guid? ValidateToken(string token)
	{
		if (string.IsNullOrEmpty(token))
			return null;

		var tokenHandler = new JwtSecurityTokenHandler();
		tokenHandler.ValidateToken(token, new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateIssuerSigningKey = true,

			ValidIssuer = _configuration["Jwt:Issuer"],
			ValidAudience = _configuration["Jwt:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]))
		}, out SecurityToken validatedToken);

		var jwtToken = (JwtSecurityToken)validatedToken;
		var subClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub);
		if (subClaim is null)
			return null;

		return new Guid(subClaim.Value);
	}
}
