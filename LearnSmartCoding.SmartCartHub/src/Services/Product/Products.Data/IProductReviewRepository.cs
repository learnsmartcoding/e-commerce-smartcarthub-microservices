using Microsoft.EntityFrameworkCore;
using Products.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Data
{
    public interface IProductReviewRepository
    {
        Task<ProductReview?> GetReviewByIdAsync(int reviewId);
        Task<List<ProductReview>> GetReviewsByProductIdAsync(int productId);
        Task<ProductReview> AddReviewAsync(ProductReview review);
        Task<ProductReview> UpdateReviewAsync(ProductReview review);
        Task<bool> DeleteReviewAsync(int reviewId);
    }

    public class ProductReviewRepository : IProductReviewRepository
    {
        private readonly LearnSmartDbContext _dbContext;

        public ProductReviewRepository(LearnSmartDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProductReview?> GetReviewByIdAsync(int reviewId)
        {
            return await _dbContext.ProductReviews.FindAsync(reviewId);
        }

        public async Task<List<ProductReview>> GetReviewsByProductIdAsync(int productId)
        {
            return await _dbContext.ProductReviews
                .Where(r => r.ProductId == productId)
                .ToListAsync();
        }

        public async Task<ProductReview> AddReviewAsync(ProductReview review)
        {
            _dbContext.ProductReviews.Add(review);
            await _dbContext.SaveChangesAsync();
            return review;
        }

        public async Task<ProductReview> UpdateReviewAsync(ProductReview review)
        {
            _dbContext.Entry(review).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return review;
        }

        public async Task<bool> DeleteReviewAsync(int reviewId)
        {
            var review = await _dbContext.ProductReviews.FindAsync(reviewId);
            if (review != null)
            {
                _dbContext.ProductReviews.Remove(review);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }


}
