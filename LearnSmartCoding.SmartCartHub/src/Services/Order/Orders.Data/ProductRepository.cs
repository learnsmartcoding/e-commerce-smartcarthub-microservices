using Microsoft.EntityFrameworkCore;
using Orders.Core.Entities;

namespace Orders.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly LearnSmartDbContext _dbContext;

        public ProductRepository(LearnSmartDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _dbContext.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task<List<Product>> GetAllProductsAsync(int pageNumber, int pageSize, string sortBy)
        {
            var query = _dbContext.Products
                .Include(p => p.ProductImages)
                .AsQueryable();

            // Sorting logic
            switch (sortBy.ToLower())
            {
                case "productname":
                    query = query.OrderBy(p => p.ProductName);
                    break;
                case "price":
                    query = query.OrderBy(p => p.Price);
                    break;
                // Add more sorting options as needed
                default:
                    // Default sorting by ProductId or any other suitable default
                    query = query.OrderBy(p => p.ProductId);
                    break;
            }

            // Pagination logic
            var paginatedProducts = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return paginatedProducts;
        }


        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _dbContext.Products
                .Include(p => p.ProductImages)
                .ToListAsync();
        }

        public async Task<List<Product>> SearchProductsAsync(string productName)
        {
            return await _dbContext.Products
                .Include(p => p.ProductImages)
                .Where(p => EF.Functions.Like(p.ProductName, $"%{productName}%"))
                .ToListAsync();
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _dbContext.Products
                .Include(p => p.ProductImages)
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            var existingProduct = await GetProductByIdAsync(product.ProductId);
            if (existingProduct != null)
            {
                existingProduct.ProductName = product.ProductName;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.Price = product.Price;
                existingProduct.Quantity = product.Quantity;

                // Check if the incoming product has images
                if (product.ProductImages != null)
                {
                    // If the existing product does not have images, initialize the list
                    if (existingProduct.ProductImages == null)
                    {
                        existingProduct.ProductImages = new List<ProductImage>();
                    }

                    // Add or update images
                    foreach (var updatedImage in product.ProductImages)
                    {
                        var existingImage = existingProduct.ProductImages.FirstOrDefault(pi => pi.ImageId == updatedImage.ImageId);

                        if (existingImage != null)
                        {
                            // Update existing image
                            existingImage.ImageUrl = updatedImage.ImageUrl;
                        }
                        else
                        {
                            // Add new image
                            existingProduct.ProductImages.Add(updatedImage);
                        }
                    }

                    // Remove images that are no longer associated with the product
                    var imagesToRemove = existingProduct.ProductImages
                        .Where(existingImage => !product.ProductImages.Any(updatedImage => updatedImage.ImageId == existingImage.ImageId))
                        .ToList();

                    foreach (var image in imagesToRemove)
                    {
                        existingProduct.ProductImages.Remove(image);
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
            return existingProduct;
        }


        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product != null)
            {
                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<ProductImage?> GetProductImageByIdAsync(int imageId)
        {
            return await _dbContext.ProductImages.FindAsync(imageId);
        }

        public async Task<List<ProductImage>> GetImagesByProductIdAsync(int productId)
        {
            return await _dbContext.ProductImages
                .Where(pi => pi.ProductId == productId)
                .ToListAsync();
        }

        public async Task<ProductImage> AddProductImageAsync(ProductImage image)
        {
            _dbContext.ProductImages.Add(image);
            await _dbContext.SaveChangesAsync();
            return image;
        }

        public async Task<bool> DeleteProductImageAsync(int imageId)
        {
            var image = await _dbContext.ProductImages.FindAsync(imageId);
            if (image != null)
            {
                _dbContext.ProductImages.Remove(image);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public Task<List<Product>> GetProductByIdsAsync(List<int> productIds)
        {
            return _dbContext.Products.Where(w=> productIds.Contains(w.ProductId)).ToListAsync();
        }
    }

}
