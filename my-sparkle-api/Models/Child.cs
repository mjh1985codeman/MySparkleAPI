using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace my_sparkle_api.Models
{
    // Represents a child/student who attends art classes
    public class Child
    {
        // Primary key (unique identifier for this child)
        public int Id { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required int Age { get; set; }

        // A list of known allergies (e.g., "Peanuts", "Latex", "Dairy")
        public List<Allergy> Allergies { get; set; } = new();

        // Foreign key to the parent (User)
        public int UserId { get; set; }

        [JsonIgnore] // prevents infinite loop
        public User Parent { get; set; } = null!;
    }
}
