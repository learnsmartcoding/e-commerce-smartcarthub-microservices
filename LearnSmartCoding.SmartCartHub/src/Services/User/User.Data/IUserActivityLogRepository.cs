using Microsoft.EntityFrameworkCore;
using User.Core.Entities;

namespace User.Data
{
    public interface IUserActivityLogRepository
    {
        Task<UserActivityLog> LogUserActivityAsync(UserActivityLog activityLog, string adObjId);
        Task<List<UserActivityLog>> GetUserActivityLogsAsync(int userId);
        Task<List<UserActivityLog>?> GetUserActivityLogsAsync(string adObjId);
    }

    public class UserActivityLogRepository : IUserActivityLogRepository
    {
        private readonly LearnSmartDbContext _dbContext;

        public UserActivityLogRepository(LearnSmartDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserActivityLog> LogUserActivityAsync(UserActivityLog activityLog, string adObjId)
        {
            //_dbContext.UserActivityLogs.Add(activityLog);
            //await _dbContext.SaveChangesAsync();
            //return activityLog;

            var currentUser = await _dbContext.UserProfiles.FirstOrDefaultAsync(p => p.AdObjId == adObjId);
            if (currentUser?.UserActivityLogs == null)
            {
                currentUser.UserActivityLogs = new List<UserActivityLog>();
            }
            currentUser.UserActivityLogs.Add(activityLog);
            await _dbContext.SaveChangesAsync();
            return activityLog;
        }

        public async Task<List<UserActivityLog>> GetUserActivityLogsAsync(int userId)
        {
            return await _dbContext.UserActivityLogs
                .Where(log => log.UserId == userId)
                .OrderByDescending(log => log.LogDate)
                .ToListAsync();
        }

        public async Task<List<UserActivityLog>?> GetUserActivityLogsAsync(string adObjId)
        {
            var user = await _dbContext.UserProfiles
                .Where(log => log.AdObjId == adObjId)
                .Include(l => l.UserActivityLogs)
               .FirstOrDefaultAsync();
            return user?.UserActivityLogs?.ToList();
        }
    }


}
