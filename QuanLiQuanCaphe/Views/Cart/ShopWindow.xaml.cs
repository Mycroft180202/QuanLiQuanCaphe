using Microsoft.EntityFrameworkCore;
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

namespace QuanLiQuanCaphe.Views.Cart
{
    /// <summary>
    /// Interaction logic for ShopWindow.xaml
    /// </summary>
    public partial class ShopWindow : Window
    {

        CoffeeShopManagementContext dbContext = new CoffeeShopManagementContext();

        public ShopWindow()
        {
            InitializeComponent();
            LoadCategories();
        }

        private void LoadCategories()
        {
            var categoryNames = dbContext.Categories.Select(c => new { c.CategoryId, c.CategoryName }).ToList();
            foreach (var category in categoryNames)
            {
                ListViewItem item = new ListViewItem { Height = 60, Tag = category.CategoryId };
                StackPanel stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
                TextBlock textBlock = new TextBlock
                {
                    Text = category.CategoryName,
                    FontSize = 17,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(20, 0, 0, 0)
                };

                stackPanel.Children.Add(textBlock);
                item.Content = stackPanel;
                CategoryListView.Items.Add(item);
            }
        }

        private void CategoryListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryListView.SelectedItem is ListViewItem selectedItem)
            {
                var selectedCategoryId = (int)selectedItem.Tag;
                LoadProducts(selectedCategoryId);
            }
        }

        private void LoadProducts(int categoryId)
        {
            ProductPanel.Children.Clear();
            var products = dbContext.Products.Where(p => p.CategoryId == categoryId).ToList();

            foreach (var product in products)
            {
                var productBorder = new Border
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1),
                    Margin = new Thickness(5),
                    Width = 270,
                    Height = 280
                };

                var stackPanel = new StackPanel { Width = 270, HorizontalAlignment = HorizontalAlignment.Left };

                var imagePath = $"pack://application:,,,/{product.Image}";
                var productImage = new Image
                {
                    Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute)), 
                    Width = 100,
                    Height = 100,
                    Margin = new Thickness(10),
                    HorizontalAlignment = HorizontalAlignment.Center
                };



                var nameTextBlock = new TextBlock
                {
                    Text = product.Name,
                    Margin = new Thickness(10),
                    FontFamily = new FontFamily("Showcard Gothic"),
                    FontSize = 12,
                    Foreground = Brushes.DarkRed
                };

                var descriptionTextBlock = new TextBlock
                {
                    Text = product.Description,
                    FontSize = 10,
                    Margin = new Thickness(5),
                    TextWrapping = TextWrapping.Wrap,
                    FontFamily = new FontFamily("Champagne & Limousines")
                };

                var priceTextBlock = new TextBlock
                {
                    Text = $"VND {product.Price}",
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(10),
                    FontSize = 20,
                    FontFamily = new FontFamily("Champagne & Limousines")
                };

                var buttonPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(10)
                };

                var addToCartButton = new Button
                {
                    Width = 100,
                    Content = "Add To Cart",
                    FontFamily = new FontFamily("Champagne & Limousines"),
                    Margin = new Thickness(5),
                    Tag = product.ProductId
                };
                addToCartButton.Click += AddToCartButton_Click;

                var buyNowButton = new Button
                {
                    Width = 100,
                    Content = "Buy Now",
                    FontFamily = new FontFamily("Champagne & Limousines"),
                    Margin = new Thickness(5),
                    Tag = product.ProductId
                };
                buyNowButton.Click += BuyNowButton_Click;

                buttonPanel.Children.Add(addToCartButton);
                buttonPanel.Children.Add(buyNowButton);

                stackPanel.Children.Add(productImage);
                stackPanel.Children.Add(nameTextBlock);
                stackPanel.Children.Add(descriptionTextBlock);
                stackPanel.Children.Add(priceTextBlock);
                stackPanel.Children.Add(buttonPanel);

                productBorder.Child = stackPanel;
                ProductPanel.Children.Add(productBorder);
            }
        }

        private void AddToCartButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var productId = button.Tag as int?;
                if (productId.HasValue)
                {
                    int currentUserId = GetCurrentUserId();

                    using (var context = new CoffeeShopManagementContext())
                    {
                        var existingCartItem = context.Carts
                            .FirstOrDefault(c => c.UserId == currentUserId && c.ProductId == productId.Value);

                        if (existingCartItem != null)
                        {
                            existingCartItem.Quantity++;
                        }
                        else
                        {
                            var cart = new QuanLiQuanCaphe.Models.Cart
                            {
                                UserId = currentUserId,
                                ProductId = productId.Value,
                                Quantity = 1
                            };

                            context.Carts.Add(cart);
                        }

                        try
                        {
                            context.SaveChanges();
                            MessageBox.Show("Sản phẩm đã được thêm vào giỏ hàng.");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error: {ex.Message}");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Product ID is not set.");
                }
            }
        }

        private int GetCurrentUserId()
        {
            return 3; 
        }

        private void BuyNowButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var productId = button.Tag as int?;
                if (productId.HasValue)
                {
                    var checkoutWindow = new DetailWindow(productId.Value);
                    checkoutWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Product ID is not set.");
                }
            }
        }

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            var cartWindow = new CartWindow(GetCurrentUserId());
            cartWindow.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSeachMain.Text.ToLower().Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Please enter a search term.", "Search", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                var filteredItems = dbContext.Carts
                    .Where(c => c.Product.Name.ToLower().Contains(keyword))
                    .Select(c => c.Product) 
                    .Distinct() 
                    .ToList();

                ProductPanel.Children.Clear();

                if (filteredItems.Any())
                {
                    foreach (var product in filteredItems)
                    {
                        var productBorder = new Border
                        {
                            BorderBrush = Brushes.Black,
                            BorderThickness = new Thickness(1),
                            Margin = new Thickness(5),
                            Width = 270,
                            Height = 280
                        };

                        var stackPanel = new StackPanel { Width = 270, HorizontalAlignment = HorizontalAlignment.Left };

                        var imagePath = $"pack://application:,,,/{product.Image}";
                        var productImage = new Image
                        {
                            Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute)), 
                            Width = 100,
                            Height = 100,
                            Margin = new Thickness(10),
                            HorizontalAlignment = HorizontalAlignment.Center
                        };

                        var nameTextBlock = new TextBlock
                        {
                            Text = product.Name,
                            Margin = new Thickness(10),
                            FontFamily = new FontFamily("Showcard Gothic"),
                            FontSize = 12,
                            Foreground = Brushes.DarkRed
                        };

                        var descriptionTextBlock = new TextBlock
                        {
                            Text = product.Description,
                            FontSize = 10,
                            Margin = new Thickness(5),
                            TextWrapping = TextWrapping.Wrap,
                            FontFamily = new FontFamily("Champagne & Limousines")
                        };

                        var priceTextBlock = new TextBlock
                        {
                            Text = $"VND {product.Price}",
                            VerticalAlignment = VerticalAlignment.Center,
                            Margin = new Thickness(10),
                            FontSize = 20,
                            FontFamily = new FontFamily("Champagne & Limousines")
                        };

                        var buttonPanel = new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Margin = new Thickness(10)
                        };

                        var addToCartButton = new Button
                        {
                            Width = 100,
                            Content = "Add To Cart",
                            FontFamily = new FontFamily("Champagne & Limousines"),
                            Margin = new Thickness(5),
                            Tag = product.ProductId
                        };
                        addToCartButton.Click += AddToCartButton_Click;

                        var buyNowButton = new Button
                        {
                            Width = 100,
                            Content = "Buy Now",
                            FontFamily = new FontFamily("Champagne & Limousines"),
                            Margin = new Thickness(5),
                            Tag = product.ProductId
                        };
                        buyNowButton.Click += BuyNowButton_Click;

                        buttonPanel.Children.Add(addToCartButton);
                        buttonPanel.Children.Add(buyNowButton);

                        stackPanel.Children.Add(productImage); 
                        stackPanel.Children.Add(nameTextBlock);
                        stackPanel.Children.Add(descriptionTextBlock);
                        stackPanel.Children.Add(priceTextBlock);
                        stackPanel.Children.Add(buttonPanel);

                        productBorder.Child = stackPanel;
                        ProductPanel.Children.Add(productBorder);
                    }
                }
                else
                {
                    MessageBox.Show("No items found matching your search.", "Search Results", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }



    }
}
