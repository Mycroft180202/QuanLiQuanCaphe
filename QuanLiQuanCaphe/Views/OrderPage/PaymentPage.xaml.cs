using QuanLiQuanCaphe.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;    


namespace QuanLiQuanCaphe.Views.Payment
{
    public partial class PaymentPage : Page
    {
        private CoffeeShopManagementContext _context;

        public PaymentPage()
        {
            InitializeComponent();
            loadOrder();

    }
        //Load dữ liệu
        private void loadOrder()
        {
            if (_context == null)
            {
                _context = new CoffeeShopManagementContext();
            }
            lvOrder.ItemsSource = _context.Orders.ToList();

        }

        //Load các lựa chọn
        private void cbPaymentMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                ComboBoxItem selectedItem = comboBox.SelectedItem as ComboBoxItem;
                if (selectedItem != null)
                {
                    string paymentMethod = selectedItem.Content.ToString();

                    MessageBox.Show($"Hình thức thanh toán đã chọn: {paymentMethod}");

                }
            }
        }




        private void lvOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvOrder.SelectedItem is QuanLiQuanCaphe.Models.Order selected)
            {
                DateTime.TryParse(selected.OrderDate.ToString(), out DateTime date);

                txtOrderId.Text = selected.OrderId.ToString();
                txtUserId.Text = selected.UserId.ToString();
                txtDatetime.Text = date.ToString("dd/MM/yyyy HH:mm:ss"); // Hiển thị cả ngày giờ
                rbProcessing.IsChecked = selected.Status.Equals("Đang xử lý");
                rbCompleted.IsChecked = selected.Status.Equals("Hoàn thành");

                cbPaymentMethod.SelectedItem = cbPaymentMethod.Items
                    .Cast<ComboBoxItem>()
                    .FirstOrDefault(item => item.Content.ToString() == selected.PaymentMethod);
            }
        }




        private void clear()
        {
            txtOrderId.Text = "";
            txtUserId.Text = "";
            txtDatetime.Text = "";

            rbProcessing.IsChecked = false;
            rbCompleted.IsChecked = false;

            cbPaymentMethod.SelectedIndex = -1;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            clear();
        }



        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var order = new Order
                {
                    UserId = int.Parse(txtUserId.Text),
                    OrderDate = DateTime.Parse(txtDatetime.Text),
                    Status = rbProcessing.IsChecked == true ? "Processing" : rbCompleted.IsChecked == true ? "Completed" : null,
                    PaymentMethod = cbPaymentMethod.Text
                };

                _context.Orders.Add(order);
                _context.SaveChanges();

                CalculateTotalAmount(order.OrderId);

                clear();
                loadOrder();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding order: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void CalculateTotalAmount(int orderId)
        {
            try
            {
                var orderDetails = _context.OrderDetails.Where(od => od.OrderId == orderId).ToList();

                var totalAmount = orderDetails.Sum(od => od.Quantity * od.Price);

                var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
                if (order != null)
                {
                    order.TotalAmount = totalAmount;
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error calculating total amount: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(txtOrderId.Text, out int id))
                {
                    var order = _context.Orders.FirstOrDefault(x => x.OrderId == id);
                    if (order != null)
                    {
                        order.UserId = int.Parse(txtUserId.Text);
                        order.OrderDate = DateTime.Parse(txtDatetime.Text);
                        order.Status = rbProcessing.IsChecked == true ? "Processing" : rbCompleted.IsChecked == true ? "Completed" : order.Status;
                        order.PaymentMethod = cbPaymentMethod.Text;

                        _context.Orders.Update(order);
                        _context.SaveChanges();

                        MessageBox.Show("Update thành công", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);

                        clear();
                        loadOrder();
                    }
                    else
                    {
                        MessageBox.Show("Order không tìm thấy", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("ID không hợp lệ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating order: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(txtOrderId.Text, out int id))
                {
                    var order = _context.Orders.FirstOrDefault(p => p.OrderId == id);

                    if (order != null)
                    {
                        _context.Orders.Remove(order);
                        _context.SaveChanges();

                        MessageBox.Show("Delete thành công", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);

                        clear();

                        loadOrder();
                    }
                    else
                    {
                        MessageBox.Show("Order không tìm thấy", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("ID không hợp lệ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting order: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
