using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using my_sparkle_api.Data;
using my_sparkle_api.DTOs;
using my_sparkle_api.Models;
using my_sparkle_api.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace my_sparkle_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly IConfiguration _config;

        public UsersController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }


        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users
                .Include(u => u.Children)
                .ToListAsync();

            var result = users.Select(u => new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Children = u.Children.Select(c => c.FirstName).ToList()
            });

            return Ok(result);
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.Children)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();

            var result = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Children = user.Children.Select(c => c.FirstName).ToList()
            };

            return Ok(result);
        }

        // GET: api/ping
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("pong");
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(UserCreateDto dto)
        {
            // 1. Model validation
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 2. Email check (prevent duplicates)
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == dto.Email.ToLower() || u.PhoneNumber == dto.PhoneNumber);

            if (existingUser != null)
            {
                return Conflict(new
                {
                    message = "A user with this email or phone number already exists.",
                });
            }

            // 3. Hash the password
            var hashedPassword = PasswordHasher.HashPassword(dto.Password);

            // 4. Create new User
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                PasswordHash = hashedPassword,
                Children = new List<Child>()
            };

            // 5. Save to DB
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // 6. Build return DTO
            var result = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Children = new List<string>()
            };

            // 7. Return success
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, result);
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<ActionResult<object>> Login(UserLoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Look up the user by email
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == dto.Email.ToLower());

            if (user == null)
            {
                // Don't reveal whether email or password was wrong
                return Unauthorized(new { message = "Invalid email or password." });
            }

            // Hash the incoming password to compare
            var hashedInput = PasswordHasher.HashPassword(dto.Password);

            if (hashedInput != user.PasswordHash)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            // Generate JWT token
            var token = JwtTokenGenerator.GenerateToken(user.Id.ToString(), user.Email, _config);

            // Return token + user info
            return Ok(new
            {
                token,
                expiresIn = 3600, // seconds (1 hour)
                user = new
                {
                    id = user.Id,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    email = user.Email
                }
            });
        }

    }
}
