﻿using System.Windows;
using Product_Management_System.Repositories.Authentication;
using QuanLiQuanCaphe.Views.ADMIN;
using QuanLiQuanCaphe.Views.Cart;

namespace Product_Management_System.Views.Admin
{
    public partial class AdminDashboardWindow : Window
    {
        private readonly UserRepository _userRepository;

        public AdminDashboardWindow()
        {
            InitializeComponent();
        }

        private void AccManage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new ActivateAccount();
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            var reportWindow = new ReportWindow();
            reportWindow.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ProductManagerWindow productManagerWindow = new ProductManagerWindow();
            productManagerWindow.Show();
        }
    }
}
