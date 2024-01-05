using User.Core.Entities;
using User.Core.Models;

namespace User.Service
{
    public interface IUserProfileService
    {
        Task<UserProfileModel> CreateUserAsync(UserProfileModel userProfileModel);
        Task<UserProfileModel?> GetUserByIdAsync(int userId);        
        Task UpdateUserAsync(UserProfileModel userProfileModel);
        Task<bool> DeleteUserAsync(int userId);
        Task<UserProfileModel?> GetUserProfileAsync(string adObjId);
    }
}
