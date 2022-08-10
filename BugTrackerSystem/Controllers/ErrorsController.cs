using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;

namespace BugTrackerAPI.Controllers
{
	[AllowAnonymous]
	[ApiExplorerSettings(IgnoreApi = true)]
	public class ErrorsController : ApiController
	{
		[Route("/error")]
		public IActionResult Error()
		{
			var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

			var exceptionMessage = context is not null ? context.Error.Message : "Internal server error";

			return AppError(500, exceptionMessage);
		}
	}
}
