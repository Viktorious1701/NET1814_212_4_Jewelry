using Jewelry.Business;
using Jewelry.Data.Models;
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
                var item = await _business.GetById(int.Parse(OrderItemID.Text));
                if (item.Data == null)
                {
                    OrderItem orderItem = new OrderItem()
                    {
                        OrderItemId = Convert.ToInt32(OrderItemID.Text),
                        OrderId = Convert.ToInt32(OrderID.Text),
                        ProductId = Convert.ToInt32(ProductID.Text),
                        Quantity = Convert.ToInt32(Quantity.Text),
                        Price = Convert.ToInt32(Price.Text),
                        Subtotal = Convert.ToInt32(Quantity.Text) * Convert.ToInt32(Price.Text)
                    };
                    var existingOrderItem = await _business.GetById(orderItem.OrderItemId);

                    if (existingOrderItem.Data as OrderItem == null)
                    {
                        var result = await _business.Save(orderItem);
                        MessageBox.Show(result.Message, "Save");

                        ClearForm();
                        GetOrderItemsAsync();

                    }
                    else
                    {
                        Console.WriteLine("save CHANGES");
                        var result = await _business.Update(orderItem);
                        MessageBox.Show(result.Message, "Save");
                        return;
                    }
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
        }
        private void ClearForm()
        {
            OrderItemID.Text = string.Empty;
            OrderID.Text = string.Empty;
            ProductID.Text = string.Empty;
            Quantity.Text = string.Empty;
            Price.Text = string.Empty;
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
                            Quantity.Text = Convert.ToString(item.ProductId);
                            Price.Text = Convert.ToString(item.ProductId);
                          
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
                if (MessageBox.Show("Do you want to delete this item?", "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var result = await _business.DeleteById(int.Parse(orderItemId));
                    MessageBox.Show($"{result.Message}", "Delete");
                    this.GetOrderItemsAsync();
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

                    // Save the updated OrderItem
                    var updateResult = await _business.Update(existingOrderItem);

                    // Show success message
                    MessageBox.Show(updateResult.Message, "Update");

                    // Refresh the grid to show updated data
                    GetOrderItemsAsync();
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

    }
}