using KMC_API.Model;

namespace KMC_API.Data
{
    public class OrganizerRepo
    {
        private AppDBContext dbContext;

        public OrganizerRepo(AppDBContext appDB)
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

        public bool AddOrganizer(Organizer organizer)
        {
            if (organizer != null)
            {
                dbContext.Organizers.Add(organizer);
                return save();
            }

            return false;
        }

        public Organizer? GetOrganizerByID(int id)
        {
            return dbContext.Organizers.Find(id);
        }

        public Organizer? GetOrganizerByUsername(string username)
        {
            return dbContext.Organizers.FirstOrDefault(o => o.Username == username);
        }

        public bool UsernameExists(string username)
        {
            return dbContext.Organizers.Any(o => o.Username == username);
        }

        public bool EmailExists(string email)
        {
            return dbContext.Organizers.Any(o => o.Email == email);
        }

        public List<Organizer> GetAllOrganizers()
        {
            return dbContext.Organizers.ToList();
        }

        public bool RemoveOrganizer(Organizer organizer)
        {
            dbContext.Organizers.Remove(organizer);
            return save();
        }
        public bool UpdateOrganizer(Organizer organizer)
        {
            dbContext.Organizers.Update(organizer);
            return save();
        }
    }
}