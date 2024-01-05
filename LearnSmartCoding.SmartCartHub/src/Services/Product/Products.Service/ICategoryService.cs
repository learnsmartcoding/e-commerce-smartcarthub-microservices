using Products.Core.Models;
using Products.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Service
{
    public interface ICategoryService
    {
        Task<List<CategoryModel>> GetAllCategoriesAsync();
        Task<CategoryModel?> GetCategoryByIdAsync(int categoryId);
        Task<CategoryModel> AddCategoryAsync(CategoryModel categoryModel);
        Task<CategoryModel> UpdateCategoryAsync(int categoryId, CategoryModel categoryModel);
        Task<bool> DeleteCategoryAsync(int categoryId);
    }

    // Service Implementation
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryModel>> GetAllCategoriesAsync()
        {
            // Implement logic using _categoryRepository
            return await _categoryRepository.GetAllCategoriesAsync();
        }

        public async Task<CategoryModel?> GetCategoryByIdAsync(int categoryId)
        {
            // Implement logic using _categoryRepository
            return await _categoryRepository.GetCategoryByIdAsync(categoryId);
        }

        public async Task<CategoryModel> AddCategoryAsync(CategoryModel categoryModel)
        {
            // Implement logic using _categoryRepository
            return await _categoryRepository.AddCategoryAsync(categoryModel);
        }

        public async Task<CategoryModel> UpdateCategoryAsync(int categoryId, CategoryModel categoryModel)
        {
            // Implement logic using _categoryRepository
            return await _categoryRepository.UpdateCategoryAsync(categoryId, categoryModel);
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            // Implement logic using _categoryRepository
            return await _categoryRepository.DeleteCategoryAsync(categoryId);
        }
    }
}
