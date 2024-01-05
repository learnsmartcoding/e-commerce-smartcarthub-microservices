using Carts.Core.Entities;
using Carts.Data;
using Microsoft.EntityFrameworkCore;

namespace Carts.Data
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByIdAsync(int cartId);
        Task<List<Cart>> GetCartsByUserIdAsync(string adObjId);
        Task<Cart> AddToCartAsync(Cart cart, string adObjId);
        Task<Cart> UpdateCartAsync(Cart cart);
        Task<bool> RemoveFromCartAsync(int cartId);
        Task<bool> IsCartIdValidAsync(int cartId, string adObjId);
        Task<Product?> GetProductByIdAsync(int productId);
    }

    public class CartRepository : ICartRepository
    {
        private readonly LearnSmartDbContext _dbContext;

        public CartRepository(LearnSmartDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Cart?> GetCartByIdAsync(int cartId)
        {
            return await _dbContext.Carts
                .Include(c => c.Product)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CartId == cartId);
        }

        public async Task<List<Cart>> GetCartsByUserIdAsync(string adObjId)
        {
            return await _dbContext.Carts
                .Include(c => c.Product)
                .Include(c => c.User)
                .Where(c => c.User.AdObjId == adObjId)
                .ToListAsync();
        }

        public async Task<Cart> AddToCartAsync(Cart cart, string adObjId)
        {
            var userProfile = _dbContext.UserProfiles.FirstOrDefault(p => p.AdObjId == adObjId);
            if(userProfile.Carts==null)
                userProfile.Carts = new List<Cart>();

            userProfile.Carts.Add(cart);
            await _dbContext.SaveChangesAsync();
            return cart;
        }

        public async Task<Cart> UpdateCartAsync(Cart cart)
        {
            _dbContext.Carts.Update(cart);
            await _dbContext.SaveChangesAsync();
            return cart;
        }

        public async Task<bool> RemoveFromCartAsync(int cartId)
        {
            var cart = await _dbContext.Carts.FindAsync(cartId);
            if (cart != null)
            {
                _dbContext.Carts.Remove(cart);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> IsCartIdValidAsync(int cartId, string adObjId)
        {
            var isValid = await _dbContext.Carts
                .Join(_dbContext.UserProfiles,
                    cart => cart.UserId,
                    user => user.UserId,
                    (cart, user) => new { Cart = cart, User = user })
                .AnyAsync(joined => joined.Cart.CartId == cartId && joined.User.AdObjId == adObjId);

            return isValid;
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _dbContext.Products
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

    }

}
