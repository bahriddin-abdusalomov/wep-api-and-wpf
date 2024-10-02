using Customer.Client.Managers;
using Customer.Client.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Customer.Client.Views
{
    /// <summary>
    /// Interaction logic for ProductDetailsWindow.xaml
    /// </summary>
    public partial class ProductDetailsWindow : Window
    {
        private Product _product;
        private int _quantity = 1;
        private List<Product> _products = new List<Product>();

        public ProductDetailsWindow(Product product)
        {
            InitializeComponent();
            _product = product;
            LoadProductDetails();
        }

        private void LoadProductDetails()
        {
            tbProductName.Text = _product.Name;
            tbProductDescription.Text = _product.Description;
            tbProductPrice.Text = $"Narxi: {_product.Price} so'm";

            if (!string.IsNullOrEmpty(_product.ImageUrl))
            {
                var baseAddress = "https://localhost:7084"; 
                var imageUrl = new Uri($"{baseAddress}/{_product.ImageUrl}", UriKind.Absolute);

                imgProduct.Source = new BitmapImage(imageUrl);
            }
        }

        private void DecreaseQuantityButton_Click(object sender, RoutedEventArgs e)
        {
            if (_quantity > 1)
            {
                _quantity--;
                tbQuantity.Text = _quantity.ToString();
            }
        }

        private void IncreaseQuantityButton_Click(object sender, RoutedEventArgs e)
        {
            _quantity++;
            tbQuantity.Text = _quantity.ToString();
        }

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            var product = new Product
            {
                ProductId = _product.ProductId,
                Name = _product.Name,
                Description = _product.Description,
                ImageUrl = _product.ImageUrl,
                Price = _product.Price,
                Quantity = _quantity,
                TotalPrice = _product.Price * _quantity,
                CategoryId = _product.CategoryId
            };

            CartManager.Instance.AddProduct(product);

            MessageBox.Show("Maxsulot savatga qo'shildi!");

            this.Hide();
        }

    }
}
