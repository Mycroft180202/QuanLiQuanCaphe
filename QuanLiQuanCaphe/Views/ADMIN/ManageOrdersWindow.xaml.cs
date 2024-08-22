using System.Linq;
using System.Windows;
using QuanLiQuanCaphe.Models;

namespace Product_Management_System.Views
{
    public partial class ManageOrdersWindow : Window
    {
        private readonly CoffeeShopManagementContext dbContext;

        public Action RefreshTables;

        public ManageOrdersWindow()
        {
            InitializeComponent();
            dbContext = new CoffeeShopManagementContext();
            LoadPendingOrders();
        }

        private void LoadPendingOrders()
        {
            var pendingOrders = dbContext.Orders
                                         .Where(o => o.Status == "Pending")
                                         .Select(o => new
                                         {
                                             o.OrderId,
                                             Table = o.Table,
                                             User = o.User,
                                             o.OrderDate,
                                             o.TotalAmount,
                                             o.Status
                                         })
                                         .ToList();

            dgOrders.ItemsSource = pendingOrders;
        }

        private void btnComplete_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = dgOrders.SelectedItem;
            if (selectedOrder == null)
            {
                MessageBox.Show("Vui lòng chọn đơn hàng!");
                return;
            }

            var order = dbContext.Orders.Find(((dynamic)selectedOrder).OrderId);
            if (order != null)
            {
                order.Status = "Completed";
                dbContext.Orders.Update(order);

                var table = dbContext.Tables.Find(order.TableId);
                if (table != null)
                {
                    table.IsOccupied = false;
                    dbContext.Tables.Update(table);
                }

                dbContext.SaveChanges();
                MessageBox.Show("Đơn hàng đã hoàn tất!");

                RefreshTables?.Invoke(); // Notify MainWindow to refresh tables
                LoadPendingOrders();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = dgOrders.SelectedItem;
            if (selectedOrder == null)
            {
                MessageBox.Show("Vui lòng chọn đơn hàng!");
                return;
            }

            var order = dbContext.Orders.Find(((dynamic)selectedOrder).OrderId);
            if (order != null)
            {
                order.Status = "Cancelled";
                dbContext.Orders.Update(order);

                var table = dbContext.Tables.Find(order.TableId);
                if (table != null)
                {
                    table.IsOccupied = false;
                    dbContext.Tables.Update(table);
                }

                dbContext.SaveChanges();
                MessageBox.Show("Đơn hàng đã bị hủy!");

                RefreshTables?.Invoke(); // Notify MainWindow to refresh tables
                LoadPendingOrders();
            }
        }
    }
}
