namespace KMC_API.Data
{
    using KMC_API.Model;
    using Microsoft.EntityFrameworkCore;

    public class RegistrationRepo
    {
        private AppDBContext dbContext;

        public RegistrationRepo(AppDBContext appDB)
        {
            dbContext = appDB;
        }

        public bool save()
        {
            int count = dbContext.SaveChanges();
            if (count > 0)
            {
                return true;
            }
            return false;
        }

        public bool AddRegistration(Registration registration)
        {
            if (registration != null)
            {
                dbContext.Registrations.Add(registration);
                return save();
            }

            return false;
        }

        public Registration? GetRegistrationByID(int id)
        {
            return dbContext.Registrations
                .Include(r => r.ForEvent)
                .Include(r => r.BookedClass)
                .FirstOrDefault(r => r.RegistrationId == id);
        }

        public List<Registration> GetRegistrationsByEvent(int eventId)
        {
            return dbContext.Registrations
                .Include(r => r.ForEvent)
                .Include(r => r.BookedClass)
                .Where(r => r.EventId == eventId)
                .OrderByDescending(r => r.RegisteredDate)
                .ToList();
        }

        public int TotalBookedForEvent(int eventId)
        {
            return dbContext.Registrations
                .Where(r => r.EventId == eventId)
                .Sum(r => (int?)r.Quantity) ?? 0;
        }

        public int TotalBookedForTicketClass(int ticketClassId)
        {
            return dbContext.Registrations
                .Where(r => r.TicketClassId == ticketClassId)
                .Sum(r => (int?)r.Quantity) ?? 0;
        }

        // NEW: every booking across every event, for the Manager's "All Bookings" page.
        public List<Registration> GetAllRegistrations()
        {
            return dbContext.Registrations
                .Include(r => r.ForEvent)
                .Include(r => r.BookedClass)
                .OrderByDescending(r => r.RegisteredDate)
                .ToList();
        }
    }
}