using Carts.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carts.Data
{
    public interface IWishlistRepository
    {
        Task<Wishlist?> GetWishlistByIdAsync(int wishlistId);
        Task<List<Wishlist>> GetWishlistByUserIdAsync(string adObjId);
        Task<Wishlist> AddToWishlistAsync(Wishlist wishlist, string adObjId);
        Task<Wishlist> UpdateWishlistAsync(Wishlist wishlist);
        Task<bool> RemoveFromWishlistAsync(int wishlistId);
        Task<bool> IsWishlistIdValidAsync(int wishlistId, int userId);
        Task<bool> IsWishlistIdValidAsync(int wishlistId, string adObjId);
        Task<bool> DoesProductExistAsync(int productId);
    }

    public class WishlistRepository : IWishlistRepository
    {
        private readonly LearnSmartDbContext _dbContext;

        public WishlistRepository(LearnSmartDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Wishlist?> GetWishlistByIdAsync(int wishlistId)
        {
            return await _dbContext.Wishlists
                .Include(w => w.Product)
                .Include(w => w.User)
                .FirstOrDefaultAsync(w => w.WishlistId == wishlistId);
        }

        public async Task<List<Wishlist>> GetWishlistByUserIdAsync(string adObjId)
        {
            return await _dbContext.Wishlists
                .Include(w => w.Product)
                    .ThenInclude(p => p.Category)  // Include the Category for each Product
                .Include(w => w.User)
                .Where(w => w.User.AdObjId == adObjId)
                .ToListAsync();
        }


        public async Task<Wishlist?> AddToWishlistAsync(Wishlist wishlist, string adObjId)
        {
            var userProfile = _dbContext.UserProfiles.FirstOrDefault(p => p.AdObjId == adObjId);
            if (userProfile.Wishlists == null)
                userProfile.Wishlists = new List<Wishlist>();

            userProfile.Wishlists.Add(wishlist);
            await _dbContext.SaveChangesAsync();
            return wishlist;
        }

        public async Task<Wishlist?> UpdateWishlistAsync(Wishlist wishlist)
        {
            _dbContext.Wishlists.Update(wishlist);
            await _dbContext.SaveChangesAsync();
            return wishlist;
        }

        public async Task<bool> RemoveFromWishlistAsync(int wishlistId)
        {
            var wishlist = await _dbContext.Wishlists.FindAsync(wishlistId);
            if (wishlist != null)
            {
                _dbContext.Wishlists.Remove(wishlist);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> IsWishlistIdValidAsync(int wishlistId, int userId)
        {
            return await _dbContext.Wishlists.AnyAsync(w => w.WishlistId == wishlistId && w.UserId == userId);
        }

        public async Task<bool> IsWishlistIdValidAsync(int wishlistId, string adObjId)
        {
            var isValid = await _dbContext.Wishlists
               .Join(_dbContext.UserProfiles,
                   cart => cart.UserId,
                   user => user.UserId,
                   (cart, user) => new { Cart = cart, User = user })
               .AnyAsync(joined => joined.Cart.WishlistId == wishlistId && joined.User.AdObjId == adObjId);

            return isValid;
        }

        public async Task<bool> DoesProductExistAsync(int productId)
        {
            // Implement the logic to check if the product with productId exists in the database
            return await _dbContext.Products.AnyAsync(p => p.ProductId == productId);
        }
    }


}
