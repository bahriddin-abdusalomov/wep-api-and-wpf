using Customer.Client.DTOs;
using Customer.Client.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Customer.Client.Views
{
    public partial class CreateProductWindow : Window
    {
        private readonly HttpClient _client;
        private ProductDTO _productDto;
        private List<Category> _categories;
        private string SelectedImagePath { get; set; }

        public CreateProductWindow()
        {
            InitializeComponent();
            _client = new HttpClient();
            _productDto = new ProductDTO();
            LoadCategoriesAsync();
        }

        private async void LoadCategoriesAsync()
        {
            var response = await _client.GetStringAsync("https://localhost:7084/api/Categories");
            _categories = JsonConvert.DeserializeObject<List<Category>>(response);
            cbCategories.ItemsSource = _categories;
            cbCategories.DisplayMemberPath = "Name";
            cbCategories.SelectedValuePath = "CategoryId";
        }

        private void cbCategories_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cbCategories.SelectedItem is Category selectedCategory)
            {
                _productDto.CategoryId = selectedCategory.CategoryId;
            }
        }

        private void UploadImageButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                SelectedImagePath = openFileDialog.FileName;
                imgPreview.Source = new BitmapImage(new Uri(SelectedImagePath));
            }
        }

        private async void SaveProductButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CollectProductData();
                var response = await PostProductAsync();

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Mahsulot muvaffaqiyatli yuklandi!");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Xatolik: {response.ReasonPhrase}\n\n{errorContent}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Xatolik: {ex.Message}");
            }
        }

        private void CollectProductData()
        {
            _productDto = new ProductDTO
            {
                Name = tbName.Text,
                Description = tbDescription.Text,
                Price = decimal.Parse(tbPrice.Text),
                Quantity = int.Parse(tbQuantity.Text),
                CategoryId = (int)cbCategories.SelectedValue
            };
        }

        private async Task<HttpResponseMessage> PostProductAsync()
        {
            using (var form = new MultipartFormDataContent())
            {
                if (!string.IsNullOrEmpty(SelectedImagePath))
                {
                    var formFile = CreateFormFileFromImage(SelectedImagePath); 
                    _productDto.Image = formFile; 
                }
                var productJson = JsonConvert.SerializeObject(_productDto);
                form.Add(new StringContent(productJson, Encoding.UTF8, "application/json"), "productDto");

                return await _client.PostAsJsonAsync("https://localhost:7084/api/Products", _productDto);
            }
        }

        private IFormFile CreateFormFileFromImage(string filePath)
        {
            var imageBytes = File.ReadAllBytes(filePath);
            var stream = new MemoryStream(imageBytes);
            var formFile = new FormFile(stream, 0, imageBytes.Length, Path.GetFileName(filePath), Path.GetFileName(filePath))
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            return formFile;
        }
    }
}
