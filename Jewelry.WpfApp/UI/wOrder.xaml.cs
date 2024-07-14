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
            LoadColumns();
        }

        private async Task RefreshOrderData()
        {
            await LoadOrders();
        }

        private void LoadColumns()
        {
            // Adding column names to the ComboBox
            var columns = new List<string>
            {
                "OrderId",
                "CustomerId",
                "PromotionId",
                "OrderDate",
                "TotalAmount",
                "Discount",
                "PaymentMethod",
                "PaymentStatus",
                "ShipmentStatus",
            };
            ColumnComboBox.ItemsSource = columns;
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
            var selectedColumn = ColumnComboBox.SelectedItem as string;
            var searchText = SearchTextBox.Text;

            var result = await _order.GetAll();
            if (result.Status > 0 && result.Data != null)
            {
                var filteredData = result.Data as List<SiOrder>;

                if (!string.IsNullOrEmpty(selectedColumn) && !string.IsNullOrEmpty(searchText))
                {
                    filteredData = filteredData.Where(p =>
                    {
                        var property = p.GetType().GetProperty(selectedColumn);
                        if (property != null)
                        {
                            var value = property.GetValue(p, null)?.ToString();
                            return value != null && value.Contains(searchText, StringComparison.OrdinalIgnoreCase);
                        }
                        return false;
                    }).ToList();
                }

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
                addOrderWindow.Closed += async (s, args) => await RefreshOrderData(); // Refresh data when the window is closed
                addOrderWindow.Show();
            }
        }

        private async void grdOrder_MouseDouble_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedOrder = grdCurrency.SelectedItem as SiOrder;
            if (selectedOrder != null)
            {
                var addOrder = new AddOrder(selectedOrder.OrderId);
                addOrder.Closed += async (s, args) => await RefreshOrderData(); // Refresh data when the window is closed
                addOrder.ShowDialog();
            }
        }
    }
}
