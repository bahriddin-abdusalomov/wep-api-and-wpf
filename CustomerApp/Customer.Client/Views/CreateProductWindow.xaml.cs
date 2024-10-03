using Customer.Client.DTOs;
using Customer.Client.Models;

using Microsoft.Win32;

using Newtonsoft.Json;

using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Customer.Client.Views
{
    public partial class CreateProductWindow : Window
    {
        private List<Category> _categories;
        private byte[] uploadedImage;
        private HttpClient _client;

        public CreateProductWindow()
        {
            InitializeComponent();
            _client = new HttpClient();
            LoadCategories();
        }

        private async void LoadCategories()
        {
            try
            {
                var response = await _client.GetStringAsync("https://localhost:7084/api/Categories");
                _categories = JsonConvert.DeserializeObject<List<Category>>(response);
                cbCategories.ItemsSource = _categories;
                cbCategories.DisplayMemberPath = "Name";
                cbCategories.SelectedValuePath = "CategoryId";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kategoriya yuklashda xatolik: {ex.Message}", "Xato", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cbCategories_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }

        private void UploadImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                string imgPath = openFileDialog.FileName;
                Image.ImageSource = new BitmapImage(new Uri(imgPath, UriKind.Relative));
            }
        }

        private async void SaveProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbName.Text) ||
                string.IsNullOrWhiteSpace(tbDescription.Text) ||
                decimal.Parse(tbPrice.Text) <= 0 ||
                int.Parse(tbQuantity.Text) <= 0 ||
                (int)(cbCategories.SelectedValue ?? 0) <= 0 ||
                Image.ImageSource == null)
            {
                MessageBox.Show("Iltimos, barcha maydonlarni to'ldiring!", "Xatolik", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newProduct = new ProductDTO
            {
                Name = tbName.Text,
                Description = tbDescription.Text,
                Price = decimal.TryParse(tbPrice.Text, out decimal price) ? price : 0,
                Quantity = int.TryParse(tbQuantity.Text, out int quantity) ? quantity : 0,
                CategoryId = (int)(cbCategories.SelectedValue ?? 0),
                Image = Image.ImageSource.ToString()
            };

            try
            {
                await PostProductAsync(newProduct);
                MessageBox.Show("Mahsulot muvaffaqiyatli saqlandi!", "Ma'lumot", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
                ProductListWindow productListWindow = new ProductListWindow();
                productListWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Mahsulotni saqlashda xatolik yuz berdi: {ex.Message}", "Xato", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task PostProductAsync(ProductDTO product)
        {
            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StringContent(product.Name), nameof(ProductDTO.Name));
                content.Add(new StringContent(product.Description), nameof(ProductDTO.Description));
                content.Add(new StringContent(product.Price.ToString()), nameof(ProductDTO.Price));
                content.Add(new StringContent(product.Quantity.ToString()), nameof(ProductDTO.Quantity));
                content.Add(new StringContent(product.CategoryId.ToString()), nameof(ProductDTO.CategoryId));
                content.Add(new StreamContent(File.OpenRead(product.Image)), "Image", product.Image);

                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7084/api/Products")
                {
                    Content = content
                };
                var response = await _client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Serverdan xato javob olindi: {errorMessage}");
                }
            }
        }
    }
}
