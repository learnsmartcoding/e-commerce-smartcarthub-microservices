using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using User.Core.Models;
using User.Service;
using User.Web.Common;

namespace User.Web.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/user-profiles")]
    [ApiController]
    [Authorize]
    public class UserProfileController(IUserProfileService userProfileService, IUserClaims userClaims) : ControllerBase
    {
        private readonly IUserProfileService _userProfileService = userProfileService ?? throw new ArgumentNullException(nameof(userProfileService));
        private readonly IUserClaims userClaims = userClaims;

        [HttpPost]        
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Write")]
        public async Task<ActionResult<UserProfileModel>> CreateUser([FromBody] UserProfileModel userProfileModel)
        {
            var createdUserProfile = await _userProfileService.CreateUserAsync(userProfileModel);
            return CreatedAtAction(nameof(GetUserById), new { userId = createdUserProfile.UserId }, createdUserProfile);
        }

        [HttpGet("{userId}")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Read")]
        public async Task<ActionResult<UserProfileModel>> GetUserById(int userId)
        {
            var userProfile = await _userProfileService.GetUserByIdAsync(userId);
            if (userProfile == null)
            {
                return NotFound();
            }

            return Ok(userProfile);
        }

        [HttpGet()]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Read")]
        public async Task<ActionResult<UserProfileModel>> GetCurrentUser()
        {
            var userAdObjId = userClaims.GetCurrentUserId();
            var userProfile = await _userProfileService.GetUserProfileAsync(userAdObjId);

            
            if (userProfile == null)
            {
                return NotFound();
            }

            return Ok(userProfile);
        }

        [HttpPut("{userId}")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Write")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserProfileModel userProfileModel)
        {
            if (userId != userProfileModel.UserId)
            {
                return BadRequest("User ID in the request body does not match the URL parameter.");
            }

            await _userProfileService.UpdateUserAsync(userProfileModel);
            return NoContent();
        }

        [HttpDelete("{userId}")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Write")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var isSuccess = await _userProfileService.DeleteUserAsync(userId);
            if (!isSuccess) {
                return NotFound();
            }

            return NoContent();
        }
    }
}
