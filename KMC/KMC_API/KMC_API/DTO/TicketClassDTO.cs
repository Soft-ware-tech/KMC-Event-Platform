namespace KMC_API.DTO
{
    public class TicketClassWriteDTO
    {
        public int EventId { get; set; }
        public string ClassName { get; set; }
        public decimal Price { get; set; }
        public int Capacity { get; set; }

        // Sent on update requests so the API can confirm the caller owns the parent event.
        public int OrganizerId { get; set; }
    }

    public class TicketClassReadDTO
    {
        public int TicketClassId { get; set; }
        public int EventId { get; set; }
        public string ClassName { get; set; }
        public decimal Price { get; set; }
        public int Capacity { get; set; }
        public int Sold { get; set; }
        public int Available { get; set; }
    }
}
