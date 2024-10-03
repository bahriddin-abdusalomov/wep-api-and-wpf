using Customer.Service.DTOs;
using Customer.Service.Models;

namespace Customer.Service.Interfaces
{
    public interface IProductService
    {
        Task<bool> CreateProductAsync(ProductDTO productDto);
        Task<List<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int productId, int categoryId);
        Task<bool> UpdateProductAsync(int productId, ProductDTO productDto);
        Task<bool> DeleteProductAsync(int productId);
    }
}
