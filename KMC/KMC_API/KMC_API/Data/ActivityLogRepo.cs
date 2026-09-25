using KMC_API.Model;

namespace KMC_API.Data
{
    public class ActivityLogRepo
    {
        private AppDBContext dbContext;

        public ActivityLogRepo(AppDBContext appDB)
        {
            dbContext = appDB;
        }

        // Called from other controllers whenever a manager does something worth recording.
        public void Log(string managerName, string actionType, string targetName)
        {
            dbContext.ActivityLogs.Add(new ActivityLog
            {
                ManagerName = managerName,
                ActionType = actionType,
                TargetName = targetName
            });
            dbContext.SaveChanges();
        }

        public List<ActivityLog> GetRecent(int count = 10)
        {
            return dbContext.ActivityLogs
                .OrderByDescending(a => a.Timestamp)
                .Take(count)
                .ToList();
        }
    }
}