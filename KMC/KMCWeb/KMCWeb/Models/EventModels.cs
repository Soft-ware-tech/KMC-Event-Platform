namespace KMCWeb.Models
{
    // Mirrors KMC_API.DTO.EventReadDTO - used to deserialize API responses.
    public class EventModel
    {
        public int EventId { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public string Category { get; set; } = "";
        public string Location { get; set; } = "";
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

        public bool IsUpcoming => StartDate.Date >= DateTime.Now.Date;
        public bool IsFull => SeatsLeft.HasValue && SeatsLeft.Value <= 0;
    }

    // Mirrors KMC_API.DTO.EventDetailDTO
    public class EventDetailModel : EventModel
    {
        public List<TicketClassModel> TicketClasses { get; set; } = new();
    }

    // Mirrors KMC_API.DTO.EventWriteDTO - used to create / update an event.
    public class EventWriteModel
    {
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public string Category { get; set; } = "";
        public string Location { get; set; } = "";
        public DateTime StartDate { get; set; } = DateTime.Now.Date.AddDays(7);
        public DateTime EndDate { get; set; } = DateTime.Now.Date.AddDays(7);
        public string? ImageUrl { get; set; }
        public int RegistrationLimit { get; set; }
        public bool HasTickets { get; set; }
        public int OrganizerId { get; set; }
    }

    // The event categories offered across the site (create form + search filter).
    public static class EventCategories
    {
        public static readonly string[] All = new[]
        {
            "Cultural", "Music", "Sports", "Workshop", "Exhibition",
            "Religious", "Community", "Food & Trade Fair", "Other"
        };
    }
}
