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

        /*private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = await _order.GetById(OrderId.Text);

                DateTime dateTime;
                if (!DateTime.TryParseExact(OrderDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                {
                    MessageBox.Show("OrderDate must be a valid date in the 'MM/dd/yyyy' format.", "Error");
                    return;
                }

                DateOnly orderDate = DateOnly.FromDateTime(dateTime);

                int? promotionId = null;
                if (int.TryParse(PromotionId.Text, out int tempPromotionId))
                {
                    promotionId = tempPromotionId;
                }

                if (item.Data == null)
                {
                    var order = new SiOrder()
                    {
                        CustomerId = int.Parse(CustomerId.Text),
                        PromotionId = promotionId,
                        OrderDate = orderDate,
                        TotalAmount = int.Parse(TotalAmount.Text),
                        Discount = int.Parse(Discount.Text),
                        PaymentMethod = PaymentMethod.Text,
                        PaymentStatus = PaymentStatus.Text,
                        ShipmentStatus = ShipmentStatus.Text
                    };

                    var result = await _order.Save(order);
                    MessageBox.Show(result.Message, "Save");
                    if (result.Status > 0) // Assuming a positive status indicates success
                    {
                        LoadOrders(); // Refresh the data grid
                    }
                }
                else
                {
                    var siOrder = item.Data as SiOrder;
                    if (siOrder != null)
                    {
                        // Now update the order
                        siOrder.CustomerId = int.Parse(CustomerId.Text);
                        siOrder.PromotionId = promotionId;
                        siOrder.OrderDate = orderDate;
                        siOrder.TotalAmount = int.Parse(TotalAmount.Text);
                        siOrder.Discount = int.Parse(Discount.Text);
                        siOrder.PaymentMethod = PaymentMethod.Text;
                        siOrder.PaymentStatus = PaymentStatus.Text;
                        siOrder.ShipmentStatus = ShipmentStatus.Text;

                        var result = await _order.Update(siOrder);
                        MessageBox.Show(result.Message, "Save");
                        if (result.Status > 0) // Assuming a positive status indicates success
                        {
                            LoadOrders(); // Refresh the data grid
                        }
                    }
                }

                OrderId.Text = string.Empty;
                CustomerId.Text = string.Empty;
                PromotionId.Text = string.Empty;
                OrderDate.Text = string.Empty;
                TotalAmount.Text = string.Empty;
                Discount.Text = string.Empty;
                PaymentMethod.Text = string.Empty;
                PaymentStatus.Text = string.Empty;
                ShipmentStatus.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }*/

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
            var result = await _order.GetAll();
            if (result.Status > 0 && result.Data != null)
            {
                grdCurrency.ItemsSource = result.Data as List<SiProduct>;
            }
            else
            {
                grdCurrency.ItemsSource = new List<SiProduct>();
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
                var selectedOrder = (SiOrder)grdCurrency.SelectedItem; // Assuming your DataGrid is bound to a collection of Order objects
                if (selectedOrder != null)
                {
                    var orderIdString = selectedOrder.OrderId.ToString();
                    MessageBox.Show($"OrderId: {orderIdString}"); // Debug message
                    if (int.TryParse(orderIdString, out int orderId))
                    {
                        var result = await _order.DeleteById(orderId);
                        MessageBox.Show(result.Message, "Delete");
                        this.LoadOrders();
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

        private void ButtonEdit_Click_1(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                var orderId = (int)button.CommandParameter;
                var addOrderWindow = new AddOrder(orderId);
                addOrderWindow.Show(); // Changed from ShowDialog() to Show()
            }
        }

        private void grdOrder_MouseDouble_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedOrder = grdCurrency.SelectedItem as SiOrder;
            if (selectedOrder != null)
            {
                var addOrder = new AddOrder();
                addOrder.ShowDialog();
                LoadOrders(); // Refresh the order list after adding/updating a order
            }
        }
    }
}
