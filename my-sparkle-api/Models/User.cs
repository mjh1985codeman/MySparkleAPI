using System.Collections.Generic;

namespace my_sparkle_api.Models
{
    // Represents a parent or guardian user in the system.
    public class User
    {
        // Primary Key (unique identifier for this user)
        public int Id { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Email { get; set; }

        // This will store a hashed password (never plain text!)
        public required string PasswordHash { get; set; }

        public required string PhoneNumber { get; set; }

        // Navigation property — allows this User to have multiple children registered.
        // The ICollection means it's a "one-to-many" relationship.
        public ICollection<Child> Children { get; set; } = new List<Child>();
    }
}
