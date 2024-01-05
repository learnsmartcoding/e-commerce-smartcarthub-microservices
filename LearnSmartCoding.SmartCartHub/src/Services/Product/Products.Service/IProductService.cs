using Products.Core.Models;

namespace Products.Service
{
    public interface IProductService
    {
        Task<ProductModel?> GetProductByIdAsync(int productId);
        Task<List<ProductModel>> GetAllProductsAsync(int pageNumber, int pageSize, string sortBy);
        Task<List<ProductModel>> GetAllProductsAsync();
        Task<List<ProductModel>> SearchProductsAsync(string productName);
        Task<List<ProductModel>> GetProductsByCategoryAsync(int categoryId);
        Task<ProductModel> AddProductAsync(ProductModel productModel);
        Task<ProductModel> UpdateProductAsync(ProductModel productModel);
        Task<bool> DeleteProductAsync(int productId);

        Task<ProductImageModel?> GetProductImageByIdAsync(int imageId);
        Task<List<ProductImageModel>> GetImagesByProductIdAsync(int productId);
        Task<ProductImageModel> AddProductImageAsync(ProductImageModel imageModel);
        Task<bool> DeleteProductImageAsync(int imageId);
    }

}
