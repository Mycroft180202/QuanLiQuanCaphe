using Product_Management_System.Repositories;
using Product_Management_System.Repositories.Authentication;
using Product_Management_System.Views.Admin;
using Product_Management_System.Views.Authentication;
using QuanLiQuanCaphe.Models;
using QuanLiQuanCaphe.Repositories;
using System.Windows;
using System.Windows.Input;

namespace Product_Management_System
{
    public partial class MainWindow : Window
    {
        private CoffeeShopManagementContext dbContext;
        private User currentUser;
        private readonly IUserRepository userRepository;

        public MainWindow(User user)
        {
            InitializeComponent();
            dbContext = new CoffeeShopManagementContext();
            userRepository = new UserRepository(dbContext);
            currentUser = user;
            if (currentUser.RoleId == 1) // Admin
            {
                btnDashboard.Visibility = Visibility.Visible;
            }
            LoadUserInfo();

        }

        public MainWindow()
        {
            InitializeComponent();
            dbContext = new CoffeeShopManagementContext();
            userRepository = new UserRepository(dbContext);
            currentUser = userRepository.GetUserByUsername("default_username"); 
            LoadUserInfo();
        }

        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            AdminDashboardWindow adminDashboard = new AdminDashboardWindow();
            adminDashboard.Show();
        }

        private void LoadUserInfo()
        {
            if (currentUser != null && !string.IsNullOrEmpty(currentUser.FullName))
            {
                txtUserFullName.Text = currentUser.FullName;
            }
            else
            {
                txtUserFullName.Text = "Unknown User";
            }
        }

        private void btnProductInventory_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnPriceHistory_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void txtUserFullName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (currentUser != null)
            {
                UserInfoWindow userInfoWindow = new UserInfoWindow(currentUser, userRepository);
                userInfoWindow.Owner = this;
                bool? result = userInfoWindow.ShowDialog();

                if (result == true)
                {
                    txtUserFullName.Text = currentUser.FullName;
                }
            }
        }

        private void btnProductManage_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnCostHistory_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}