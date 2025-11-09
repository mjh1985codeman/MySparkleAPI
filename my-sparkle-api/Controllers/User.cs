using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using my_sparkle_api.Data;
using my_sparkle_api.Models;

namespace my_sparkle_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Constructor: inject AppDbContext so we can talk to the DB
        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            // Fetch all users with their children
            var users = await _context.Users
                .Include(u => u.Children)
                .ToListAsync();

            return Ok(users); // return 200 OK with the data
        }

        // GET: api/users/1
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.Children)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound(); // 404 if not found

            return Ok(user);
        }

        // GET: api/users/ping
        [HttpGet("ping")]
        public async Task<ActionResult<string>> Ping()
        {
            return Ok("pong"); // double quotes for string
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync(); // persist to DB

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // PUT: api/users/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id) return BadRequest();

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(u => u.Id == id)) return NotFound();
                throw;
            }

            return NoContent(); // 204: success, nothing to return
        }

        // DELETE: api/users/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
