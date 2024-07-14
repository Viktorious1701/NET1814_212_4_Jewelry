using Jewelry.Business;
using Jewelry.Data;
using Jewelry.Data.Models;
using Jewelry.WpfApp.Models;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Xml.Linq;
using SiOrder = Jewelry.Data.Models.SiOrder;

namespace Jewelry.WpfApp.UI
{
    /// <summary>
    /// Interaction logic for AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Window
    {
        private readonly OrderBusiness _order;

        public AddOrder()
        {
            InitializeComponent();
            _order = new OrderBusiness();
        }
        public AddOrder(int orderId)
        {
            InitializeComponent(); // This initializes the form's controls.
            _order = new OrderBusiness();
            LoadOrderData(orderId); // Now it's safe to access OrderId.Text
        }

        private async void LoadOrderData(int orderId)
        {
            var item = await _order.GetById(orderId);
            if (item.Data is SiOrder order)
            {
                Dispatcher.Invoke(() =>
                {
                    // Update UI elements on the UI thread
                    OrderId.Text = orderId.ToString();
                    CustomerId.Text = order.CustomerId.ToString();
                    PromotionId.Text = order.PromotionId?.ToString() ?? string.Empty; // Handle nullable int
                    OrderDate.Text = order.OrderDate?.ToDateTime(TimeOnly.MinValue).ToString("MM/dd/yyyy"); // Assuming OrderDate is of type DateOnly
                    TotalAmount.Text = order.TotalAmount.ToString();
                    Discount.Text = order.Discount.ToString();
                    PaymentMethod.Text = order.PaymentMethod;
                    PaymentStatus.Text = order.PaymentStatus;
                    ShipmentStatus.Text = order.ShipmentStatus;
                });
            }
            else
            {
                Dispatcher.Invoke(() => MessageBox.Show("Order not found.", "Error"));
            }
        }

        private async void grdOrder_ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(OrderId.Text, out int orderId))
                {
                    MessageBox.Show("Order ID must be a valid integer.", "Error");
                    return;
                }

                var item = await _order.GetById(orderId);
                DateTime dateTime;
                if (!DateTime.TryParseExact(OrderDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                {
                    MessageBox.Show("OrderDate must be a valid date in the 'MM/dd/yyyy' format.", "Error");
                    return;
                }

                DateOnly orderDate = DateOnly.FromDateTime(dateTime);
                int? promotionId = int.TryParse(PromotionId.Text, out int tempPromotionId) ? tempPromotionId : (int?)null;

                if (item.Data == null)
                {
                    var orders = new SiOrder()
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

                    var result = await _order.Save(orders);
                    MessageBox.Show(result.Message, "Save");
                }
                else
                {
                    var order = item.Data as SiOrder;
                    if (order != null)
                    {
                        order.CustomerId = int.Parse(CustomerId.Text);
                        order.PromotionId = promotionId;
                        order.OrderDate = orderDate;
                        order.TotalAmount = int.Parse(TotalAmount.Text);
                        order.Discount = int.Parse(Discount.Text);
                        order.PaymentMethod = PaymentMethod.Text;
                        order.PaymentStatus = PaymentStatus.Text;
                        order.ShipmentStatus = ShipmentStatus.Text;

                        var result = await _order.Update(order);
                        MessageBox.Show(result.Message, "Save");
                    }
                }

                clearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void clearForm()
        {
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

        private void grdOrder_ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}