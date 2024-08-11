using Product_Management_System.Repositories;
using QuanLiQuanCaphe.Models;
using QuanLiQuanCaphe.Repositories;
using System.Windows;

namespace Product_Management_System
{
    public partial class UserInfoWindow : Window
    {
        private User currentUser;
        private readonly IUserRepository userRepository;
        private readonly CoffeeShopManagementContext dbContext;

        public UserInfoWindow(User user, IUserRepository repository)
        {
            InitializeComponent();
            currentUser = user;
            userRepository = repository;
            dbContext = new CoffeeShopManagementContext();
            LoadUserInfo();
        }

        private void LoadUserInfo()
        {
            txtUsername.Text = currentUser.Username;

            if (dbContext != null)
            {
                var roleName = (from user in dbContext.Users
                                join role in dbContext.Roles on user.RoleId equals role.RoleId
                                where user.UserId == currentUser.UserId
                                select role.RoleName).FirstOrDefault(); 

                txtRole.Text = roleName; 
            }

            txtFullName.Text = currentUser.FullName;
            txtEmail.Text = currentUser.Email;
            txtPassword.Password = new string('*', currentUser.Password.Length);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            currentUser.FullName = txtFullName.Text;
            currentUser.Email = txtEmail.Text;

            if (txtPassword.Password != new string('*', currentUser.Password.Length))
            {
                currentUser.Password = txtPassword.Password;
            }

            userRepository.UpdateUser(currentUser);
            MessageBox.Show("User information updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}