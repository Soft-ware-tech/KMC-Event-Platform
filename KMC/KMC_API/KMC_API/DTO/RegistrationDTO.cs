namespace KMC_API.DTO
{
    // Submitted by the public booking form.
    // CardNumber / Cvv are used only in-memory to derive a mock "approval" and the
    // last 4 digits - they are never persisted to the database.
    public class RegistrationWriteDTO
    {
        public int EventId { get; set; }
        public string ParticipantName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Quantity { get; set; } = 1;

        // Required only when the event HasTickets == true.
        public int? TicketClassId { get; set; }

        // Mock card payment fields (ticketed events only).
        public string? CardHolderName { get; set; }
        public string? CardNumber { get; set; }
        public string? ExpiryMonth { get; set; }
        public string? ExpiryYear { get; set; }
        public string? Cvv { get; set; }
    }

    public class RegistrationReadDTO
    {
        public int RegistrationId { get; set; }
        public int EventId { get; set; }
        public string? EventTitle { get; set; }
        public string ParticipantName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Quantity { get; set; }
        public string? TicketClassName { get; set; }
        public string? CardLast4 { get; set; }
        public decimal AmountPaid { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime RegisteredDate { get; set; }
    }
}
