using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Orders.Core.Models;
using Orders.Service;
using Orders.Web.Common;
using Orders.Web.ViewModels;

namespace Orders.Web.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Produces("application/json")]
    public class OrderController(IOrderService orderService, 
        IUserClaims userClaims, IMapper mapper,
        IUserProfileService userProfileService) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;
        private readonly IUserClaims userClaims = userClaims;
        private readonly IMapper mapper = mapper;
        private readonly IUserProfileService userProfileService = userProfileService;

        ///// <summary>
        ///// Get an order by ID.
        ///// </summary>
        ///// <param name="orderId">The ID of the order.</param>
        ///// <returns>The order.</returns>
        [HttpGet("{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Read")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            OrderModel order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        ///// <summary>
        ///// Get orders for a specific user.
        ///// </summary>
        ///// <param name="userId">The ID of the user.</param>
        ///// <returns>The list of orders for the user.</returns>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OrderModel>))]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Read")]
        public async Task<IActionResult> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        /// <summary>
        /// Get orders for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The list of orders for the user.</returns>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OrderModel>))]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Read")]
        public async Task<IActionResult> GetOrdersByCurrentUserAsync()
        {
            var userId = await GetUserIdAsync();
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }


        /// <summary>
        /// Add a new order.
        /// </summary>
        /// <param name="orderModel">The order details.</param>
        /// <returns>The created order.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Write")]
        public async Task<IActionResult> AddOrderAsync([FromBody] CreateOrder model)
        {            
            model.UserId = await GetUserIdAsync();
            
            var orderModel = mapper.Map<OrderModel>(model);
            var addedOrder = await _orderService.AddOrderAsync(orderModel);
            return CreatedAtAction(nameof(GetOrderById), new { orderId = addedOrder.OrderId }, addedOrder);
        }

        /*
        // Ideally order cannot be updated by user, Admin or backend team can do it and needs special permission and work flow
        /// <summary>
        /// Update an existing order.
        /// </summary>
        /// <param name="orderId">The ID of the order to be updated.</param>
        /// <param name="orderModel">The updated order details.</param>
        /// <returns>The updated order.</returns>
        [HttpPut("{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrderAsync(int orderId, [FromBody] OrderModel orderModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedOrder = await _orderService.UpdateOrderAsync(orderModel);
            if (updatedOrder == null)
            {
                return NotFound();
            }

            return Ok(updatedOrder);
        }
        */

        /*
        // Order cannot be deleted, instead order status can be marked as cancelled.
        /// <summary>
        /// Delete an order by ID.
        /// </summary>
        /// <param name="orderId">The ID of the order to be deleted.</param>
        /// <returns>No content if successfully deleted, NotFound if order not found.</returns>
        [HttpDelete("{orderId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrderAsync(int orderId)
        {
            var isDeleted = await _orderService.DeleteOrderAsync(orderId);
            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }
        */

        [HttpPatch("cancel/{orderId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Write")]
        public async Task<IActionResult> CancelOrderAsync(int orderId)
        {
            var isCancelled = await _orderService.CancelOrderAsync(orderId);
            if (!isCancelled)
            {
                return NotFound();
            }

            return NoContent();
        }

        private async Task<int> GetUserIdAsync()
        {
            var user = await userProfileService.GetUserProfileAsync(userClaims.GetCurrentUserId());
            return user?.UserId ?? 0;
        }
    }
}
