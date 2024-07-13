using Jewelry.Business;
using Jewelry.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace Jewelry.WpfApp.UI
{
    /// <summary>
    /// Interaction logic for wOrderItem.xaml
    /// </summary>
    public partial class wOrderItem : Window
    {
        private readonly OrderItemBusiness _business;
        public wOrderItem()
        {
            InitializeComponent();
            _business = new OrderItemBusiness();
            Loaded += OnWindowLoaded;
        }

        private async void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            await GetOrderItemsAsync();
        }

        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OrderItem orderItem = new OrderItem()
                {
                    OrderItemId = Convert.ToInt32(OrderItemID.Text),
                    OrderId = Convert.ToInt32(OrderID.Text),
                    ProductId = Convert.ToInt32(ProductID.Text),
                    Quantity = Convert.ToInt32(Quantity.Text),
                    Price = Convert.ToInt32(Price.Text),
                    Subtotal = Convert.ToInt32(Quantity.Text) * Convert.ToInt32(Price.Text),
                    Total = Convert.ToDouble(Total.Text),
                    Discount = Convert.ToDouble(SubTotal.Text) * Convert.ToDouble(Discount.Text),
                    Status = Status.Text,
                    CustomerId = Convert.ToInt32(CustomerID.Text)

                };
                var item = await _business.GetById(int.Parse(OrderItemID.Text));
                if (item.Data == null)
                {

                    var result = await _business.Save(orderItem);
                    MessageBox.Show(result.Message, "Save");
                    ClearForm();
                    await GetOrderItemsAsync();
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer("D:\\Semester5\\PRN212\\NET1814_212_4_Jewelry\\Jewelry.WpfApp\\UI\\Sounds\\tactical-nuke.wav");
                    if (player != null)
                    {
                        player.Play();
                    }
                }
                else
                {
                    ButtonUpdate_Click(sender, e);
                    return;
                
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }

        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
            System.Media.SoundPlayer player = new System.Media.SoundPlayer("D:\\Semester5\\PRN212\\NET1814_212_4_Jewelry\\Jewelry.WpfApp\\UI\\Sounds\\finger-snap.wav");
            player.Play();
        }
        private void ClearForm()
        {
            OrderItemID.Text = string.Empty;
            OrderID.Text = string.Empty;
            ProductID.Text = string.Empty;
            Quantity.Text = string.Empty;
            Price.Text = string.Empty;
            SubTotal.Text = string.Empty;
            Total.Text = string.Empty;
            Discount.Text = string.Empty;
            Status.Text = string.Empty;
            CustomerID.Text = string.Empty;
        }
        private async Task GetOrderItemsAsync()
        {
            var result = await _business.GetAll();


            if (result.Status > 0 && result.Data != null)
            {
                grdOrderItem.ItemsSource = result.Data as List<OrderItem>;
            }
            else
            {
                grdOrderItem.ItemsSource = new List<OrderItem>();
            }
        }
        private async void ButtonSelect_Click(object sender, RoutedEventArgs e)
        {
            OrderItem orderItem = grdOrderItem.SelectedItem as OrderItem;
            if (orderItem != null)
            {
                OrderItemID.Text = orderItem.OrderItemId.ToString();
                OrderID.Text = orderItem.OrderId.ToString();
                ProductID.Text = orderItem.ProductId.ToString();
                Quantity.Text = orderItem.Quantity.ToString();
                Price.Text = orderItem.Price.ToString();
                SubTotal.Text = orderItem.Subtotal.ToString();
                CustomerID.Text = orderItem.CustomerId.ToString();
                Status.Text = orderItem.Status;
                Total.Text = orderItem.Total.ToString();
                Discount.Text = orderItem.Discount.ToString();
            }
            System.Media.SoundPlayer player = new System.Media.SoundPlayer("D:\\Semester5\\PRN212\\NET1814_212_4_Jewelry\\Jewelry.WpfApp\\UI\\Sounds\\i-am-the-chosen-one.wav");
            if(player!=null)
            {
                player.Play();
            }
            



        }
        private async void grdProduct_MouseDouble_Click(object sender, RoutedEventArgs e)
        {
            DataGrid grd = sender as DataGrid;
            if (grd != null && grd.SelectedItem != null && grd.SelectedItems.Count == 1)
            {
                var row = grd.ItemContainerGenerator.ContainerFromItem(grd.SelectedItem) as DataGridRow;
                if (row != null)
                {
                    var item = row.Item as OrderItem;
                    if (item != null)
                    {
                        var productResult = await _business.GetById(item.OrderItemId);

                        if (productResult.Status > 0 && productResult.Data != null)
                        {
                            item = productResult.Data as OrderItem;
                            OrderItemID.Text = Convert.ToString(item.OrderItemId);
                            OrderID.Text = Convert.ToString(item.OrderId);
                            ProductID.Text = Convert.ToString(item.ProductId);
                            Quantity.Text = Convert.ToString(item.Quantity);
                            Price.Text = Convert.ToString(item.ProductId);
                            SubTotal.Text = Convert.ToString(item.Subtotal);
                            Discount.Text = Convert.ToString(item.Discount);
                            Total.Text = Convert.ToString(item.Total);
                            Status.Text = item.Status;
                            CustomerID.Text = Convert.ToString(item.CustomerId);

                        }
                    }
                }
            }
        }

        private async void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            string orderItemId = btn.CommandParameter.ToString();

            //MessageBox.Show(productId);

            if (!string.IsNullOrEmpty(orderItemId))
            {
                System.Media.SoundPlayer player1 = new System.Media.SoundPlayer("D:\\Semester5\\PRN212\\NET1814_212_4_Jewelry\\Jewelry.WpfApp\\UI\\Sounds\\riel.wav");
                if (player1 != null)
                {
                    player1.Play();
                }
                if (MessageBox.Show("Do you want to delete this item?", "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer("D:\\Semester5\\PRN212\\NET1814_212_4_Jewelry\\Jewelry.WpfApp\\UI\\Sounds\\nuke-bomb.wav");
                    if (player != null)
                    {
                        player.Play();
                    }
                    var result = await _business.DeleteById(int.Parse(orderItemId));
                    MessageBox.Show($"{result.Message}", "Delete");
                    await this.GetOrderItemsAsync();

                }
            }
        }

        private async void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Fetch the existing OrderItem asynchronously
                var result = await _business.GetById(Convert.ToInt32(OrderItemID.Text));

                // Check if the OrderItem exists
                if (result.Data is OrderItem existingOrderItem)
                {
                    // Update the properties
                    existingOrderItem.OrderId = Convert.ToInt32(OrderID.Text);
                    existingOrderItem.ProductId = Convert.ToInt32(ProductID.Text);
                    existingOrderItem.Quantity = Convert.ToInt32(Quantity.Text);
                    existingOrderItem.Price = Convert.ToInt32(Price.Text);
                    existingOrderItem.Subtotal = Convert.ToInt32(Quantity.Text) * Convert.ToInt32(Price.Text);
                    existingOrderItem.Discount = Convert.ToDouble(Discount.Text);
                    existingOrderItem.Total = Convert.ToDouble(SubTotal.Text) * (1-Convert.ToDouble(Discount.Text));
                    existingOrderItem.Status = Status.Text;
                    existingOrderItem.CustomerId = Convert.ToInt32(CustomerID.Text);

                    // Save the updated OrderItem
                    var updateResult = await _business.Update(existingOrderItem);

                    // Show success message
                    MessageBox.Show(updateResult.Message, "Update");

                    // Refresh the grid to show updated data
                    await GetOrderItemsAsync();
                }
                else
                {
                    // Show a warning if the OrderItem is not found
                    MessageBox.Show("OrderItemID not found", "Warning");
                }
            }
            catch (Exception ex)
            {
                // Handle and show any errors that occurred during the update process
                MessageBox.Show(ex.ToString(), "Error");
            }
        }
        private async void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create a new OrderItem object to hold the search criteria
                OrderItem searchCriteria = new OrderItem
                {
                    OrderItemId = !string.IsNullOrEmpty(OrderItemID.Text) ? Convert.ToInt32(OrderItemID.Text) : 0,
                    OrderId = !string.IsNullOrEmpty(OrderID.Text) ? Convert.ToInt32(OrderID.Text) : 0,
                    ProductId = !string.IsNullOrEmpty(ProductID.Text) ? Convert.ToInt32(ProductID.Text) : 0,
                    Quantity = !string.IsNullOrEmpty(Quantity.Text) ? Convert.ToInt32(Quantity.Text) : 0,
                    Price = !string.IsNullOrEmpty(Price.Text) ? Convert.ToInt32(Price.Text) : 0,
                    Status = Status.Text,
                    CustomerId = !string.IsNullOrEmpty(CustomerID.Text) ? Convert.ToInt32(CustomerID.Text) : 0
                };

                // Assuming you have a method in your business layer to search for OrderItems
                var result = await _business.SearchOrderItems(searchCriteria);

                if (result.Status > 0 && result.Data != null)
                {
                    grdOrderItem.ItemsSource = result.Data as List<OrderItem>;
                    //MessageBox.Show($"Found {(result.Data as List<OrderItem>).Count} matching order item(s).", "Search Results", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    grdOrderItem.ItemsSource = new List<OrderItem>();
                    MessageBox.Show("No matching order items found.", "Search Results", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                // Play a sound effect for search
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }
    }
}