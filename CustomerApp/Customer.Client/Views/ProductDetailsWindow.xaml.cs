using Customer.Client.Models;

using System;
using System.Collections.Generic;
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

            // Rasm URL manzilidan (server yoki mahalliy) olingan rasmni o'rnatish
            if (!string.IsNullOrEmpty(_product.ImageUrl))
            {
                var uri = new Uri(_product.ImageUrl, UriKind.RelativeOrAbsolute);
                imgProduct.Source = new BitmapImage(uri);
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

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"{_quantity} ta {_product.Name} mahsuloti sotib olindi!", "Sotib olish muvaffaqiyatli!");
        }
    }
}
