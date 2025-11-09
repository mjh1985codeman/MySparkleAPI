using System.Collections.Generic;

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
        public ICollection<string> Allergies { get; set; } = [];

        // Foreign key to the parent (User)
        public int UserId { get; set; }

        // Navigation property — lets us access the parent object from the child and every child must have a User (Parent) 
        public required User User { get; set; }
    }
}
