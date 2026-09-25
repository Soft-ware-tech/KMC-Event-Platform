namespace KMC_API.Model
{
    using System.ComponentModel.DataAnnotations;

    // Represents an event organizer (city officer, local business, community group, etc.)
    // who is allowed to create and manage events on the KMC platform.
    public class Organizer
    {
        [Key]
        public int OrganizerId { get; set; }

        [Required]
        public string FullName { get; set; }

        // Optional - e.g. "Kandy Rotary Club", "ABC (Pvt) Ltd"
        public string? OrganizationName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Username { get; set; }

        // Stores a SHA256 hash of the password - never the plain text password.
        [Required]
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public List<Event> EventList { get; set; } = new List<Event>();
    }
}
