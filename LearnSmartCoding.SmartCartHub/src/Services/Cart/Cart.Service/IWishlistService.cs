using Carts.Core.Entities;
using Carts.Core.Models;
using Carts.Data;

namespace Carts.Service
{
    public interface IWishlistService
    {
        Task<WishlistModel> GetWishlistByIdAsync(int wishlistId);
        Task<IEnumerable<WishlistModel>> GetWishlistByUserIdAsync(string adObjId);
        Task<int> AddToWishlistAsync(WishlistModel wishlistProduct, string adObjId);
        Task<WishlistModel> UpdateWishlistAsync(int wishlistId, WishlistModel wishlistProduct);
        Task<bool> RemoveFromWishlistAsync(int wishlistId);
        Task<bool> IsWishlistIdValidAsync(int wishlistId, int userId);
        Task<bool> IsWishlistIdValidAsync(int wishlistId, string adObjId);
        Task<bool> IsProductIdValidAsync(int productId);
    }


    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepository;       

        public WishlistService(IWishlistRepository wishlistRepository)
        {
            _wishlistRepository = wishlistRepository;
        }

        public async Task<WishlistModel?> GetWishlistByIdAsync(int wishlistId)
        {
            var wishlist = await _wishlistRepository.GetWishlistByIdAsync(wishlistId);
            return wishlist==null?null:WishlistEntityToModel(wishlist);
        }

        public async Task<IEnumerable<WishlistModel>> GetWishlistByUserIdAsync(string adObjId)
        {
            var wishlists = await _wishlistRepository.GetWishlistByUserIdAsync(adObjId);
            return wishlists.Select(WishlistEntityToModel);
        }

        public async Task<int> AddToWishlistAsync(WishlistModel wishlistProduct, string adObjId)
        {
            var wishlist = await _wishlistRepository.AddToWishlistAsync(WishlistModelToEntity(wishlistProduct), adObjId);
            return wishlist.WishlistId;
        }

        public async Task<bool> RemoveFromWishlistAsync(int wishlistId)
        {
            return await _wishlistRepository.RemoveFromWishlistAsync(wishlistId);
        }

        public async Task<bool> IsWishlistIdValidAsync(int wishlistId, int userId)
        {
            return await _wishlistRepository.IsWishlistIdValidAsync(wishlistId, userId);
        }

        public async Task<bool> IsWishlistIdValidAsync(int wishlistId, string adObjId)
        {
            return await _wishlistRepository.IsWishlistIdValidAsync(wishlistId, adObjId);
        }

        private WishlistModel WishlistEntityToModel(Wishlist wishlist)
        {
            return new WishlistModel
            {
                WishlistId = wishlist.WishlistId,
                UserId = wishlist.UserId ?? 0, // Assuming UserId is nullable, adjust accordingly
                ProductId = wishlist.ProductId ?? 0, // Assuming ProductId is nullable, adjust accordingly
                Product = ProductEntityToModel(wishlist.Product),
            };
        }

        private WishlistProductModel ProductEntityToModel(Product product)
        {
            return new WishlistProductModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.Price,
                Quantity = product.Quantity,
                CategoryId = product.CategoryId??0,
            };
        }

        private Wishlist WishlistModelToEntity(WishlistModel wishlistProduct)
        {
            return new Wishlist
            {                
                ProductId = wishlistProduct.ProductId
            };
        }

        public async Task<WishlistModel> UpdateWishlistAsync(int wishlistId, WishlistModel wishlistProduct)
        {
            var updatedWishlist = WishlistModelToEntity(wishlistProduct);
            updatedWishlist.WishlistId = wishlistId;

            var result = await _wishlistRepository.UpdateWishlistAsync(updatedWishlist);
            return WishlistEntityToModel(result);
        }

        public async Task<bool> IsProductIdValidAsync(int productId)
        {
            // Implement the logic to check if the productId is valid
            // This could involve checking against the database, business rules, etc.

            // Example: Check if the product with productId exists in the system
            return await _wishlistRepository.DoesProductExistAsync(productId);
        }
    }

}
