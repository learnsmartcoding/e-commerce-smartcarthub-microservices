using User.Core.Entities;
using User.Core.Models;
using User.Data;

namespace User.Service
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userRepository;

        public UserProfileService(IUserProfileRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<UserProfileModel> CreateUserAsync(UserProfileModel userProfileModel)
        {
            var userProfileEntity = MapToUserProfileEntity(userProfileModel);
            var createdUserProfile = await _userRepository.CreateUserAsync(userProfileEntity);
            return MapToUserProfileModel(createdUserProfile);
        }

        public async Task<UserProfileModel?> GetUserByIdAsync(int userId)
        {
            var userProfileEntity = await _userRepository.GetUserByIdAsync(userId);
            return userProfileEntity != null ? MapToUserProfileModel(userProfileEntity) : null;
        }

        public async Task<IEnumerable<UserProfileModel>> GetAllUsersAsync()
        {
            var userProfiles = await _userRepository.GetAllUsersAsync();
            return userProfiles.Select(MapToUserProfileModel);
        }

        public async Task UpdateUserAsync(UserProfileModel userProfileModel)
        {
            var userProfileEntity = MapToUserProfileEntity(userProfileModel);
            await _userRepository.UpdateUserAsync(userProfileEntity);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            return await _userRepository.DeleteUserAsync(userId);
        }

        private UserProfile MapToUserProfileEntity(UserProfileModel userProfileModel)
        {
            return new UserProfile
            {
                UserId = userProfileModel.UserId,
                DisplayName = userProfileModel.DisplayName,
                FirstName = userProfileModel.FirstName,
                LastName = userProfileModel.LastName,
                Email = userProfileModel.Email,
                AdObjId = userProfileModel.AdObjId
                // Add other properties as needed
            };
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
