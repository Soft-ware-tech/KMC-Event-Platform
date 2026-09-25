using KMC_API.Model;

namespace KMC_API.Data
{
    public class ManagerRepo
    {
        private AppDBContext dbContext;

        public ManagerRepo(AppDBContext appDB)
        {
            dbContext = appDB;
        }

        public Manager? GetManagerByUsername(string username)
        {
            return dbContext.Managers.FirstOrDefault(m => m.Username == username);
        }

        public Manager? GetManagerByID(int id)
        {
            return dbContext.Managers.Find(id);
        }

        public (int totalOrganizers, int totalEvents, int totalRegistrations, int totalSeats, decimal totalRevenue) GetPlatformStats()
        {
            int totalOrganizers = dbContext.Organizers.Count();
            int totalEvents = dbContext.Events.Count();
            int totalRegistrations = dbContext.Registrations.Count();
            int totalSeats = dbContext.Registrations.Sum(r => (int?)r.Quantity) ?? 0;
            decimal totalRevenue = dbContext.Registrations.Sum(r => (decimal?)r.AmountPaid) ?? 0;

            return (totalOrganizers, totalEvents, totalRegistrations, totalSeats, totalRevenue);
        }

        // Groups every event by its Category, counts them, and sums the revenue
        // collected from registrations belonging to that category's events.
        // Powers the two bar charts on the Manager dashboard.
        public List<(string category, int eventCount, decimal revenue)> GetCategoryBreakdown()
        {
            var grouped = dbContext.Events
                .GroupBy(e => e.Category)
                .Select(g => new { Category = g.Key, EventIds = g.Select(e => e.EventId).ToList() })
                .ToList();

            var result = new List<(string, int, decimal)>();
            foreach (var g in grouped)
            {
                decimal revenue = dbContext.Registrations
                    .Where(r => g.EventIds.Contains(r.EventId))
                    .Sum(r => (decimal?)r.AmountPaid) ?? 0;

                result.Add((g.Category, g.EventIds.Count, revenue));
            }

            return result.OrderByDescending(x => x.Item2).ToList();
        }
    }
}