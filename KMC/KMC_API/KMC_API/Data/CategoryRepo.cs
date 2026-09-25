using KMC_API.Model;

namespace KMC_API.Data
{
    public class CategoryRepo
    {
        private AppDBContext dbContext;

        public CategoryRepo(AppDBContext appDB)
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

        public List<Category> GetAll()
        {
            return dbContext.Categories.OrderBy(c => c.Name).ToList();
        }

        public Category? GetById(int id)
        {
            return dbContext.Categories.Find(id);
        }

        public bool NameExists(string name)
        {
            return dbContext.Categories.Any(c => c.Name.ToLower() == name.ToLower());
        }

        public bool Add(Category category)
        {
            if (category != null)
            {
                dbContext.Categories.Add(category);
                return save();
            }
            return false;
        }

        public bool Remove(Category category)
        {
            dbContext.Categories.Remove(category);
            return save();
        }
    }
}