using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vk_app.Database;
using vk_app.Entities;

namespace vk_app.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserGroupController : ControllerBase
    {
        private readonly MyDbContext _context;

        public UserGroupController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserGroup>>> GetUserGroups()
        {
            var userGroups = await _context.UserGroups.ToListAsync();

            return Ok(userGroups);
        }
    }
}
