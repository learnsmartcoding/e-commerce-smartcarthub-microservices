using AutoMapper;
using Orders.Core.Entities;
using Orders.Core.Models;
using Orders.Data;

namespace Orders.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductModel?> GetProductByIdAsync(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            return _mapper.Map<ProductModel?>(product);
        }

        public async Task<List<ProductModel>> GetAllProductsAsync(int pageNumber, int pageSize, string sortBy)
        {
            var products = await _productRepository.GetAllProductsAsync(pageNumber, pageSize, sortBy);
            return _mapper.Map<List<ProductModel>>(products);
        }

        public async Task<List<ProductModel>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return _mapper.Map<List<ProductModel>>(products);
        }

        public async Task<List<ProductModel>> SearchProductsAsync(string productName)
        {
            var products = await _productRepository.SearchProductsAsync(productName);
            return _mapper.Map<List<ProductModel>>(products);
        }

        public async Task<List<ProductModel>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _productRepository.GetProductsByCategoryAsync(categoryId);
            return _mapper.Map<List<ProductModel>>(products);
        }

        public async Task<ProductModel> AddProductAsync(ProductModel productModel)
        {
            var product = _mapper.Map<Product>(productModel);
            var addedProduct = await _productRepository.AddProductAsync(product);
            return _mapper.Map<ProductModel>(addedProduct);
        }

        public async Task<ProductModel> UpdateProductAsync(ProductModel productModel)
        {
            var product = _mapper.Map<Product>(productModel);
            var updatedProduct = await _productRepository.UpdateProductAsync(product);
            return _mapper.Map<ProductModel>(updatedProduct);
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            return await _productRepository.DeleteProductAsync(productId);
        }

        public async Task<ProductImageModel?> GetProductImageByIdAsync(int imageId)
        {
            var image = await _productRepository.GetProductImageByIdAsync(imageId);
            return _mapper.Map<ProductImageModel?>(image);
        }

        public async Task<List<ProductImageModel>> GetImagesByProductIdAsync(int productId)
        {
            var images = await _productRepository.GetImagesByProductIdAsync(productId);
            return _mapper.Map<List<ProductImageModel>>(images);
        }

        public async Task<ProductImageModel> AddProductImageAsync(ProductImageModel imageModel)
        {
            var image = _mapper.Map<ProductImage>(imageModel);
            var addedImage = await _productRepository.AddProductImageAsync(image);
            return _mapper.Map<ProductImageModel>(addedImage);
        }

        public async Task<bool> DeleteProductImageAsync(int imageId)
        {
            return await _productRepository.DeleteProductImageAsync(imageId);
        }

        public async Task<List<int>> GetInvalidProductIdsAsync(List<int> productIds)
        {
            var existingProductIds = (await _productRepository.GetProductByIdsAsync(productIds))
                                .Select(p => p.ProductId)
                                .ToList();

            var invalidProductIds = productIds.Except(existingProductIds).ToList();

            return invalidProductIds;

        }
    }

}
