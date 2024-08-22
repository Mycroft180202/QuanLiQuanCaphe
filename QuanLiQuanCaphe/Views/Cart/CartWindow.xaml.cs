using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for CartWindow.xaml
    /// </summary>
    public partial class CartWindow : Window
    {
        private readonly CoffeeShopManagementContext _dbContext;
        private readonly int _currentUserId;

        public CartWindow(int currentUserId)
        {
            InitializeComponent();
            _dbContext = new CoffeeShopManagementContext();
            _currentUserId = currentUserId;
            LoadCartItems();  
            DisplayCurrentDate();
            DisplayBillDetails();
            UpdateTotalPrice();
        }

        public void LoadCartItems()
        {
            var cartItems = _dbContext.Carts
                .Where(c => c.UserId == _currentUserId)
                .Select(c => new QuanLiQuanCaphe.Models.Cart
                {
                    CartId = c.CartId,
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    Product = c.Product
                })
                .ToList();

            if (cartItems.Count == 0)
            {
                MessageBox.Show("No items found in the cart for this user.");
            }
            else
            {
                CartListView.ItemsSource = cartItems;
            }

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var bookingWindow = new ShopWindow();
            bookingWindow.Show();
            this.Close();
        }
        private void DisplayCurrentDate()
        {
            txtDateBill.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        public void DisplayBillDetails()
        {
            var cartItems = _dbContext.Carts
                .Where(c => c.UserId == _currentUserId)
                .Select(c => new
                {
                    c.ProductId,
                    c.Product.Category.CategoryId,
                    c.Quantity
                })
                .ToList();

            if (cartItems.Count == 0)
            {
                txtBillNumber.Text = "No items in cart.";
                return;
            }

            var categoryIds = cartItems.Select(item => item.CategoryId).Distinct();
            var productIds = cartItems.Select(item => item.ProductId);
            var totalQuantity = cartItems.Sum(item => item.Quantity);

            txtBillNumber.Text = $"{_currentUserId}" +
                $"{string.Join("", categoryIds)}" +
                $"{string.Join("", productIds)}" +
                $"{totalQuantity}";
        }

        private void TxtQuality_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CartListView.SelectedItem is QuanLiQuanCaphe.Models.Cart selectedCart)
            {
                if (int.TryParse(txtQuality.Text, out int newQuantity))
                {
                    if (newQuantity > 0)
                    {
                        selectedCart.Quantity = newQuantity;

                        UpdateTotalPrice();
                    }
                    else
                    {
                        MessageBox.Show("Quantity must be greater than 0.");
                    }
                }

            }

        }


        private void CartListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            if (CartListView.SelectedItem is QuanLiQuanCaphe.Models.Cart selectedCart)
            {
                txtQuality.Text = selectedCart.Quantity.ToString();
            }

        }

        private void Button_Click_add(object sender, RoutedEventArgs e)
        {
            if (CartListView.SelectedItem is QuanLiQuanCaphe.Models.Cart selectedCart)
            {
                if (int.TryParse(txtQuality.Text, out int newQuantity) && newQuantity > 0)
                {
                    try
                    {
                        var cartInDb = _dbContext.Carts
                            .FirstOrDefault(c => c.CartId == selectedCart.CartId);

                        if (cartInDb == null)
                        {
                            MessageBox.Show("The selected cart item could not be found in the database.");
                            return;
                        }

                        cartInDb.Quantity = newQuantity;
                        int changes = _dbContext.SaveChanges();

                        if (changes > 0)
                        {
                            LoadCartItems();
                            UpdateTotalPrice();

                            MessageBox.Show("Quantity updated successfully.");
                        }
                        else
                        {
                            MessageBox.Show("No changes were saved.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("Quantity must be greater than 0 and a valid number.");
                }
            }
            else
            {
                MessageBox.Show("No item selected in the cart.");
            }
        }






        private void Delete_Oder(object sender, RoutedEventArgs e)
        {
            if (CartListView.SelectedItem is QuanLiQuanCaphe.Models.Cart selectedCart)
            {
                var cartItemToRemove = _dbContext.Carts
                    .FirstOrDefault(c => c.ProductId == selectedCart.ProductId && c.UserId == _currentUserId);

                if (cartItemToRemove != null)
                {
                    _dbContext.Carts.Remove(cartItemToRemove);
                    _dbContext.SaveChanges();

                    LoadCartItems();
                    UpdateTotalPrice();


                    MessageBox.Show("Item removed from cart.");
                }
                else
                {
                    MessageBox.Show("Item not found in the cart.");
                }
            }
            else
            {
                MessageBox.Show("Please select an item to delete.");
            }
        }


        private void UpdateTotalPrice()
        {
            var totalPrice = _dbContext.Carts
                .Where(c => c.UserId == _currentUserId)
                .Sum(c => c.Quantity * c.Product.Price);

            totalprice.Text = $"{totalPrice:#,##0}₫";
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Button_Click_2 clicked.");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string keyword = txtSeach.Text.ToLower().Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Please enter a search term.", "Search", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                var filteredCarts = _dbContext.Carts
                    .Where(c => c.Product.Name.ToLower().Contains(keyword) && c.UserId == _currentUserId)
                    .ToList();

                if (filteredCarts.Any())
                {
                    CartListView.ItemsSource = filteredCarts;
                }
                else
                {
                    MessageBox.Show("No items found matching your search.", "Search Results", MessageBoxButton.OK, MessageBoxImage.Information);
                    CartListView.ItemsSource = null; 
                }
            }

        }

    }
}

