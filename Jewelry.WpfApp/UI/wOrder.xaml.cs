using Jewelry.Business;
using Jewelry.Data;
using Jewelry.Data.Models;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Xml.Linq;

namespace Jewelry.WpfApp.UI
{
    /// <summary>
    /// Interaction logic for wOrder.xaml
    /// </summary>
    public partial class wOrder : Window
    {
        private readonly OrderBusiness _order;

        public wOrder()
        {
            InitializeComponent();
            _order = new OrderBusiness();
            Loaded += OnWindowLoaded;
        }

        private async void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            await LoadOrders();
        }

        private async Task RefreshOrderData()
        {
            await LoadOrders();
        }


        private async Task LoadOrders(string searchInput = null)
        {
            var result = await _order.GetAll(); // This should fetch fresh data from the database
            if (result.Status > 0 && result.Data != null)
            {
                grdCurrency.ItemsSource = result.Data as List<SiOrder>; // Changed SiProduct to SiOrder
            }
            else
            {
                grdCurrency.ItemsSource = new List<SiOrder>(); // Changed SiProduct to SiOrder
            }
        }

        private async void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            var result = await _order.GetAll();
            if (result.Status > 0 && result.Data != null)
            {
                var filteredData = result.Data as List<SiOrder>;

                filteredData = filteredData.Where(order =>
                    (string.IsNullOrEmpty(OrderIdSearch.Text) || order.OrderId.ToString() == OrderIdSearch.Text) &&
                    (string.IsNullOrEmpty(CustomerIdSearch.Text) || order.CustomerId.ToString() == CustomerIdSearch.Text) &&
                    (string.IsNullOrEmpty(OrderDateSearch.Text) || order.OrderDate.ToString().Contains(OrderDateSearch.Text)) &&
                    (string.IsNullOrEmpty(TotalAmountSearch.Text) || order.TotalAmount.ToString() == TotalAmountSearch.Text) &&
                    (string.IsNullOrEmpty(PaymentMethodSearch.Text) || string.Equals(order.PaymentMethod, PaymentMethodSearch.Text, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrEmpty(PaymentStatusSearch.Text) || string.Equals(order.PaymentStatus, PaymentStatusSearch.Text, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrEmpty(ShipmentStatusSearch.Text) || string.Equals(order.ShipmentStatus, ShipmentStatusSearch.Text, StringComparison.OrdinalIgnoreCase))
                ).ToList();

                grdCurrency.ItemsSource = filteredData;
            }
            else
            {
                grdCurrency.ItemsSource = new List<SiOrder>();
            }
        }

        private async void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedOrder = (SiOrder)grdCurrency.SelectedItem;
                if (selectedOrder != null)
                {
                    var orderIdString = selectedOrder.OrderId.ToString();
                    if (int.TryParse(orderIdString, out int orderId))
                    {
                        var result = await _order.DeleteById(orderId);
                        MessageBox.Show(result.Message, "Delete");
                        await RefreshOrderData(); // Refresh data after deletion
                    }
                    else
                    {
                        MessageBox.Show("OrderId must be a valid integer.", "Error");
                    }
                }
                else
                {
                    MessageBox.Show("No order selected.", "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private async void ButtonEdit_Click_1(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                var orderId = (int)button.CommandParameter;
                var addOrderWindow = new AddOrder(orderId);
                addOrderWindow.ShowDialog();
            }
        }

        private async void grdOrder_MouseDouble_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedOrder = grdCurrency.SelectedItem as SiOrder;
            if (selectedOrder != null)
            {
                var addOrder = new AddOrder(selectedOrder.OrderId);
                addOrder.ShowDialog();
            }
        }

        private void Open_AddOrder_Click(object sender, RoutedEventArgs e)
        {
            var addOrder = new AddOrder(); 
            addOrder.ShowDialog();
        }

        private void ButtonView_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is int orderId)
            {
                ViewOrder viewOrderWindow = new ViewOrder(orderId);
                viewOrderWindow.ShowDialog();
            }
        }
    }
}
