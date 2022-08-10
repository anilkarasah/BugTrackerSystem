using Microsoft.AspNetCore.Mvc;

namespace BugTrackerAPI.Controllers
{
    public class ProjectsController : ApiController
    {
        private readonly DataContext _context;
        public ProjectsController(DataContext context)
		{
            _context = context;
		}

		[HttpGet]
        public ActionResult<List<Project>> GetAllProject()
        {
            return _context.Projects.ToList();
        }
    }
}
