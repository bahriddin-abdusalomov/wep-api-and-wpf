using Customer.Service.Data;
using Customer.Service.DTOs;
using Customer.Service.Interfaces;
using Customer.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Customer.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CreateProduct(ProductDTO productDto)
        {
            if (productDto == null)
            {
                return BadRequest("Invalid product data.");
            }

            var result = await _productService.CreateProductAsync(productDto);

            if (result)
            {
                return Ok("Product created successfully.");
            }
            else
            {
                return StatusCode(500, "An error occurred while creating the product.");
            }
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult<bool>> DeleteProduct(int productId)
        {
            var result = await _productService.DeleteProductAsync(productId);

            if (result)
            {
                return Ok("Product deleted successfully.");
            }
            else
            {
                return NotFound("Product not found.");
            }
        }

        [HttpGet("{productId}/{categoryId}")]
        public async Task<ActionResult<Product>> GetProductById(int productId, int categoryId)
        {
            var product = await _productService.GetProductByIdAsync(productId, categoryId);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            return Ok(product);
        }

        [HttpGet("{categoryId}")]
        public async Task<ActionResult<List<Product>>> GetProductsByCategory(int categoryId)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);

            if (products == null || !products.Any())
            {
                return Ok(new List<Product>());
            }

            return Ok(products);
        }
    }
}
