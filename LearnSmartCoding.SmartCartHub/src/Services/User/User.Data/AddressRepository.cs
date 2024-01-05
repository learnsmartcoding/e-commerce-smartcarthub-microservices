using Microsoft.EntityFrameworkCore;
using User.Core.Entities;
using User.Data;

namespace User.Data
{
    public class AddressRepository : IAddressRepository
    {
        private readonly LearnSmartDbContext _dbContext;

        public AddressRepository(LearnSmartDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Address> GetAddressByIdAsync(int addressId)
        {
            return await _dbContext.Addresses.FindAsync(addressId);
        }

        public async Task<List<Address>> GetAddressesByUserIdAsync(int userId)
        {
            return await _dbContext.Addresses.Where(a => a.UserId == userId).ToListAsync();
        }

        public async Task<List<Address>> GetAddressesByAdObjIdAsync(string adObjId)
        {
            return await _dbContext.UserProfiles
                .Where(u => u.AdObjId == adObjId)
                .SelectMany(u => u.Addresses)
                .ToListAsync();
        }


        public async Task<Address> AddAddressAsync(Address address)
        {
            _dbContext.Addresses.Add(address);
            await _dbContext.SaveChangesAsync();
            return address;
        }

        /// <summary>
        /// We should not use such thing as we have to be careful with userid, 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        [Obsolete]
        public async Task<Address> UpdateAddressAsync(Address address)
        {
            _dbContext.Entry(address).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return address;
        }

        public async Task<Address> UpdateAddressAsync(Address address, string adObjId)
        {
            var userProfile = await _dbContext.UserProfiles
                .Include(u => u.Addresses)  // Assuming Addresses is the collection navigation property
                .FirstOrDefaultAsync(u => u.AdObjId == adObjId);

            if (userProfile != null)
            {
                var existingAddress = userProfile.Addresses.FirstOrDefault(a => a.AddressId == address.AddressId);

                if (existingAddress != null)
                {
                    // Update properties of existing address
                    existingAddress.Street = address.Street;
                    existingAddress.City = address.City;
                    existingAddress.State = address.State;
                    existingAddress.ZipCode = address.ZipCode;
                    existingAddress.IsShippingAddress = address.IsShippingAddress;

                    
                    await _dbContext.SaveChangesAsync();
                    return existingAddress;
                }
            }

            // Return null if the user profile or address doesn't exist
            return null;
        }


        public async Task<bool> DeleteAddressAsync(int addressId)
        {
            var address = await _dbContext.Addresses.FindAsync(addressId);
            if (address == null)
            {
                return false; // Address not found
            }

            _dbContext.Addresses.Remove(address);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsAddressIdValidAsync(int addressId, int userId)
        {
            return await _dbContext.Addresses.AnyAsync(a => a.AddressId == addressId && a.UserId == userId);
        }

        public async Task<bool> IsAddressIdValidAsync(int addressId, string adObjId)
        {
            return await _dbContext.Addresses
                .Join(_dbContext.UserProfiles,
                      address => address.UserId,
                      user => user.UserId,
                      (address, user) => new { Address = address, User = user })
                .Where(result => result.Address.AddressId == addressId && result.User.AdObjId == adObjId)
                .AnyAsync();
        }

    }

}
