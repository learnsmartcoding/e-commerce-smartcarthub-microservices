using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using User.Core.Models;
using User.Service;
using User.Web.Common;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace User.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController(IAddressService addressService, IUserProfileService userProfileService, IUserClaims userClaims) : ControllerBase
    {
        private readonly IAddressService _addressService = addressService;
        private readonly IUserProfileService userProfileService = userProfileService;
        private readonly IUserClaims userClaims = userClaims;

        [HttpGet("{addressId}")]
        [ProducesResponseType(typeof(AddressModel), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Read")]
        public async Task<IActionResult> GetAddressById(int addressId)
        {
            var address = await _addressService.GetAddressByIdAsync(addressId);

            if (address == null)
            {
                return NotFound();
            }

            return Ok(address);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(List<AddressModel>), 200)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Read")]
        public async Task<IActionResult> GetAddressesByUserId()
        {
            var userId = await GetUserProfieIdAsync();
            var addresses = await _addressService.GetAddressesByUserIdAsync(userId);
            return Ok(addresses);
        }

        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(List<AddressModel>), 200)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Read")]
        public async Task<IActionResult> GetAddressesByUserId(int userId)
        {            
            var addresses = await _addressService.GetAddressesByUserIdAsync(userId);
            return Ok(addresses);
        }


        [HttpPost]
        [ProducesResponseType(typeof(AddressModel), 201)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Write")]
        public async Task<IActionResult> AddAddress([FromBody] AddressModel addressModel)
        {
            addressModel.UserId = await GetUserProfieIdAsync();
            var addedAddress = await _addressService.AddAddressAsync(addressModel);
            return CreatedAtAction(nameof(GetAddressById), new { addressId = addedAddress.AddressId }, addedAddress);
        }

        [HttpPut]
        [ProducesResponseType(typeof(AddressModel), 200)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Write")]
        public async Task<IActionResult> UpdateAddress([FromBody] AddressModel addressModel)
        {
            var updatedAddress = await _addressService.UpdateAddressAsync(addressModel);

            if (updatedAddress == null)
            {
                return NotFound();
            }

            return Ok(updatedAddress);
        }

        [HttpDelete("{addressId}")]
        [ProducesResponseType(202)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Write")]
        public async Task<IActionResult> DeleteAddress(int addressId)
        {            
            var isDeleted = await _addressService.DeleteAddressAsync(addressId);

            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        //[HttpGet("{addressId}/validate/{userId}")]
        //[ProducesResponseType(typeof(bool), 200)]
        private async Task<IActionResult> IsAddressIdValid(int addressId, int userId)
        {
            var isValid = await _addressService.IsAddressIdValidAsync(addressId, userId);
            return Ok(isValid);
        }

        private async Task<int> GetUserProfieIdAsync()
        {
            var userAdObjId = userClaims.GetCurrentUserId();
            var userProfile= await userProfileService.GetUserProfileAsync(userAdObjId);
            return userProfile?.UserId??0;
        }
    }

}
