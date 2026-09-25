namespace KMCWeb.Models
{
    public class ActivityLogModel
    {
        public int ActivityLogId { get; set; }
        public string ManagerName { get; set; } = "";
        public string ActionType { get; set; } = "";
        public string TargetName { get; set; } = "";
        public DateTime Timestamp { get; set; }
    }
}