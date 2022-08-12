namespace BugTrackerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApiController : ControllerBase
{
	protected IActionResult SendResponse(object? value, int statusCode = 200)
	{
		return StatusCode(
			statusCode: statusCode,
			value: value);
	}
}
