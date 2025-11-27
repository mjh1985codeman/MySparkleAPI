using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using my_sparkle_api.Data;
using my_sparkle_api.DTOs;
using my_sparkle_api.Models;
using System.Security.Claims;

namespace my_sparkle_api.Controllers
{
    [Authorize] // Require login for ALL children routes
    [ApiController]
    [Route("api/[controller]")] // api/children
    public class ChildrenController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ChildrenController(AppDbContext context)
        {
            _context = context;
        }

        private int GetLoggedInUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
                throw new Exception("User ID claim not found in token.");

            return int.Parse(userIdString);
        }

        // POST: api/children
        [HttpPost]
        public async Task<ActionResult<ChildDto>> AddChild(ChildCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 1. Get logged-in user ID from JWT
            int userId = GetLoggedInUserId();

            // 2. Create the child entity
            var child = new Child
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Age = dto.Age,
                UserId = userId,
                Allergies = new List<Allergy>() 
            };

            // 3. Map allergy strings (from DTO) into Allergy entities
            if (dto.Allergies != null)
            {
                foreach (var allergyName in dto.Allergies)
                {
                    if (!string.IsNullOrWhiteSpace(allergyName))
                    {
                        child.Allergies.Add(new Allergy
                        {
                            Name = allergyName
                            // ChildId will be set automatically by EF when saving
                        });
                    }
                }
            }

            // 4. Save to database
            _context.Children.Add(child);
            await _context.SaveChangesAsync();

            // 5. Build return DTO (map Allergy entities back to strings)
            var returnDto = new ChildDto
            {
                Id = child.Id,
                FirstName = child.FirstName,
                LastName = child.LastName,
                Age = child.Age,
                Allergies = child.Allergies
                    .Select(a => a.Name)
                    .ToList()
            };

            return Ok(returnDto);
        }
    }
}
