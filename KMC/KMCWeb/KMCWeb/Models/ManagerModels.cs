namespace KMCWeb.Models
{
    public class ManagerReadModel
    {
        public int ManagerId { get; set; }
        public string FullName { get; set; } = "";
        public string Username { get; set; } = "";
    }

    public class DashboardStatsModel
    {

        public int TotalOrganizers { get; set; }
        public int TotalEvents { get; set; }
        public int TotalRegistrations { get; set; }
        public int TotalSeatsBooked { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<CategoryStatModel> EventsByCategory { get; set; } = new();
    }
    public class CategoryStatModel
    {
        public string CategoryName { get; set; } = "";
        public int EventCount { get; set; }
        public decimal TotalRevenue { get; set; }
    }

}