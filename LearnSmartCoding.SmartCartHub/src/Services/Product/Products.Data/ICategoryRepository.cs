using Microsoft.EntityFrameworkCore;
using Products.Core.Entities;
using Products.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Data
{
    public interface ICategoryRepository
    {
        Task<List<CategoryModel>> GetAllCategoriesAsync();
        Task<CategoryModel?> GetCategoryByIdAsync(int categoryId);
        Task<CategoryModel> AddCategoryAsync(CategoryModel categoryModel);
        Task<CategoryModel> UpdateCategoryAsync(int categoryId, CategoryModel categoryModel);
        Task<bool> DeleteCategoryAsync(int categoryId);
    }
    public class CategoryRepository : ICategoryRepository
    {
        private readonly LearnSmartDbContext _dbContext;

        public CategoryRepository(LearnSmartDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CategoryModel>> GetAllCategoriesAsync()
        {
            // Implement logic to get all categories from the database
            return await _dbContext.Categories
                .Select(c => new CategoryModel
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName
                })
                .ToListAsync();
        }

        public async Task<CategoryModel?> GetCategoryByIdAsync(int categoryId)
        {
            // Implement logic to get a category by ID from the database
            var category = await _dbContext.Categories
                .Where(c => c.CategoryId == categoryId)
                .Select(c => new CategoryModel
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName
                })
                .FirstOrDefaultAsync();

            return category;
        }

        public async Task<CategoryModel> AddCategoryAsync(CategoryModel categoryModel)
        {
            // Implement logic to add a new category to the database
            var category = new Category
            {
                CategoryName = categoryModel.CategoryName
            };

            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync();

            categoryModel.CategoryId = category.CategoryId;
            return categoryModel;
        }

        public async Task<CategoryModel> UpdateCategoryAsync(int categoryId, CategoryModel categoryModel)
        {
            // Implement logic to update an existing category in the database
            var category = await _dbContext.Categories.FindAsync(categoryId);

            if (category != null)
            {
                category.CategoryName = categoryModel.CategoryName;
                await _dbContext.SaveChangesAsync();
            }

            return categoryModel;
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            // Implement logic to delete a category from the database
            var category = await _dbContext.Categories.FindAsync(categoryId);

            if (category != null)
            {
                _dbContext.Categories.Remove(category);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
