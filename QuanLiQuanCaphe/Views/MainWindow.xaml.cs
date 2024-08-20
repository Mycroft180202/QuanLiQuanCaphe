using Product_Management_System.Repositories;
using Product_Management_System.Repositories.Authentication;
using Product_Management_System.Views.Admin;
using QuanLiQuanCaphe.Models;
using QuanLiQuanCaphe.Repositories;
using QuanLiQuanCaphe.Views.Authentication;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
        }

        private void SetupUIBasedOnUserRole()
        {
            if (currentUser != null && currentUser.RoleId == 1) // Admin
            {
                btnDashboard.Visibility = Visibility.Visible;
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

        private void TableButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                // Lấy tên bàn từ nội dung nút
                var tableName = button.Content.ToString();

                // Tìm bàn trong cơ sở dữ liệu dựa trên tên
                selectedTable = dbContext.Tables.FirstOrDefault(t => t.Name == tableName);

                if (selectedTable != null)
                {
                    // Hiển thị thông tin người dùng hiện tại
                    txtUserName.Text = currentUser.FullName;

                    // Lấy thời gian hiện tại
                    txtOrderDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                    // Xóa các món cũ và đặt tổng tiền về 0
                    lstOrderDetails.Items.Clear();
                    totalAmount = 0;
                    txtTotalAmount.Text = totalAmount.ToString("C");

                    // Đặt phương thức thanh toán mặc định
                    cmbPaymentMethod.SelectedIndex = 0;
                }
            }
        }

        // Sự kiện thêm đồ uống (giả sử có sẵn danh sách đồ uống để chọn)
        private void AddDrinkToOrder(string drinkName, decimal price, int quantity)
        {
            totalAmount += price * quantity;
            txtTotalAmount.Text = totalAmount.ToString("C");
            lstOrderDetails.Items.Add($"{drinkName} - {quantity} x {price:C}");
        }

        // Sự kiện khi bấm nút Order
        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTable != null)
            {
                var order = new Order
                {
                    UserId = currentUser.UserId,
                    OrderDate = DateTime.Now,
                    Status = "Active",
                    TotalAmount = totalAmount,
                    PaymentMethod = cmbPaymentMethod.Text,
                    TableId = selectedTable.Id
                };

                dbContext.Orders.Add(order);
                dbContext.SaveChanges();

                MessageBox.Show("Đơn hàng đã được đặt thành công!");
                ClearOrderDetails(); // Sau khi đặt hàng xong thì xóa các thông tin
            }
            else
            {
                MessageBox.Show("Vui lòng chọn bàn trước khi đặt hàng!");
            }
        }

        private void ClearOrderDetails()
        {
            txtUserName.Text = string.Empty;
            txtOrderDate.Text = string.Empty;
            txtTotalAmount.Text = string.Empty;
            lstOrderDetails.Items.Clear();
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            // Handle menu button click
        }

        private void btnCart_Click(object sender, RoutedEventArgs e)
        {
            // Handle cart button click
        }
    }
}
