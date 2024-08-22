using QuanLiQuanCaphe.Models;
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

namespace QuanLiQuanCaphe.Views.Cart
{
    /// <summary>
    /// Interaction logic for DetailWindow.xaml
    /// </summary>
    public partial class DetailWindow : Window
    {
        private readonly CoffeeShopManagementContext dbContext;
        private int _productId;
        private decimal _productPrice;

        public DetailWindow()
        {
            InitializeComponent();
            dbContext = new CoffeeShopManagementContext(); 
        }

        public DetailWindow(int productId) : this() 
        {
            _productId = productId;
            LoadProductDetails(); 
        }

        private void LoadProductDetails()
        {
            var product = dbContext.Products.FirstOrDefault(p => p.ProductId == _productId);
            if (product != null)
            {
                txtpName.Text = product.Name; 
                txtDes.Text = product.Description; 
                _productPrice = product.Price; 
                totalPrice.Text = $"{_productPrice:#,##0}₫\"";

                var imagePath = $"pack://application:,,,/{product.Image}";
                productImage.Source = new BitmapImage(new Uri(imagePath));
            }
        }

        private void QuantityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (quantityComboBox.SelectedItem != null)
            {
                var selectedQuantity = int.Parse((quantityComboBox.SelectedItem as ComboBoxItem).Content.ToString());
                var total = selectedQuantity * _productPrice;
                totalPrice.Text = $"{total:#,##0}₫\"";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                    var checkoutWindow = new ShopWindow();
                    checkoutWindow.Show();
                    this.Close();
                
              
            }
        }
    }
}
