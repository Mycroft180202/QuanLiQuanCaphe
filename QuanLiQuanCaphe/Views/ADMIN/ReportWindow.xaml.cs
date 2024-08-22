using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using QuanLiQuanCaphe.Models;

namespace Product_Management_System.Views.Admin
{
    public partial class ReportWindow : Window
    {
        private readonly CoffeeShopManagementContext dbContext;

        public ReportWindow()
        {
            InitializeComponent();
            dbContext = new CoffeeShopManagementContext();
            LoadSalesReport();
        }

        private void LoadSalesReport()
        {
            // Query to aggregate sales data
            var salesReport = dbContext.OrderDetails
                                       .GroupBy(od => od.Product.Name)
                                       .Select(group => new
                                       {
                                           ProductName = group.Key,
                                           QuantitySold = group.Sum(od => od.Quantity),
                                           TotalSales = group.Sum(od => od.Price * od.Quantity)
                                       })
                                       .ToList();

            dgReport.ItemsSource = salesReport;
        }

        private void ExportToCSV_Click(object sender, RoutedEventArgs e)
        {
            // Open a SaveFileDialog to let the user choose a file path
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV file (*.csv)|*.csv",
                FileName = "SalesReport.csv"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                // Export the report data to a CSV file
                using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                {
                    writer.WriteLine("Product Name,Quantity Sold,Total Sales");

                    foreach (var item in dgReport.Items)
                    {
                        dynamic reportItem = item;
                        writer.WriteLine($"{reportItem.ProductName},{reportItem.QuantitySold},{reportItem.TotalSales}");
                    }
                }

                MessageBox.Show("Report exported successfully!");
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
