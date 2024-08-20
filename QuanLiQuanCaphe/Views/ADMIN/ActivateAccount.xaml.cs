using Product_Management_System.Repositories.Authentication;
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

namespace QuanLiQuanCaphe.Views.ADMIN
{
    /// <summary>
    /// Interaction logic for AvtivateAccount.xaml
    /// </summary>
    public partial class ActivateAccount 
    {
        private readonly UserRepository _userRepository;
        public ActivateAccount()
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

        private void Page_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {

        }
    }
}
