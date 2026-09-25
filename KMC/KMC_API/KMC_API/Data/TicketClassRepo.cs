namespace KMC_API.Data
{
    using KMC_API.Model;

    public class TicketClassRepo
    {
        private AppDBContext dbContext;

        public TicketClassRepo(AppDBContext appDB)
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

        public bool AddTicketClass(TicketClass ticketClass)
        {
            if (ticketClass != null)
            {
                dbContext.TicketClasses.Add(ticketClass);
                return save();
            }

            return false;
        }

        public bool UpdateTicketClass(TicketClass ticketClass)
        {
            if (ticketClass != null)
            {
                dbContext.TicketClasses.Update(ticketClass);
                return save();
            }

            return false;
        }

        public bool RemoveTicketClass(TicketClass ticketClass)
        {
            if (ticketClass != null)
            {
                dbContext.TicketClasses.Remove(ticketClass);
                return save();
            }

            return false;
        }

        public List<TicketClass> GetTicketClassesByEvent(int eventId)
        {
            return dbContext.TicketClasses.Where(t => t.EventId == eventId).ToList();
        }

        public TicketClass? GetTicketClassByID(int id)
        {
            return dbContext.TicketClasses.Find(id);
        }

        public int CountSold(int ticketClassId)
        {
            return dbContext.Registrations
                .Where(r => r.TicketClassId == ticketClassId)
                .Sum(r => (int?)r.Quantity) ?? 0;
        }
    }
}
