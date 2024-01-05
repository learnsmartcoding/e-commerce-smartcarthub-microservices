using Microsoft.EntityFrameworkCore;
using Products.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Data
{
    public  interface IUserProfileRepository
    {
        Task<UserProfile?> GetUserProfileAsync(string adObjId);
    }
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly LearnSmartDbContext _dbContext;

        public UserProfileRepository(LearnSmartDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<UserProfile?> GetUserProfileAsync(string adObjId)
        {
            return _dbContext.UserProfiles.FirstOrDefaultAsync(propa => propa.AdObjId == adObjId);
        }
    }
}
