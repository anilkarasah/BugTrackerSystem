using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

namespace BugTrackerAPI.Controllers
{
	[AllowAnonymous]
	[ApiExplorerSettings(IgnoreApi = true)]
	public class ErrorsController : ApiController
	{
		[Route("/error")]
		public IActionResult Error()
		{
			var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

			var statusCode = 500;
			var message = exception is not null ? exception.Message : "Internal server error";

			if (exception is ApiException ex)
			{
				statusCode = ex.StatusCode;
				message = ex.Message;
			}

			return Problem(statusCode: statusCode, title: message);
		}
	}
}
