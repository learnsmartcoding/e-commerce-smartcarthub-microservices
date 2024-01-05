using Cart.Service;
using Carts.Core.Models;
using Carts.Web.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace Carts.Web.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/carts")]
    public class CartController(ICartService cartService, IUserClaims userClaims) : ControllerBase
    {
        private readonly ICartService _cartService = cartService;
        private readonly IUserClaims userClaims = userClaims;
        

        /// <summary>
        /// Get a specific cart by ID.
        /// </summary>
        /// <param name="cartId">The ID of the cart to retrieve.</param>
        /// <returns>The retrieved cart.</returns>
        [HttpGet("{cartId}")]
        [ProducesResponseType(typeof(CartModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Read")]
        public async Task<ActionResult<CartModel>> GetCartById(int cartId)
        {
            var adObjId = userClaims.GetCurrentUserId();
            var isValid = await _cartService.IsCartIdValidAsync(cartId, adObjId);
            
            if (!isValid)
            {
                return NotFound();
            }

            var cart = await _cartService.GetCartByIdAsync(cartId);
            return Ok(cart);
        }

        /// <summary>
        /// Get all carts for a specific user.
        /// </summary>
        /// <param name="adObjId">The AD object ID of the user.</param>
        /// <returns>The list of carts for the user.</returns>
        [HttpGet("user/{adObjId}")]
        [ProducesResponseType(typeof(List<CartModel>), StatusCodes.Status200OK)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Read")]
        public async Task<ActionResult<List<CartModel>>> GetCartsByUserId(string adObjId)
        {
            var carts = await _cartService.GetCartsByUserIdAsync(adObjId);
            return Ok(carts);
        }

        /// <summary>
        /// Get all carts for a specific user.
        /// </summary>
        /// <param name="adObjId">The AD object ID of the user.</param>
        /// <returns>The list of carts for the user.</returns>
        [HttpGet()]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Read")]
        [ProducesResponseType(typeof(List<CartModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<CartModel>>> GetCurrentUserCarts()
        {
            var adObjId = userClaims.GetCurrentUserId();
            var carts = await _cartService.GetCartsByUserIdAsync(adObjId);
            return Ok(carts);
        }

        /// <summary>
        /// Add a new item to the user's cart.
        /// </summary>
        /// <param name="cart">The cart item to add.</param>
        /// <param name="adObjId">The AD object ID of the user.</param>
        /// <returns>The added cart item.</returns>
        [HttpPost()]
        [ProducesResponseType(typeof(CartModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Write")]
        public async Task<ActionResult<CartModel>> AddToCart(CartModel cart)
        {
            var adObjId = userClaims.GetCurrentUserId();
            var isValid = await _cartService.IsProductIdValidAsync(cart.ProductId);

            if (!isValid)
            {
                ModelState.AddModelError("ProductId", "Invalid ProductId");
                return BadRequest(ModelState);
            }
            
            var addedCart = await _cartService.AddToCartAsync(cart, adObjId);
            
            return CreatedAtAction(nameof(GetCartById), new { cartId = addedCart.CartId }, addedCart);
        }

        /// <summary>
        /// Update an existing cart item.
        /// </summary>
        /// <param name="cart">The updated cart item.</param>
        /// <returns>The updated cart item.</returns>
        [HttpPut()]
        [ProducesResponseType(typeof(CartModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Write")]
        public async Task<ActionResult<CartModel>> UpdateCart(CartModel cart)
        {
            var adObjId = userClaims.GetCurrentUserId();
            var isValid = await _cartService.IsCartIdValidAsync(cart.CartId, adObjId);

            if (!isValid)
            {
                return NotFound();
            }

            var updatedCart = await _cartService.UpdateCartAsync(cart);
           
            return Ok(updatedCart);
        }

        /// <summary>
        /// Remove an item from the user's cart.
        /// </summary>
        /// <param name="cartId">The ID of the cart item to remove.</param>
        /// <returns>True if the item was successfully removed; otherwise, false.</returns>
        [HttpDelete("{cartId}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Write")]
        public async Task<ActionResult<bool>> RemoveFromCart(int cartId)
        {
            var adObjId = userClaims.GetCurrentUserId();
            var isValid = await _cartService.IsCartIdValidAsync(cartId, adObjId);

            if (!isValid)
            {
                return NotFound();
            }

            var removed = await _cartService.RemoveFromCartAsync(cartId);
            return Ok(removed);
        }

        
    }

}
