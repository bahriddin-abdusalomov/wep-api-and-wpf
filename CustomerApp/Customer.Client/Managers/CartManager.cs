using Customer.Client.Models;
using System.Collections.Generic;

namespace Customer.Client.Managers
{
    public class CartManager
    {
        private static CartManager _instance;
        public static CartManager Instance => _instance ??= new CartManager();

        public List<Product> Products { get; private set; } = new List<Product>();

        private CartManager() { }

        public void AddProduct(Product product)
        {
            Products.Add(product);
        }

        public void ClearCart()
        {
            Products.Clear();
        }

        public decimal CalculateTotalPrice()
        {
            decimal totalPrice = 0;
            foreach (var product in Products)
            {
                totalPrice += product.Price * product.Quantity;
            }
            return totalPrice;
        }
    }
}
