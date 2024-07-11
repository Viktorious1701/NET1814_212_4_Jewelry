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
            var context = new Net1814_212_4_JewelryContext();
            var unitOfWork = new UnitOfWork(context);
            _order = new OrderBusiness(unitOfWork, context);
            this.LoadOrders();
        }

        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
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
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void LoadOrders(string searchInput = null)
        {
            try
            {
                var result = await _order.GetAll();
                if (result.Status > 0 && result.Data != null)
                {
                    var orders = result.Data as IEnumerable<SiOrder>;
                    if (!string.IsNullOrEmpty(searchInput))
                    {
                        // Splitting the input into column name and search term
                        var parts = searchInput.Split(new[] { ' ' }, 2);
                        if (parts.Length == 2)
                        {
                            var columnName = parts[0];
                            var searchTerm = parts[1];

                            orders = orders.Where(o => DoesColumnContain(o, columnName, searchTerm, StringComparison.OrdinalIgnoreCase));
                        }
                    }
                    grdCurrency.ItemsSource = orders;
                }
                else
                {
                    MessageBox.Show(result.Message, "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private bool DoesColumnContain(SiOrder order, string columnName, string searchTerm, StringComparison comparison)
        {
            switch (columnName)
            {
                case "CustomerId":
                    return order.CustomerId.ToString().Contains(searchTerm, comparison);
                case "PromotionId":
                    return order.PromotionId?.ToString().Contains(searchTerm, comparison) ?? false;
                case "OrderDate":
                    return order.OrderDate.ToString().Contains(searchTerm, comparison);
                case "TotalAmount":
                    return order.TotalAmount.ToString().Contains(searchTerm, comparison);
                case "Discount":
                    return order.Discount?.ToString().Contains(searchTerm, comparison) ?? false;
                case "PaymentMethod":
                    return order.PaymentMethod?.Contains(searchTerm, comparison) ?? false;
                case "PaymentStatus":
                    return order.PaymentStatus?.Contains(searchTerm, comparison) ?? false;
                case "ShipmentStatus":
                    return order.ShipmentStatus?.Contains(searchTerm, comparison) ?? false;
                default:
                    return false;
            }
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            // Get the search term from the TextBox
            var searchTerm = Search.Text;
            // Call LoadOrders with the search term
            LoadOrders(searchTerm);
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
            try
            {
                var button = sender as Button;
                var item = button?.DataContext as SiOrder;

                if (item != null)
                {
                    OrderId.Text = Convert.ToString(item.OrderId);
                    CustomerId.Text = Convert.ToString(item.CustomerId);
                    PromotionId.Text = Convert.ToString(item.PromotionId);
                    if (item.OrderDate.HasValue)
                    {
                        OrderDate.Text = item.OrderDate.Value.ToString("MM/dd/yyyy");
                    }
                    TotalAmount.Text = Convert.ToString(item.TotalAmount);
                    Discount.Text = Convert.ToString(item.Discount);
                    PaymentMethod.Text = item.PaymentMethod;
                    PaymentStatus.Text = item.PaymentStatus;
                    ShipmentStatus.Text = item.ShipmentStatus;

                    LoadOrders();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void grdCurrency_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var order = ((FrameworkElement)e.OriginalSource).DataContext as SiOrder; // Replace OrderType with your actual order class
            if (order != null)
            {
                // Populate the form fields with the order details for editing
                OrderId.Text = order.OrderId.ToString();
                CustomerId.Text = order.CustomerId?.ToString() ?? "";
                PromotionId.Text = order.PromotionId?.ToString() ?? "";
                OrderDate.Text = order.OrderDate.ToString();
                TotalAmount.Text = order.TotalAmount.ToString();
                Discount.Text = order.Discount.ToString();
                PaymentMethod.Text = order.PaymentMethod;
                PaymentStatus.Text = order.PaymentStatus;
                ShipmentStatus.Text = order.ShipmentStatus;
            }
        }
    }
}
