namespace my_sparkle_api.Models
{
    public class Allergy
    {
        public int Id { get; set; } // Primary key
        public string Name { get; set; } = string.Empty; // Allergy name, e.g. "Peanuts"

        // Relationship back to the child
        public int ChildId { get; set; }
        public Child Child { get; set; } = null!;
    }
}
