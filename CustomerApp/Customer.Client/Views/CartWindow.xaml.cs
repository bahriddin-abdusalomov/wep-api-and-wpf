using Customer.Client.Managers;
using Customer.Client.Models;
using System.Windows;
using System.Windows.Controls;

namespace Customer.Client.Views
{
    public partial class CartWindow : Window
    {
        public CartWindow()
        {
            InitializeComponent();
            lvProducts.ItemsSource = CartManager.Instance.Products; 
            CalculateTotalPrice();
        }

        private void CalculateTotalPrice()
        {
            decimal totalPrice = CartManager.Instance.CalculateTotalPrice();
            tbTotalPrice.Text = $"Umumiy Narx: {totalPrice} so'm"; 
        }

        private void lvProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
