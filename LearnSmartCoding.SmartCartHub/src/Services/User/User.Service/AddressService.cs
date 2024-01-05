using User.Core.Entities;
using User.Core.Models;
using User.Data;

namespace User.Service
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<AddressModel> GetAddressByIdAsync(int addressId)
        {
            var address = await _addressRepository.GetAddressByIdAsync(addressId);
            return MapAddressToModel(address);
        }

        public async Task<IEnumerable<AddressModel>> GetAddressesByUserIdAsync(int userId)
        {
            var addresses = await _addressRepository.GetAddressesByUserIdAsync(userId);
            return addresses.Select(MapAddressToModel);
        }

        public async Task<AddressModel> AddAddressAsync(AddressModel addressModel)
        {
            var address = MapModelToAddress(addressModel);
            var addedAddress = await _addressRepository.AddAddressAsync(address);
            return MapAddressToModel(addedAddress);
        }

        public async Task<AddressModel> UpdateAddressAsync(AddressModel addressModel, string adObjId)
        {
            var address = MapModelToAddress(addressModel);
            var updatedAddress = await _addressRepository.UpdateAddressAsync(address, adObjId);
            return MapAddressToModel(updatedAddress);
        }

        public async Task<List<AddressModel>> GetAddressesByAdObjIdAsync(string adObjId)
        {
            var addresses = await _addressRepository.GetAddressesByAdObjIdAsync(adObjId);
            return addresses.Select(MapAddressToModel).ToList();
        }

        public async Task<AddressModel> UpdateAddressAsync(AddressModel addressModel)
        {
            var address = MapModelToAddress(addressModel);
            var updatedAddress = await _addressRepository.UpdateAddressAsync(address);
            return MapAddressToModel(updatedAddress);
        }

        public async Task<bool> DeleteAddressAsync(int addressId)
        {
            return await _addressRepository.DeleteAddressAsync(addressId);
        }

        public async Task<bool> IsAddressIdValidAsync(int addressId, int userId)
        {
            return await _addressRepository.IsAddressIdValidAsync(addressId, userId);
        }

        public async Task<bool> IsAddressIdValidAsync(int addressId, string adObjId)
        {
            return await _addressRepository.IsAddressIdValidAsync(addressId, adObjId);
        }

        private AddressModel MapAddressToModel(Address address)
        {
            if (address == null)
            {
                return null;
            }

            return new AddressModel
            {
                AddressId = address.AddressId,
                UserId = address.UserId,
                Street = address.Street,
                City = address.City,
                State = address.State,
                ZipCode = address.ZipCode,
                IsShippingAddress = address.IsShippingAddress
            };
        }

        private Address MapModelToAddress(AddressModel addressModel)
        {
            if (addressModel == null)
            {
                return null;
            }

            return new Address
            {
                AddressId = addressModel.AddressId,
                UserId = addressModel.UserId,
                Street = addressModel.Street,
                City = addressModel.City,
                State = addressModel.State,
                ZipCode = addressModel.ZipCode,
                IsShippingAddress = addressModel.IsShippingAddress
            };
        }

        
    }

}
