using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Product_Management_System.Repositories;
using Product_Management_System.Repositories.Authentication;
using Product_Management_System.Views;
using Product_Management_System.Views.Admin;
using QuanLiQuanCaphe.Models;
using QuanLiQuanCaphe.Repositories;
using QuanLiQuanCaphe.Views.Authentication;
using QuanLiQuanCaphe.Views.Cart;

namespace Product_Management_System
{
    public partial class MainWindow : Window
    {
        private readonly CoffeeShopManagementContext dbContext;
        private readonly User currentUser;
        private readonly IUserRepository userRepository;
        private Table selectedTable;
        private decimal totalAmount;

        public MainWindow(User user = null)
        {
            InitializeComponent();
            dbContext = new CoffeeShopManagementContext();
            userRepository = new UserRepository(dbContext);
            currentUser = user ?? userRepository.GetUserByUsername("default_username");
            SetupUIBasedOnUserRole();
            LoadUserInfo();
            LoadTablesAsync(); // Load tables asynchronously
            LoadProductsAsync(); // Load products asynchronously
        }

        private void SetupUIBasedOnUserRole()
        {
            if (currentUser != null && currentUser.RoleId == 1) // Admin
            {
                btnDashboard.Visibility = Visibility.Visible;
                btnManage.Visibility = Visibility.Visible;
            }
            if (currentUser != null && currentUser.RoleId == 2) // Staff
            {
                btnManage.Visibility = Visibility.Visible;
            }
        }

        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            var adminDashboard = new AdminDashboardWindow();
            adminDashboard.Show();
        }

        private void LoadUserInfo()
        {
            txtUserFullName.Text = currentUser?.FullName ?? "Unknown User";
        }

        private void txtUserFullName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (currentUser != null && !string.IsNullOrEmpty(currentUser.FullName))
            {
                var userInfoWindow = new UserInfoWindow(currentUser, userRepository)
                {
                    Owner = this
                };
                var result = userInfoWindow.ShowDialog();

                if (result == true)
                {
                    txtUserFullName.Text = currentUser.FullName;
                }
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private async Task LoadTablesAsync()
        {
            var tables = await Task.Run(() => dbContext.Tables.ToList());
            foreach (var table in tables)
            {
                var tableButton = new Button
                {
                    Content = table.Name,
                    Width = 100,
                    Height = 100,
                    Margin = new Thickness(5),
                    Background = (bool)table.IsOccupied ? Brushes.Red : Brushes.Green
                };
                tableButton.Click += TableButton_Click;
                tablesPanel.Children.Add(tableButton);
            }
        }

        private async Task LoadProductsAsync()
        {
            List<Product> products;
            using (var context = new CoffeeShopManagementContext())
            {
                products = context.Products.ToList();
            }

            lstProducts.Items.Clear();
            foreach (var product in products)
            {
                var item = new ListBoxItem
                {
                    Content = $"{product.Name} - {product.Price:C}",
                    Tag = product // Store the product object for later use
                };
                lstProducts.Items.Add(item);
            }
        }

        private void lstProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Recalculate the total amount
            CalculateTotalAmount();
        }

        private void CalculateTotalAmount()
        {
            totalAmount = 0; // Reset the total amount

            foreach (ListBoxItem selectedItem in lstProducts.SelectedItems)
            {
                var product = (Product)selectedItem.Tag;
                totalAmount += product.Price; // Add the price of each selected product
            }

            // Update the total amount in the UI
            txtTotalAmount.Text = totalAmount.ToString("C");
        }


        private void TableButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var tableName = button.Content.ToString();
                selectedTable = dbContext.Tables.FirstOrDefault(t => t.Name == tableName);

                if (selectedTable != null)
                {
                    if (selectedTable.IsOccupied == true)
                    {
                        MessageBox.Show($"Bàn {selectedTable.Name} đã có người đặt rồi. Vui lòng chọn bàn khác.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        txtSelectedTable.Text = $"Bàn {selectedTable.Name}";
                        txtUserName.Text = currentUser.FullName;
                        txtOrderDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        totalAmount = 0;
                        txtTotalAmount.Text = totalAmount.ToString("C");
                        cmbPaymentMethod.SelectedIndex = 0;
                        MessageBox.Show($"Bạn đã chọn bàn {selectedTable.Name}.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        private void AddDrinkToOrder(Product product, int quantity)
        {
            totalAmount += product.Price * quantity;
            txtTotalAmount.Text = totalAmount.ToString("C");
        }

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTable == null)
            {
                MessageBox.Show("Vui lòng chọn bàn trước khi đặt hàng!");
                return;
            }

            if (lstProducts.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một sản phẩm!");
                return;
            }

            var order = new Order
            {
                UserId = currentUser.UserId,
                OrderDate = DateTime.Now,
                Status = "Pending",
                TotalAmount = totalAmount,
                PaymentMethod = cmbPaymentMethod.Text,
                TableId = selectedTable.Id
            };

            dbContext.Orders.Add(order);
            dbContext.SaveChanges(); // Save to generate OrderId

            foreach (ListBoxItem selectedItem in lstProducts.SelectedItems)
            {
                var product = (Product)selectedItem.Tag;

                var orderDetail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = product.ProductId,
                    Price = product.Price,
                    Quantity = 1 // Default quantity for now; you can adjust this as needed
                };

                dbContext.OrderDetails.Add(orderDetail);
            }

            dbContext.SaveChanges();

            selectedTable.IsOccupied = true;
            dbContext.Tables.Update(selectedTable);
            dbContext.SaveChanges();

            UpdateTableButtonColor(selectedTable.Name, Brushes.Red);
            MessageBox.Show("Đơn hàng đã được đặt thành công!");
            ClearOrderDetails();
        }

        private void UpdateTableButtonColor(string tableName, Brush color)
        {
            foreach (Button button in tablesPanel.Children.OfType<Button>())
            {
                if (button.Content.ToString() == tableName)
                {
                    button.Background = color;
                    break;
                }
            }
        }

        private void ClearOrderDetails()
        {
            txtUserName.Text = string.Empty;
            txtOrderDate.Text = string.Empty;
            txtTotalAmount.Text = string.Empty;
            lstProducts.SelectedItems.Clear();
        }

        private void btnManage_Click(object sender, RoutedEventArgs e)
        {
            // Ensure that only Admin and Staff can access the Manage functionality
            if (currentUser != null && (currentUser.RoleId == 1 || currentUser.RoleId == 2)) // Admin or Staff
            {
                var manageOrdersWindow = new ManageOrdersWindow();
                manageOrdersWindow.RefreshTables = RefreshTableColors;
                manageOrdersWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này!");
            }
        }
        private void RefreshTableColors()
        {
            Dispatcher.Invoke(() =>
            {
                using (var context = new CoffeeShopManagementContext())
                {
                    foreach (Button button in tablesPanel.Children.OfType<Button>())
                    {
                        var tableName = button.Content.ToString();
                        var table = context.Tables.FirstOrDefault(t => t.Name == tableName);

                        if (table != null)
                        {
                            // Update the button color based on the table's occupancy status
                            button.Background = table.IsOccupied == true ? Brushes.Red : Brushes.Green;
                        }
                    }
                }
            });
        }

    }
}
