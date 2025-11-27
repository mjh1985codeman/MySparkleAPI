using System.Collections.Generic;

namespace my_sparkle_api.DTOs
{
    public class ChildDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public int Age { get; set; }

        public List<string> Allergies { get; set; } = new List<string>();
    }
}
