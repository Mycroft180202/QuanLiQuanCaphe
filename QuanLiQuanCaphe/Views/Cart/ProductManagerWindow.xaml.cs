using QuanLiQuanCaphe.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace QuanLiQuanCaphe.Views.Cart
{
    /// <summary>
    /// Interaction logic for ProductManagerWindow.xaml
    /// </summary>
    public partial class ProductManagerWindow : Window
    {

        CoffeeShopManagementContext context =new CoffeeShopManagementContext();
        public ProductManagerWindow()
        {
            InitializeComponent();
            LoadData();


        }
        public void LoadData()
        {
            dgProduct.ItemsSource = context.Products.Select(p => new
            {
                ProductID = p.ProductId,
                ProductName = p.Name,
                ProductDescription = p.Description,
                ProductCategory = p.Category.CategoryName,
                ProductPrice = p.Price,
                ProductStock = p.Stock,
            }).ToList();

            var lsC = context.Categories.ToList();
            cbCate.ItemsSource = lsC;
            cbCate.DisplayMemberPath = "CategoryName"; 
            cbCate.SelectedValuePath = "CategoryId";  
        }



        private void dgProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = dgProduct.SelectedItem;
            if (selected != null)
            {
                var productCategory = selected.GetType().GetProperty("ProductCategory").GetValue(selected, null).ToString();
                txtId.Text = selected.GetType().GetProperty("ProductID").GetValue(selected, null).ToString();
                txtName.Text = selected.GetType().GetProperty("ProductName").GetValue(selected, null).ToString();
                txtDes.Text = selected.GetType().GetProperty("ProductDescription").GetValue(selected, null).ToString();
                txtPrice.Text = selected.GetType().GetProperty("ProductPrice").GetValue(selected, null).ToString();
                txtStock.Text = selected.GetType().GetProperty("ProductStock").GetValue(selected, null).ToString();

                var selectedCategory = context.Categories.FirstOrDefault(c => c.CategoryName == productCategory);
                if (selectedCategory != null)
                {
                    cbCate.SelectedItem = selectedCategory;
                }
            }
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(txtPrice.Text, out decimal price) && int.TryParse(txtStock.Text, out int stock))
            {
                if (cbCate.SelectedItem is Category selectedCategory)
                {
                    Product ProAdd = new Product
                    {
                        Name = txtName.Text,
                        Description = txtDes.Text,
                        Price = price,
                        Stock = stock, 
                        CategoryId = selectedCategory.CategoryId 
                    };

                    context.Products.Add(ProAdd);
                    if (context.SaveChanges() > 0)
                    {
                        MessageBox.Show("Add success", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData();
                    }
                }
                else
                {
                    MessageBox.Show("Please select a category.");
                }
            }
            else
            {
                MessageBox.Show("Please enter valid numbers for Price and Stock.");
            }
        }

        private void TxtPrice_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !decimal.TryParse((sender as TextBox).Text + e.Text, out _);
        }
        private void TxtStock_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

      



        private void Button_Click_Edit(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtId.Text, out int productId))
            {
                var productToEdit = context.Products.FirstOrDefault(p => p.ProductId == productId);

                if (productToEdit != null)
                {
                    if (decimal.TryParse(txtPrice.Text, out decimal price) && int.TryParse(txtStock.Text, out int stock))
                    {
                        if (cbCate.SelectedItem is Category selectedCategory)
                        {
                            productToEdit.Name = txtName.Text;
                            productToEdit.Description = txtDes.Text;
                            productToEdit.Price = price;
                            productToEdit.Stock = stock;
                            productToEdit.CategoryId = selectedCategory.CategoryId;

                            if (context.SaveChanges() > 0)
                            {
                                MessageBox.Show("Edit success", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                LoadData(); 
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please select a category.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter valid numbers for Price and Stock.");
                    }
                }
                else
                {
                    MessageBox.Show("Product not found.");
                }
            }
            else
            {
                MessageBox.Show("Invalid Product ID.");
            }
        }


        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtId.Text, out int productId))
            {
                var productToDelete = context.Products.FirstOrDefault(p => p.ProductId == productId);

                if (productToDelete != null)
                {
                    context.Products.Remove(productToDelete);

                    if (context.SaveChanges() > 0)
                    {
                        MessageBox.Show("Delete success", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData(); 
                    }
                }
                else
                {
                    MessageBox.Show("Product not found.");
                }
            }
            else
            {
                MessageBox.Show("Invalid Product ID.");
            }
        }

    }
}
