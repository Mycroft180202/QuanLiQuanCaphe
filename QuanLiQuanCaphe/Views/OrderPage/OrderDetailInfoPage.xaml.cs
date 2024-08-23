using Microsoft.EntityFrameworkCore;
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

namespace QuanLiQuanCaphe.Views.OrderPage
{
    /// <summary>
    /// Interaction logic for OrderDetailInfoPage.xaml
    /// </summary>
    public partial class OrderDetailInfoPage : Page
    {
        private CoffeeShopManagementContext _context;

        public OrderDetailInfoPage()
        {
            InitializeComponent();
            loadOrderDetail();
        }

        //Load dữ liệu
        private void loadOrderDetail()
        {
            if (_context == null)
            {
                _context = new CoffeeShopManagementContext();
            }

            var orderDetails = _context.OrderDetails.ToList();

            lvOrderDetails.ItemsSource = orderDetails;
            lvOrderDetails.Items.Refresh(); // Làm mới dữ liệu ListView
        }

        private void lvOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvOrderDetails.SelectedItem is QuanLiQuanCaphe.Models.OrderDetail selected)
            {

                txtOrderDetailId.Text = selected.OrderDetailId.ToString();
                txtOrderId.Text = selected.OrderId.ToString();
                txtProductId.Text = selected.ProductId.ToString();
                txtQuantity.Text = selected.Quantity.ToString();
                txtPrice.Text = selected.Price.ToString();

            }
        }
        private void clear()
        {
            txtOrderDetailId.Text = "";
            txtOrderId.Text = "";
            txtProductId.Text = "";
            txtQuantity.Text = "";
            txtPrice.Text = "";

        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            clear();
        }



        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Tạo một đối tượng OrderDetail mới và gán các giá trị từ các TextBox
                var orderDetail = new OrderDetail
                {
                    // OrderDetailId sẽ được lấy từ nguồn dữ liệu đã thiết lập
                    // OrderId, ProductId, Quantity, và Price là các trường được người dùng nhập vào
                    OrderId = int.Parse(txtOrderId.Text),
                    ProductId = int.Parse(txtProductId.Text),
                    Quantity = int.Parse(txtQuantity.Text),
                    Price = decimal.Parse(txtPrice.Text)
                };

                // Thêm OrderDetail vào cơ sở dữ liệu và lưu thay đổi
                _context.OrderDetails.Add(orderDetail);
                _context.SaveChanges();

                // Hiển thị thông báo thành công
                MessageBox.Show("Add thành công", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);

                // Làm sạch các trường nhập liệu và refresh lại ListView
                clear();
                loadOrderDetail();
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi nếu có
                MessageBox.Show($"Error adding order detail: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Xác thực và lấy OrderDetailId từ TextBox
                if (int.TryParse(txtOrderDetailId.Text, out int id))
                {
                    var orderDetail = _context.OrderDetails.FirstOrDefault(x => x.OrderDetailId == id);
                    if (orderDetail != null)
                    {
                        // Cập nhật các trường OrderDetail từ TextBox
                        orderDetail.OrderId = int.Parse(txtOrderId.Text);
                        orderDetail.ProductId = int.Parse(txtProductId.Text);
                        orderDetail.Quantity = int.Parse(txtQuantity.Text);
                        orderDetail.Price = decimal.Parse(txtPrice.Text);

                        // Cập nhật OrderDetail trong cơ sở dữ liệu
                        _context.OrderDetails.Update(orderDetail);
                        _context.SaveChanges();

                        // Hiển thị thông báo thành công
                        MessageBox.Show("Update thành công", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Làm sạch các trường nhập liệu và refresh lại ListView
                        clear();
                        loadOrderDetail();
                    }
                    else
                    {
                        // Thông báo nếu không tìm thấy OrderDetail
                        MessageBox.Show("OrderDetail không tìm thấy", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    // Thông báo nếu ID không hợp lệ
                    MessageBox.Show("ID không hợp lệ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi nếu có
                MessageBox.Show($"Error updating order detail: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(txtOrderDetailId.Text, out int id))
                {
                    var orderDetail = _context.OrderDetails
                        .FirstOrDefault(p => p.OrderDetailId == id);

                    if (orderDetail != null)
                    {
                        _context.OrderDetails.Remove(orderDetail);
                        _context.SaveChanges();

                        MessageBox.Show("Delete thành công", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);

                        clear();

                        loadOrderDetail();
                    }
                    else
                    {
                        // Thông báo nếu không tìm thấy OrderDetail
                        MessageBox.Show("OrderDetail không tìm thấy", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    // Thông báo nếu ID không hợp lệ
                    MessageBox.Show("ID không hợp lệ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi nếu có
                MessageBox.Show($"Error deleting order detail: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
