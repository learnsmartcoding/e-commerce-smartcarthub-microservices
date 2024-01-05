using Carts.Core.Models;
using Carts.Service;
using Carts.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Carts.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize] // Assuming wishlist operations require authentication
    public class WishlistController(IWishlistService wishlistService, IUserClaims userClaims) : ControllerBase
    {
        private readonly IWishlistService _wishlistService = wishlistService;
        private readonly IUserClaims userClaims = userClaims;

        [HttpGet("{wishlistId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WishlistModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWishlistById(int wishlistId)
        {
            var wishlist = await _wishlistService.GetWishlistByIdAsync(wishlistId);
            if (wishlist == null)
                return NotFound();

            return Ok(wishlist);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WishlistModel>))]
        public async Task<IActionResult> GetWishlistByUserId()
        {
            var wishlist = await _wishlistService.GetWishlistByUserIdAsync(userClaims.GetCurrentUserId());
            return Ok(wishlist);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(WishlistModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddToWishlist([FromBody] WishlistModel wishlistProduct)
        {
            var validProduct = await wishlistService.IsProductIdValidAsync(wishlistProduct.ProductId);
            if (!validProduct)
            {
                ModelState.AddModelError("ProductId", "Invalid ProductId");
                return BadRequest(ModelState);
            }

            var wishlistId = await _wishlistService.AddToWishlistAsync(wishlistProduct, userClaims.GetCurrentUserId());
            wishlistProduct.WishlistId = wishlistId;
            return CreatedAtAction(nameof(GetWishlistById), new { wishlistId = wishlistId }, wishlistProduct);
        }

        [HttpPut("{wishlistId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WishlistModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateWishlist(int wishlistId, [FromBody] WishlistModel wishlistProduct)
        {
            //TODO validate this wishlist with current user for security
            var validProduct = await wishlistService.IsProductIdValidAsync(wishlistProduct.ProductId);
            if (!validProduct)
            {
                ModelState.AddModelError("ProductId", "Invalid ProductId");
                return BadRequest(ModelState);
            }

            var wishlist = await _wishlistService.UpdateWishlistAsync(wishlistId, wishlistProduct);
            return Ok(wishlist);
        }

        [HttpDelete("{wishlistId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveFromWishlist(int wishlistId)
        {
            var isRemoved = await _wishlistService.RemoveFromWishlistAsync(wishlistId);
            if (isRemoved)
                return Ok();

            return NotFound();
        }

    }

}
