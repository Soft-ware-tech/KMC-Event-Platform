namespace KMCWeb.Models
{
    // Mirrors KMC_API.DTO.RegistrationWriteDTO - the public booking form posts this.
    public class RegistrationWriteModel
    {
        public int EventId { get; set; }
        public string ParticipantName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public int Quantity { get; set; } = 1;
        public int? TicketClassId { get; set; }

        // Mock card payment fields - only used for ticketed events.
        public string? CardHolderName { get; set; }
        public string? CardNumber { get; set; }
        public string? ExpiryMonth { get; set; }
        public string? ExpiryYear { get; set; }
        public string? Cvv { get; set; }
    }

    // Mirrors KMC_API.DTO.RegistrationReadDTO - the booking confirmation.
    public class RegistrationReadModel
    {
        public int RegistrationId { get; set; }
        public int EventId { get; set; }
        public string? EventTitle { get; set; }
        public string ParticipantName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public int Quantity { get; set; }
        public string? TicketClassName { get; set; }
        public string? CardLast4 { get; set; }
        public decimal AmountPaid { get; set; }
        public string PaymentStatus { get; set; } = "";
        public DateTime RegisteredDate { get; set; }
    }
}
