namespace KMC_API.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    // A ticket tier for a ticketed event, e.g. "1st Class", "2nd Class", "3rd Class".
    // Each tier has its own price and its own seat limit.
    public class TicketClass
    {
        [Key]
        public int TicketClassId { get; set; }

        [ForeignKey("Event")]
        public int EventId { get; set; }
        public Event? ForEvent { get; set; }

        [Required]
        public string ClassName { get; set; }

        [Required]
        public decimal Price { get; set; }

        // Number of seats available for this ticket class.
        [Required]
        public int Capacity { get; set; }

        public List<Registration> Registrations { get; set; } = new List<Registration>();
    }
}
