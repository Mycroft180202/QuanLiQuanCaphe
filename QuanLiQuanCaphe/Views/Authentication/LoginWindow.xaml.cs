using Product_Management_System;
using Product_Management_System.Repositories.Authentication;
using QuanLiQuanCaphe.Models;
using QuanLiQuanCaphe.Services;
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
using static Product_Management_System.App;

namespace QuanLiQuanCaphe.Views.Authentication
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly CurrentUserService _currentUserService;
        public LoginWindow()
        {
            InitializeComponent();
            var context = new CoffeeShopManagementContext();
            var userRepository = new UserRepository(context);
            _authenticationService = new AuthenticationService(userRepository);
            _currentUserService = new CurrentUserService();
        }
        public LoginWindow(IAuthenticationService authService, CurrentUserService currentUserService)
        {
            InitializeComponent();
            var context = new CoffeeShopManagementContext();
            var userRepository = new UserRepository(context);
            _authenticationService = new AuthenticationService(userRepository);
            _authenticationService = authService;
            _currentUserService = currentUserService;
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
