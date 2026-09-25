namespace KMCWeb.Models
{
    // Mirrors KMC_API.DTO.TicketClassReadDTO
    public class TicketClassModel
    {
        public int TicketClassId { get; set; }
        public int EventId { get; set; }
        public string ClassName { get; set; } = "";
        public decimal Price { get; set; }
        public int Capacity { get; set; }
        public int Sold { get; set; }
        public int Available { get; set; }
    }

    // Mirrors KMC_API.DTO.TicketClassWriteDTO
    public class TicketClassWriteModel
    {
        public int EventId { get; set; }
        public string ClassName { get; set; } = "1st Class";
        public decimal Price { get; set; }
        public int Capacity { get; set; }
        public int OrganizerId { get; set; }
    }

    public static class TicketClassNames
    {
        public static readonly string[] All = new[] { "1st Class", "2nd Class", "3rd Class" };
    }
}
