using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Products.Core.Models;
using Products.Service;
using Products.Web.Common;

namespace Products.Web.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [Route("api/productreviews")]
    [Authorize]
    public class ProductReviewsController(IProductReviewService productReviewService, IUserClaims userClaims, 
        IUserProfileService userProfileService) : ControllerBase
    {
        private readonly IProductReviewService _productReviewService = productReviewService;
        private readonly IUserClaims userClaims = userClaims;
        private readonly IUserProfileService userProfileService = userProfileService;

        [HttpGet("{reviewId}")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:ProductReviewRead")]
        public async Task<IActionResult> GetReviewByIdAsync(int reviewId)
        {
            var review = await _productReviewService.GetReviewByIdAsync(reviewId);
            if (review == null)
            {
                return NotFound();
            }
            return Ok(review);
        }

        [HttpGet("product/{productId}")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:ProductReviewRead")]
        public async Task<IActionResult> GetReviewsByProductIdAsync(int productId)
        {
            var reviews = await _productReviewService.GetReviewsByProductIdAsync(productId);
            return Ok(reviews);
        }

        [HttpPost]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:ProductReviewWrite")]
        public async Task<IActionResult> AddReviewAsync([FromBody] ProductReviewModel reviewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var profile = await userProfileService.GetUserProfileAsync(userClaims.GetCurrentUserId());
            reviewModel.UserId = profile?.UserId;
            var addedReview = await _productReviewService.AddReviewAsync(reviewModel);
            //return CreatedAtAction(nameof(GetReviewByIdAsync), new { reviewId = addedReview.ReviewId }, addedReview);
            return Ok(addedReview);

        }

        [HttpPut]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:ProductReviewWrite")]
        public async Task<IActionResult> UpdateReviewAsync([FromBody] ProductReviewModel reviewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var profile = await userProfileService.GetUserProfileAsync(userClaims.GetCurrentUserId());
            reviewModel.UserId = profile?.UserId;

            var updatedReview = await _productReviewService.UpdateReviewAsync(reviewModel);

            if (updatedReview == null)
            {
                return NotFound();
            }
            return Ok(updatedReview);
        }

        [HttpDelete("{reviewId}")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:ProductReviewWrite")]
        public async Task<IActionResult> DeleteReviewAsync(int reviewId)
        {
            var isDeleted = await _productReviewService.DeleteReviewAsync(reviewId);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }

}
