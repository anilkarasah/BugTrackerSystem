using Microsoft.AspNetCore.Mvc;

namespace BugTrackerAPI.Controllers
{
    public class ProjectsController : ControllerBase
    {
        public IActionResult Index()
        {
            return Problem();
        }
    }
}
