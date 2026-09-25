namespace KMC_API.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    // A member of the public registering / booking a seat for an event.
    public class Registration
    {
        [Key]
        public int RegistrationId { get; set; }

        [ForeignKey("Event")]
        public int EventId { get; set; }
        public Event? ForEvent { get; set; }

        [Required]
        public string ParticipantName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        // How many seats this booking covers (only relevant for ticketed events).
        public int Quantity { get; set; } = 1;

        // Null for free / non-ticketed events.
        [ForeignKey("TicketClass")]
        public int? TicketClassId { get; set; }
        public TicketClass? BookedClass { get; set; }

        // Mock card-payment trail - only the card holder name and last 4 digits
        // are ever persisted. The full card number / CVV are never stored.
        public string? CardHolderName { get; set; }
        public string? CardLast4 { get; set; }

        public decimal AmountPaid { get; set; }

        // "N/A" (free event), "Paid" (mock payment approved), "Failed"
        [Required]
        public string PaymentStatus { get; set; } = "N/A";

        public DateTime RegisteredDate { get; set; } = DateTime.Now;
    }
}
