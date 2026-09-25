namespace KMC_API.Model
{
    using System.ComponentModel.DataAnnotations;

    // A platform-level administrator - manages organizers and can view all
    // public bookings across every event. Unlike Organizer, there is no
    // public sign-up endpoint for this - accounts are created directly in
    // the database by whoever runs the platform.
    public class Manager
    {
        [Key]
        public int ManagerId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}