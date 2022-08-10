namespace BugTrackerAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ApiController : ControllerBase
	{
		protected IActionResult AppError(int statusCode = 500, string message = "Internal server error")
		{
			return Problem(statusCode: statusCode, title: message);
		}

		protected IActionResult SendResponse(object? value, int statusCode = 200, string message = "OK")
		{
			return StatusCode(
				statusCode: statusCode,
				value: new { message, value });
		}
	}
}
