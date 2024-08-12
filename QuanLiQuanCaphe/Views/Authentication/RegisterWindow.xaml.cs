using Product_Management_System.Repositories.Authentication;
using Product_Management_System.Views.Authentication;
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

namespace QuanLiQuanCaphe.Views.Authentication
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private readonly UserRepository _userRepository;
        public RegisterWindow()
        {
            InitializeComponent();
            _userRepository = new UserRepository(new CoffeeShopManagementContext());
        }
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string fullName = txtFullName.Text;
            string email = txtEmail.Text;
            string username = txtUser.Text;
            string password = txtPass.Password;
            string confirmPassword = txtConfirmPass.Password;

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_userRepository.GetUserByUsername(username) != null)
            {
                MessageBox.Show("Username already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            User newUser = new User
            {
                Username = username,
                Password = password,
                FullName = fullName,
                Email = email,
                IsActive = true,
                RoleId = 2
            };

            try
            {
                _userRepository.AddUser(newUser);
                MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Registration failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
