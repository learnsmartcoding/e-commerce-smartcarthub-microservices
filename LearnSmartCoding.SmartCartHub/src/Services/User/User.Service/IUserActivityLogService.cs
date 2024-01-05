using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Core.Entities;
using User.Core.Models;
using User.Data;

namespace User.Service
{
    public interface IUserActivityLogService
    {
        Task<UserActivityLogModel> LogUserActivityAsync(UserActivityLogModel activityLog, string adObjId);
        Task<List<UserActivityLogModel>> GetUserActivityLogsAsync(int userId);
        Task<List<UserActivityLogModel>> GetUserActivityLogsAsync(string adObjId);
    }

    public class UserActivityLogService : IUserActivityLogService
    {
        private readonly IUserActivityLogRepository _repository;

        public UserActivityLogService(IUserActivityLogRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserActivityLogModel> LogUserActivityAsync(UserActivityLogModel activityLogModel, string adObjId)
        {
            var activityLog = MapToUserActivityLog(activityLogModel);
            var loggedActivity = await _repository.LogUserActivityAsync(activityLog, adObjId);
            return MapToUserActivityLogModel(loggedActivity);
        }

        public async Task<List<UserActivityLogModel>> GetUserActivityLogsAsync(int userId)
        {
            var activityLogs = await _repository.GetUserActivityLogsAsync(userId);
            return activityLogs.Select(MapToUserActivityLogModel).ToList();
        }

        public async Task<List<UserActivityLogModel>> GetUserActivityLogsAsync(string adObjId)
        {
            var activityLogs = await _repository.GetUserActivityLogsAsync(adObjId);
            return activityLogs.Select(MapToUserActivityLogModel).ToList();

        }
        private UserActivityLogModel MapToUserActivityLogModel(UserActivityLog activityLog)
        {
            return new UserActivityLogModel
            {
                UserId = activityLog.UserId,
                ActivityType = activityLog.ActivityType,
                ActivityDescription = activityLog.ActivityDescription,
                LogDate = activityLog.LogDate
            };
        }

        private UserActivityLog MapToUserActivityLog(UserActivityLogModel activityLog)
        {
            return new UserActivityLog
            {                
                ActivityType = activityLog.ActivityType,
                ActivityDescription = activityLog.ActivityDescription,
                LogDate =DateTime.Now
            };
        }
    }

}
