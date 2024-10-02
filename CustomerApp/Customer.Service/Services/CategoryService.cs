using Customer.Service.Data;
using Customer.Service.DTOs;
using Customer.Service.Interfaces;
using Customer.Service.Models;

using Microsoft.EntityFrameworkCore;

namespace Customer.Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<CategoryService> _logger;
        public CategoryService(AppDbContext dbContext, ILogger<CategoryService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> CreateCategoryAsync(CategoryDTO categoryDto)
        {
            try
            {
                var category = new Category
                {
                    Name = categoryDto.Name
                };

                await _dbContext.Categories.AddAsync(category);
                int result = await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"New category created: {category.Name}");

                return result > 0; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating category.");
                return false; 
            }
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _dbContext.Categories.Include(p => p.Products).ToListAsync();

                _logger.LogInformation("All categories retrieved successfully.");

                return categories; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving categories.");
                return new List<Category>(); 
            }
        }
    }
}
