using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Core.Entities;

namespace User.Data
{
    public interface IUserProfileRepository
    {
        // Create a new user profile
        Task<UserProfile> CreateUserAsync(UserProfile userProfile);

        // Get a user profile by ID
        Task<UserProfile?> GetUserByIdAsync(int userId);

        // Get a user profile by ID
        Task<UserProfile?> GetUserByAdObIdAsync(string adObjId);

        // Get all user profiles
        Task<IEnumerable<UserProfile>> GetAllUsersAsync();

        // Update an existing user profile
        Task UpdateUserAsync(UserProfile userProfile);

        // Delete a user profile by ID
        Task<bool> DeleteUserAsync(int userId);

        Task<UserProfile?> GetUserProfileAsync(string adObjId);
    }
}
