using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace my_sparkle_api.DTOs
{
    public class ChildCreateDto
    {
        [Required(ErrorMessage = "Child first name is required.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Child last name is required.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Child Age Must be provided.")]
        public int Age { get; set; }

        // Optional: List of allergy names
        public List<string>? Allergies { get; set; }
    }
}

