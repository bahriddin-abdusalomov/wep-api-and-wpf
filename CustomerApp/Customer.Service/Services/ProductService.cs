using AutoMapper;
using Customer.Service.Data;
using Customer.Service.DTOs;
using Customer.Service.Interfaces;
using Customer.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Customer.Service.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _dbContext;
        private readonly IFileService _fileService;
        private readonly ILogger<ProductService> _logger;
        private readonly IMapper _mapper;

        public ProductService(AppDbContext dbContext, 
            IFileService fileService, 
            ILogger<ProductService> logger,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _fileService = fileService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> CreateProductAsync(ProductDTO productDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);
                 
                if (productDto.Image != null) 
                {
                    product.ImageUrl = await _fileService.UploadImageAsync(productDto.Image);
                }

                await _dbContext.Products.AddAsync(product);
                int result = await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Product '{product.Name}' created successfully with ID: {product.ProductId}");
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating product: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            try
            {
                var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
                if (product == null)
                {
                    _logger.LogWarning($"Product with ID {productId} not found.");
                    return false; 
                }

                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    var imageDeleted = await _fileService.DeleteImageAsync(product.ImageUrl);
                    if (!imageDeleted)
                    {
                        _logger.LogWarning($"Failed to delete image for product ID {productId}.");
                    }
                }

                _dbContext.Products.Remove(product);
                var result = await _dbContext.SaveChangesAsync();

                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting product with ID {productId}: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            try
            {
                var products = await _dbContext.Products.ToListAsync();
                return products; 
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving products: {ex.Message}");
                return new List<Product>(); 
            }
        }

        public async Task<Product> GetProductByIdAsync(int productId, int categoryId)
        {
            try
            {
                var product = await _dbContext.Products
                    .Include(p => p.Category) 
                    .Where(p => p.ProductId == productId && p.CategoryId == categoryId)
                    .FirstOrDefaultAsync();

                if (product == null)
                {
                    _logger.LogWarning($"Product with ID {productId} not found.");
                    return new Product();
                }

                return product; 
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving product with ID {productId}: {ex.Message}");
                return new Product(); 
            }
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            try
            {
                var products = await _dbContext.Products
                    .Where(p => p.CategoryId == categoryId)
                    .ToListAsync();

                return products; 
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving products for category ID {categoryId}: {ex.Message}");
                return new List<Product>(); 
            }
        }

        public async Task<bool> UpdateProductAsync(int productId, ProductDTO productDto)
        {
            try
            {
                var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
                if (product == null)
                {
                    _logger.LogWarning($"Product with ID {productId} not found.");
                    return false; 
                }
                product.Quantity = (int)productDto.Quantity;

                _dbContext.Products.Update(product);
                var result = await _dbContext.SaveChangesAsync();

                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating product with ID {productId}: {ex.Message}");
                return false;
            }
        }
    }
}
