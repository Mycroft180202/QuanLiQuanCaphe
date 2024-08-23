using QuanLiQuanCaphe.Models;
using QuanLiQuanCaphe.Views.Payment;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLiQuanCaphe.Views.OrderPage
{
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        public OrderWindow()
        {
            InitializeComponent();
        }
       
        private void btnOrderDetails_Click(object sender, RoutedEventArgs e)
        {
            var orderDetailWindow = new Window
            {
                Content = new OrderDetailInfoPage(),
                Title = "Order Details",
                Width = 800,
                Height = 600
            };
            orderDetailWindow.Show();
        }


        private void btnPaymentDetails_Click(object sender, RoutedEventArgs e)
        {
            var paymentWindow = new Window
            {
                Content = new PaymentPage(),
                Title = "Payment Details",
                Width = 800,
                Height = 600
            };
            paymentWindow.Show();
        }
    }
}
