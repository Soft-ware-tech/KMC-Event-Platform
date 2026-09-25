namespace KMC_API.Model
{
    using System.ComponentModel.DataAnnotations;

    // One row per manager action - who did what, to what, and when.
    public class ActivityLog
    {
        [Key]
        public int ActivityLogId { get; set; }

        [Required]
        public string ManagerName { get; set; }

        [Required]
        public string ActionType { get; set; }   // e.g. "Suspended Organizer", "Deleted Event"

        [Required]
        public string TargetName { get; set; }   // e.g. the organizer's name or event title

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}