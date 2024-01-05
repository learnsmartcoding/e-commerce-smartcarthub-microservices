using User.Core.Models;

namespace User.Service
{
    public interface IAddressService
    {
        Task<AddressModel> GetAddressByIdAsync(int addressId);
        Task<IEnumerable<AddressModel>> GetAddressesByUserIdAsync(int userId);
        Task<AddressModel> AddAddressAsync(AddressModel address);
        Task<AddressModel> UpdateAddressAsync(AddressModel address);
        Task<bool> DeleteAddressAsync(int addressId);
        Task<bool> IsAddressIdValidAsync(int addressId, int userId);
        Task<bool> IsAddressIdValidAsync(int addressId, string adObjId);

        Task<AddressModel> UpdateAddressAsync(AddressModel address, string adObjId);
        Task<List<AddressModel>> GetAddressesByAdObjIdAsync(string adObjId);
    }

}
