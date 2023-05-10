using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vk_app.Database;
using vk_app.Entities;

namespace vk_app.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserStateController : ControllerBase
    {
        private readonly MyDbContext _context;

        public UserStateController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserState>>> GetUserStates()
        {
            var userStates = await _context.UserStates.ToListAsync();

            return Ok(userStates);
        }
    }
}
