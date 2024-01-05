using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Core.Entities;
using User.Core.Models;

namespace User.Data
{
    public interface IAddressRepository
    {
        Task<Address> GetAddressByIdAsync(int addressId);
        Task<List<Address>> GetAddressesByUserIdAsync(int userId);
        Task<Address> AddAddressAsync(Address address);
        Task<Address> UpdateAddressAsync(Address address);
        Task<bool> DeleteAddressAsync(int addressId);
        Task<bool> IsAddressIdValidAsync(int addressId, string adObjId);
        Task<bool> IsAddressIdValidAsync(int addressId, int userId);//verifies whether the addressid belongs to the user
        Task<Address> UpdateAddressAsync(Address address, string adObjId);
        Task<List<Address>> GetAddressesByAdObjIdAsync(string adObjId);
    }
}
