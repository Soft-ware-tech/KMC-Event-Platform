namespace KMC_API.DTO
{
    public class ManagerLoginDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class ManagerReadDTO
    {
        public int ManagerId { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
    }

    // One row per category - powers the two analytics charts.
    public class CategoryStatDTO
    {
        public string CategoryName { get; set; }
        public int EventCount { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class DashboardStatsDTO
    {
        public int TotalOrganizers { get; set; }
        public int TotalEvents { get; set; }
        public int TotalRegistrations { get; set; }
        public int TotalSeatsBooked { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<CategoryStatDTO> EventsByCategory { get; set; } = new();
    }
}