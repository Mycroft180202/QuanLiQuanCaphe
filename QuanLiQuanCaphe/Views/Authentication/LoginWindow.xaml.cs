using System.Windows;
using QuanLiQuanCaphe.Models;
using QuanLiQuanCaphe.Repositories;
using Microsoft.Extensions.DependencyInjection;
using QuanLiQuanCaphe.Services;
using Product_Management_System.Repositories.Authentication;
using QuanLiQuanCaphe;
using static Product_Management_System.App;
using QuanLiQuanCaphe.Views.Authentication;

namespace Product_Management_System.Views.Authentication
{
    public partial class LoginWindow : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly CurrentUserService _currentUserService;

        public LoginWindow(IAuthenticationService authService, CurrentUserService currentUserService)
        {
            InitializeComponent();
            var context = new CoffeeShopManagementContext(); 
            var userRepository = new UserRepository(context);
            _authenticationService = new AuthenticationService(userRepository);
            _authenticationService = authService;
            _currentUserService = currentUserService; 
        }

        public LoginWindow()
        {
            InitializeComponent();
            var context = new CoffeeShopManagementContext();
            var userRepository = new UserRepository(context);
            _authenticationService = new AuthenticationService(userRepository);
            _currentUserService = new CurrentUserService();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUser.Text;
            string password = txtPass.Password;

            var user = _authenticationService.Authenticate(username, password);

            if (user != null)
            {
                MessageBox.Show("Đăng nhập thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                MainWindow mainWindow = new MainWindow(user);
                mainWindow.Show();

                this.Close();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng hoặc tài khoản chưa được kích hoạt.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
