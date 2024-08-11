using System.Collections.Generic;
using System.Windows;
using Product_Management_System.Repositories;
using Product_Management_System.Repositories.Authentication;
using QuanLiQuanCaphe.Models;

namespace Product_Management_System.Views.Admin
{
    public partial class AdminDashboardWindow : Window
    {
        private readonly UserRepository _userRepository;

        public AdminDashboardWindow()
        {
            InitializeComponent();
            _userRepository = new UserRepository(new CoffeeShopManagementContext());
            LoadUsers();
        }

        private void LoadUsers()
        {
            List<User> users = _userRepository.GetAllUsers();
            lvUsers.ItemsSource = users;
        }

        private void btnActivate_Click(object sender, RoutedEventArgs e)
        {
            User selectedUser = (User)lvUsers.SelectedItem;
            if (selectedUser != null)
            {
                selectedUser.IsActive = true;
                _userRepository.UpdateUser(selectedUser);
                LoadUsers();
            }
        }

        private void btnDeactivate_Click(object sender, RoutedEventArgs e)
        {
            User selectedUser = (User)lvUsers.SelectedItem;
            if (selectedUser != null)
            {
                selectedUser.IsActive = false;
                _userRepository.UpdateUser(selectedUser);
                LoadUsers();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            User selectedUser = (User)lvUsers.SelectedItem;
            if (selectedUser != null)
            {
                _userRepository.DeleteUser(selectedUser.UserId);
                LoadUsers();
            }
        }
    }
}
