using Microsoft.EntityFrameworkCore;
using User.Core.Entities;

namespace User.Data
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly LearnSmartDbContext _dbContext;

        public UserProfileRepository(LearnSmartDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserProfile> CreateUserAsync(UserProfile userProfile)
        {
            _dbContext.UserProfiles.Add(userProfile);
            await _dbContext.SaveChangesAsync();
            return userProfile;
        }

        public async Task<UserProfile?> GetUserByIdAsync(int userId)
        {
            return await _dbContext.UserProfiles.FindAsync(userId);
        }

        public async Task<IEnumerable<UserProfile>> GetAllUsersAsync()
        {
            return await _dbContext.UserProfiles.AsNoTracking().ToListAsync();
        }

        public async Task UpdateUserAsync(UserProfile userProfile)
        {
            _dbContext.Entry(userProfile).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var userProfile = await _dbContext.UserProfiles.FindAsync(userId);
            if (userProfile != null)
            {
                _dbContext.UserProfiles.Remove(userProfile);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public Task<UserProfile?> GetUserProfileAsync(string adObjId)
        {
            return _dbContext.UserProfiles.FirstOrDefaultAsync(propa=>propa.AdObjId == adObjId);
        }

        public Task<UserProfile?> GetUserByAdObIdAsync(string adObjId)
        {
            return _dbContext.UserProfiles.FirstOrDefaultAsync(propa => propa.AdObjId == adObjId);
        }

        /*
        public async Task<bool> DeleteUserAsync(int userId)
        {
            var userProfile = await _dbContext.UserProfiles
                .Include(u => u.Addresses)  // Include related addresses
                .Include(u => u.UserActivityLogs)  // Include related user activity logs
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (userProfile != null)
            {
                // Check if there are associated addresses or user activity logs
                if (userProfile.Addresses.Any() || userProfile.UserActivityLogs.Any())
                {
                    // Associated data exists, don't delete the user
                    return false;
                }

                // No associated data, proceed with deletion
                _dbContext.UserProfiles.Remove(userProfile);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            // User not found
            return false;
        }
        */
    }
}
