using Orders.Core.Entities;
using Orders.Core.Models;
using Orders.Data;

namespace Orders.Service
{
    public interface IUserProfileService
    {       
        Task<UserProfileModel?> GetUserProfileAsync(string adObjId);
    }
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userRepository;

        public UserProfileService(IUserProfileRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        private UserProfileModel MapToUserProfileModel(UserProfile userProfileEntity)
        {
            return new UserProfileModel
            {
                UserId = userProfileEntity.UserId,
                DisplayName = userProfileEntity.DisplayName,
                FirstName = userProfileEntity.FirstName,
                LastName = userProfileEntity.LastName,
                Email = userProfileEntity.Email,
                AdObjId = userProfileEntity.AdObjId
                // Add other properties as needed
            };
        }

        public async Task<UserProfileModel?> GetUserProfileAsync(string adObjId)
        {
            var userProfileEntity = await _userRepository.GetUserProfileAsync(adObjId);
            return userProfileEntity != null ? MapToUserProfileModel(userProfileEntity) : null;

        }
    }
}
