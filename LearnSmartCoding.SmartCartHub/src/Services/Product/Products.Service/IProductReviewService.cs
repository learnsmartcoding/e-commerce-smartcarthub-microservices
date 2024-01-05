using Products.Core.Entities;
using Products.Core.Models;
using Products.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Service
{
    public interface IProductReviewService
    {
        Task<ProductReviewModel?> GetReviewByIdAsync(int reviewId);
        Task<List<ProductReviewModel>> GetReviewsByProductIdAsync(int productId);
        Task<ProductReviewModel> AddReviewAsync(ProductReviewModel reviewModel);
        Task<ProductReviewModel> UpdateReviewAsync(ProductReviewModel reviewModel);
        Task<bool> DeleteReviewAsync(int reviewId);
    }

    public class ProductReviewService : IProductReviewService
    {
        private readonly IProductReviewRepository _productReviewRepository;

        public ProductReviewService(IProductReviewRepository productReviewRepository)
        {
            _productReviewRepository = productReviewRepository;
        }

        public async Task<ProductReviewModel?> GetReviewByIdAsync(int reviewId)
        {
            var review = await _productReviewRepository.GetReviewByIdAsync(reviewId);
            return review == null ? null : MapToModel(review);
        }

        public async Task<List<ProductReviewModel>> GetReviewsByProductIdAsync(int productId)
        {
            var reviews = await _productReviewRepository.GetReviewsByProductIdAsync(productId);
            return reviews.Select(MapToModel).ToList();
        }

        public async Task<ProductReviewModel> AddReviewAsync(ProductReviewModel reviewModel)
        {
            var reviewEntity = MapToEntity(reviewModel);
            var addedReview = await _productReviewRepository.AddReviewAsync(reviewEntity);
            return MapToModel(addedReview);
        }

        public async Task<ProductReviewModel> UpdateReviewAsync(ProductReviewModel reviewModel)
        {
            var existingReview = await _productReviewRepository.GetReviewByIdAsync(reviewModel.ReviewId);
            if (existingReview != null)
            {
                existingReview.Rating = reviewModel.Rating;
                existingReview.ReviewText = reviewModel.ReviewText;

                var updatedReview = await _productReviewRepository.UpdateReviewAsync(existingReview);
                return MapToModel(updatedReview);
            }

            // Handle the case when the review doesn't exist
            return null;
        }

        public async Task<bool> DeleteReviewAsync(int reviewId)
        {
            return await _productReviewRepository.DeleteReviewAsync(reviewId);
        }

        private ProductReviewModel MapToModel(ProductReview review)
        {
            // Implement mapping from entity to model
            // ...

            return new ProductReviewModel
            {
                // Map properties accordingly
                ReviewId = review.ReviewId,
                ProductId = review.ProductId ?? 0,
                UserId = review.UserId,
                Rating = review.Rating,
                ReviewText = review.ReviewText ?? string.Empty,
                ReviewDate = review.ReviewDate
            };
        }

        private ProductReview MapToEntity(ProductReviewModel reviewModel)
        {
            // Implement mapping from model to entity
            // ...

            return new ProductReview
            {
                // Map properties accordingly
                ReviewId = reviewModel.ReviewId,
                ProductId = reviewModel.ProductId,
                UserId = reviewModel.UserId,
                Rating = reviewModel.Rating,
                ReviewText = reviewModel.ReviewText,
                ReviewDate = reviewModel.ReviewDate
            };
        }
    }

}
