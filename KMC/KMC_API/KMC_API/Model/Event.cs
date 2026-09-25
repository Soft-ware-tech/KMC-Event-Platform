namespace KMC_API.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        // Event type / category - used for public search, e.g. "Cultural", "Music",
        // "Sports", "Workshop", "Exhibition", "Religious", "Community", "Other".
        [Required]
        public string Category { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        // Optional banner / poster image for the event (URL or relative wwwroot path).
        public string? ImageUrl { get; set; }

        // Maximum number of participants that can register for this event.
        // 0 (or null) means unlimited.
        public int RegistrationLimit { get; set; }

        // Not every event sells tickets. When false, registration is a simple,
        // free head-count against RegistrationLimit. When true, the public must
        // pick one of the event's TicketClasses (1st / 2nd / 3rd Class) and pay.
        public bool HasTickets { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [ForeignKey("Organizer")]
        public int OrganizerId { get; set; }
        public Organizer? CreatedBy { get; set; }

        public List<TicketClass> TicketClasses { get; set; } = new List<TicketClass>();
        public List<Registration> Registrations { get; set; } = new List<Registration>();
    }
}
