using Microsoft.AspNetCore.Http;

namespace Customer.Client.DTOs
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; } 
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
    }
}
