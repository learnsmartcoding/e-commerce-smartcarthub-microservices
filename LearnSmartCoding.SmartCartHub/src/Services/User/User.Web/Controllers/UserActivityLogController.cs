using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using User.Core.Entities;
using User.Core.Models;
using User.Service;
using User.Web.Common;

namespace User.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserActivityLogController(IUserActivityLogService service, IUserClaims userClaims) : ControllerBase
    {
        private readonly IUserActivityLogService _service = service;
        private readonly IUserClaims userClaims = userClaims;

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Write")]
        public async Task<ActionResult<UserActivityLogModel>> LogUserActivity(UserActivityLogModel activityLog)
        {            
            var adObjId= userClaims.GetCurrentUserId();
            var loggedActivity = await _service.LogUserActivityAsync(activityLog, adObjId);
            return CreatedAtAction(nameof(LogUserActivity), new { id = loggedActivity.UserId }, loggedActivity);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Read")]
        public async Task<ActionResult<List<UserActivityLogModel>>> GetUserActivityLogs(int userId)
        {
            var activityLogs = await _service.GetUserActivityLogsAsync(userId);

            if (activityLogs == null || activityLogs.Count == 0)
            {
                return NotFound();
            }

            return Ok(activityLogs);
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Read")]
        public async Task<ActionResult<List<UserActivityLogModel>>> GetLoggedInUserActivityLogs()
        {            
            var adObjId= userClaims.GetCurrentUserId();
            var activityLogs = await _service.GetUserActivityLogsAsync(adObjId);

            if (activityLogs == null)
            {
                return NotFound();
            }

            return Ok(activityLogs);
        }
    }

}
