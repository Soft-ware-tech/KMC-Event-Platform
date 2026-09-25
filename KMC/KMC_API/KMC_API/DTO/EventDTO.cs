namespace KMC_API.DTO
{
    public class EventWriteDTO
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? ImageUrl { get; set; }
        public int RegistrationLimit { get; set; }
        public bool HasTickets { get; set; }
        public int OrganizerId { get; set; }
    }

    public class EventReadDTO
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? ImageUrl { get; set; }
        public int RegistrationLimit { get; set; }
        public bool HasTickets { get; set; }
        public int OrganizerId { get; set; }
        public string? OrganizerName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int RegisteredCount { get; set; }
        public int? SeatsLeft { get; set; }
    }

    // Full detail view for the event page - includes ticket tiers.
    public class EventDetailDTO : EventReadDTO
    {
        public List<TicketClassReadDTO> TicketClasses { get; set; } = new List<TicketClassReadDTO>();
    }
}
