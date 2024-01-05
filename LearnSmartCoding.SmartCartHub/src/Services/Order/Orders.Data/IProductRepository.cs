using Orders.Core.Entities;

namespace Orders.Data
{
    public interface IProductRepository
    {
        Task<Product?> GetProductByIdAsync(int productId);
        Task<List<Product>> GetProductByIdsAsync(List<int> productIds);
        Task<List<Product>> GetAllProductsAsync(int pageNumber, int pageSize, string sortBy);
        Task<List<Product>> GetAllProductsAsync();
        Task<List<Product>> SearchProductsAsync(string productName);
        Task<List<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<Product> AddProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int productId);

        Task<ProductImage?> GetProductImageByIdAsync(int imageId);
        Task<List<ProductImage>> GetImagesByProductIdAsync(int productId);
        Task<ProductImage> AddProductImageAsync(ProductImage image);
        Task<bool> DeleteProductImageAsync(int imageId);
    }

}
