using Customer.Service.DTOs;
using Customer.Service.Models;

namespace Customer.Service.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategoriesAsync();
        Task<bool> CreateCategoryAsync(CategoryDTO categoryDto);
    }
}
