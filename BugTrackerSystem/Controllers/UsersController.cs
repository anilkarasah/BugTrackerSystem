using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BugTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User u)
        {
            await _context.Users.AddAsync(u);
            return Ok(u);
        }
    }
}
