using Customer.Client.DTOs;
using Customer.Client.Managers;
using Customer.Client.Models;

using Newtonsoft.Json;

using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Customer.Client.Views
{
    /// <summary>
    /// Interaction logic for ProductDetailsWindow.xaml
    /// </summary>
    public partial class ProductDetailsWindow : Window
    {
        private Product _product;
        private HttpClient _client;
        private int _quantity = 1;
        private List<Product> _products = new List<Product>();

        public ProductDetailsWindow(Product product)
        {
            InitializeComponent();
            _client = new HttpClient();
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
            if (_quantity < _product.Quantity)
            {
                _quantity++;
                tbQuantity.Text = _quantity.ToString();
            }
        }

        private async void CartButton_Click(object sender, RoutedEventArgs e)
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

            var productDto = new ProductDTO
            {
                Quantity = _product.Quantity - _quantity,
            };

            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StringContent(productDto.Quantity.ToString()), nameof(ProductDTO.Quantity));

                var request = new HttpRequestMessage(HttpMethod.Put, $"https://localhost:7084/api/Products/{product.ProductId}")
                {
                    Content = content
                };
                var response = await _client.SendAsync(request);
            }

            CartManager.Instance.AddProduct(product);

            MessageBox.Show("Maxsulot savatga qo'shildi!");
            this.Close();
        }
    }
}
