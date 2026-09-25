namespace KMCWeb.Models
{
    // Mirrors KMC_API.DTO.OrganizerReadDTO
    public class OrganizerReadModel
    {
        public int OrganizerId { get; set; }
        public string FullName { get; set; } = "";
        public string? OrganizationName { get; set; }
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Username { get; set; } = "";
        public bool IsActive { get; set; }
    }

}
