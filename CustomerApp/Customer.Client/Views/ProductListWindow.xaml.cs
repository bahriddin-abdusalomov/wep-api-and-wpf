using Customer.Client.Models;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
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
    public partial class ProductListWindow : Window
    {
        private HttpClient _client;
        private List<Category> _categories;

        public ProductListWindow()
        {
            InitializeComponent();
            _client = new HttpClient();
            LoadCategories();
        }

        private async void LoadCategories()
        {
            var response = await _client.GetStringAsync("https://localhost:7084/api/Categories");
            _categories = JsonConvert.DeserializeObject<List<Category>>(response);
            cbCategories.ItemsSource = _categories;

            cbCategories.ItemsSource = _categories;
            cbCategories.DisplayMemberPath = "Name"; 
            cbCategories.SelectedValuePath = "CategoryId";
            cbCategories.SelectedIndex = 0; 
        }

        private async void cbCategories_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cbCategories.SelectedItem is Category selectedCategory)
            {
                var response = await _client.GetStringAsync($"https://localhost:7084/api/Products/{selectedCategory.CategoryId}");
                var products = JsonConvert.DeserializeObject<List<Product>>(response);
                dgProducts.ItemsSource = products;
            }
        }

        private async void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            // Tanlangan mahsulotni olish
            var button = sender as Button;
            var product = button?.DataContext as Product;

            if (product != null)
            {
                try
                {
                    var productDetails = await _client.GetStringAsync($"https://localhost:7084/api/Products/{product.ProductId}/{product.CategoryId}");
                    var productView = JsonConvert.DeserializeObject<Product>(productDetails);

                    if (productView != null)
                    {
                        var productDetailsWindow = new ProductDetailsWindow(productView);
                        productDetailsWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Mahsulot topilmadi.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Xato yuz berdi: {ex.Message}");
                }
            }
        }

        private async void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var product = button?.DataContext as Product;

            if (product != null)
            {
                try
                {
                    var response = await _client.DeleteAsync($"https://localhost:7084/api/Products/{product.ProductId}");

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Mahsulot o'chirildi!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Mahsulotni o'chirishda hatolik bor: {response.ReasonPhrase}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hatolik: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            CreateProductWindow createProductWindow = new CreateProductWindow();
            createProductWindow.ShowDialog();
            this.Close();
        }

        private void dgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}