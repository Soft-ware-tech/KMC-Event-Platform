namespace KMC_API.Data
{
    using KMC_API.Model;
    using Microsoft.EntityFrameworkCore;

    public class EventRepo
    {
        private AppDBContext dbContext;

        public EventRepo(AppDBContext appDB)
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

        public bool AddEvent(Event ev)
        {
            if (ev != null)
            {
                dbContext.Events.Add(ev);
                return save();
            }

            return false;
        }

        public bool UpdateEvent(Event ev)
        {
            if (ev != null)
            {
                dbContext.Events.Update(ev);
                return save();
            }

            return false;
        }

        public bool RemoveEvent(Event ev)
        {
            if (ev != null)
            {
                // Delete Registrations first, then TicketClasses, then the Event -
                // manual order avoids a SQL conflict, since Registration -> TicketClass
                // is Restrict while TicketClass -> Event and Registration -> Event
                // are both Cascade.
                var regs = dbContext.Registrations.Where(r => r.EventId == ev.EventId).ToList();
                dbContext.Registrations.RemoveRange(regs);

                var tickets = dbContext.TicketClasses.Where(t => t.EventId == ev.EventId).ToList();
                dbContext.TicketClasses.RemoveRange(tickets);

                dbContext.Events.Remove(ev);
                return save();
            }

            return false;
        }

        // Public listing - includes organizer name, newest events first.
        public List<Event> GetEvents()
        {
            return dbContext.Events
                .Include(e => e.CreatedBy)
                .OrderByDescending(e => e.StartDate)
                .ToList();
        }

        public Event? GetEventByID(int id)
        {
            return dbContext.Events
                .Include(e => e.CreatedBy)
                .Include(e => e.TicketClasses)
                .FirstOrDefault(e => e.EventId == id);
        }

        public List<Event> GetEventsByOrganizer(int organizerId)
        {
            return dbContext.Events
                .Include(e => e.CreatedBy)
                .Where(e => e.OrganizerId == organizerId)
                .OrderByDescending(e => e.CreatedDate)
                .ToList();
        }

        // Public search by keyword / type(category) / a specific date that falls
        // within the event's Start-End window.
        public List<Event> SearchEvents(string? keyword, string? category, DateTime? date)
        {
            var query = dbContext.Events.Include(e => e.CreatedBy).AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(e =>
                    EF.Functions.Like(e.Title, $"%{keyword}%") ||
                    EF.Functions.Like(e.Location, $"%{keyword}%") ||
                    (e.Description != null && EF.Functions.Like(e.Description, $"%{keyword}%")));
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(e => e.Category == category);
            }

            if (date.HasValue)
            {
                var d = date.Value.Date;
                query = query.Where(e => e.StartDate.Date <= d && e.EndDate.Date >= d);
            }

            return query.OrderBy(e => e.StartDate).ToList();
        }

        public int CountRegistrations(int eventId)
        {
            return dbContext.Registrations.Count(r => r.EventId == eventId);
        }
    }
}
