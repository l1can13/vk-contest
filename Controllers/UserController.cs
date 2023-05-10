using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vk_app.Database;
using vk_app.Entities;

namespace vk_app.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly MyDbContext _context;

        public UserController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _context.Users
                .Include(u => u.UserGroup)
                .Include(u => u.UserState)
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.UserGroup)
                .Include(u => u.UserState)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> AddUser(User user)
        {
            if (await _context.Users.AnyAsync(u => u.Login == user.Login))
            {
                return Conflict("User with this login already exists");
            }

            if (user.UserGroup.Code == "Admin" && await _context.Users.AnyAsync(u => u.UserGroup.Code == "Admin"))
            {
                return Conflict("There can be only one user with Admin role");
            }

            user.CreatedDate = DateTime.UtcNow;
            user.UserState = await _context.UserStates.FirstOrDefaultAsync(us => us.Code == "Active");

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            user.UserState = await _context.UserStates.FirstOrDefaultAsync(us => us.Code == "Blocked");

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
