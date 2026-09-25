namespace KMC_API.DTO
{
    // Used when a new organizer signs up.
    public class OrganizerRegisterDTO
    {
        public string FullName { get; set; }
        public string? OrganizationName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    // Used to log an existing organizer in.
    public class OrganizerLoginDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    // Safe, public-facing view of an organizer (never includes the password hash).
    public class OrganizerReadDTO
    {
        public int OrganizerId { get; set; }
        public string FullName { get; set; }
        public string? OrganizationName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }
        public bool IsActive { get; set; }
    }
}
