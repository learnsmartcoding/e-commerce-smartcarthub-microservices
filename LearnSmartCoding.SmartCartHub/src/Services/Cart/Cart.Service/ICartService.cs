using Carts.Core.Models;
using Carts.Data;

namespace Cart.Service
{
    public interface ICartService
    {
        Task<CartModel?> GetCartByIdAsync(int cartId);
        Task<List<CartModel>> GetCartsByUserIdAsync(string adObjId);
        Task<CartModel> AddToCartAsync(CartModel cart, string adObjId);
        Task<CartModel> UpdateCartAsync(CartModel cart);
        Task<bool> RemoveFromCartAsync(int cartId);
        Task<bool> IsCartIdValidAsync(int cartId, string adObjId);
        Task<bool> IsProductIdValidAsync(int productId);
    }

    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<CartModel?> GetCartByIdAsync(int cartId)
        {
            var cart = await _cartRepository.GetCartByIdAsync(cartId);
            return cart != null ? MapToCartModel(cart) : null;
        }

        public async Task<List<CartModel>> GetCartsByUserIdAsync(string adObjId)
        {
            var carts = await _cartRepository.GetCartsByUserIdAsync(adObjId);
            return carts.Select(MapToCartModel).ToList();
        }

        public async Task<CartModel> AddToCartAsync(CartModel cart, string adObjId)
        {
            var cartEntity = MapToCartEntity(cart);
            var addedCart = await _cartRepository.AddToCartAsync(cartEntity, adObjId);
            return MapToCartModel(addedCart);
        }

        public async Task<CartModel> UpdateCartAsync(CartModel cart)
        {
            var cartEntity = MapToCartEntity(cart);
            var updatedCart = await _cartRepository.UpdateCartAsync(cartEntity);
            return MapToCartModel(updatedCart);
        }

        public async Task<bool> RemoveFromCartAsync(int cartId)
        {
            return await _cartRepository.RemoveFromCartAsync(cartId);
        }

        public async Task<bool> IsCartIdValidAsync(int cartId, string adObjId)
        {
            // Assuming you have a method in the repository to check cart validity using adObjId
            return await _cartRepository.IsCartIdValidAsync(cartId, adObjId);
        }

        public async Task<bool> IsProductIdValidAsync(int productId)
        {
            // Implement the logic to check if the productId is valid
            // You might want to check if the product with the given ID exists in your database

            // Example:
            var product = await _cartRepository.GetProductByIdAsync(productId);
            return product != null;
        }

        private static CartModel MapToCartModel(Carts.Core.Entities.Cart cart)
        {
            return new CartModel
            {
                CartId = cart.CartId,
                UserId = cart.UserId,
                ProductId = cart.ProductId,
                Quantity = cart.Quantity
            };
        }

        private static Carts.Core.Entities.Cart MapToCartEntity(CartModel cartModel)
        {
            return new Carts.Core.Entities.Cart
            {
                CartId = cartModel.CartId,
                UserId = cartModel.UserId,
                ProductId = cartModel.ProductId,
                Quantity = cartModel.Quantity
            };
        }
    }

}
