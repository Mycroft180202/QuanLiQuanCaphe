using QuanLiQuanCaphe.Models;
using QuanLiQuanCaphe.Repositories.Authentication;
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

namespace QuanLiQuanCaphe.Views.Authentication.Login
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            var context = new CoffeeShopManagementContext();
            var userRepository = new UserRepository(context);

        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
