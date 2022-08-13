namespace BugTrackerAPI.Common.Authentication;

public class JwtMiddleware
{
	private readonly RequestDelegate _next;
	public JwtMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
	{
		// split "Authorization" header into "Bearer" and JWT
		var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

		// validate JWT and save user content into context
		var userID = jwtUtils.ValidateToken(token);
		if (userID is not null)
			context.Items["User"] = await userService.GetUserByID(userID.Value);

		await _next(context);
	}
}
