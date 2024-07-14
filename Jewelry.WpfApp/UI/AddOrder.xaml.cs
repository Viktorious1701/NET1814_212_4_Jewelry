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
using System.Diagnostics;

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
            InitializeComponent();
            _order = new OrderBusiness();
            this.DataContext = this; // Set DataContext to this window
            LoadOrderData(orderId);
        }

        private async void LoadOrderData(int orderId)
        {
            var item = await _order.GetById(orderId);
            if (item?.Data is SiOrder order)
            {
                // Directly update UI elements
                OrderId.Text = orderId.ToString();
                CustomerId.Text = order.CustomerId.ToString();
                PromotionId.Text = order.PromotionId?.ToString() ?? "";
                OrderDate.Text = order.OrderDate?.ToDateTime(TimeOnly.MinValue).ToString("MM/dd/yyyy") ?? "";
                TotalAmount.Text = order.TotalAmount.ToString(); // Assuming you want to format the amount
                Discount.Text = order.Discount.ToString(); // Same assumption as above
                PaymentMethod.Text = order.PaymentMethod;
                PaymentStatus.Text = order.PaymentStatus;
                ShipmentStatus.Text = order.ShipmentStatus;
            }
            else
            {
                MessageBox.Show("Order not found.", "Error");
            }
        }

        private async void grdOrder_ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Parse and validate the OrderId
                if (!int.TryParse(OrderId.Text, out int orderId))
                {
                    MessageBox.Show("Invalid Order ID. Please enter a valid number.", "Error");
                    return;
                }

                // Parse and validate the OrderDate
                if (!DateTime.TryParseExact(OrderDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
                {
                    MessageBox.Show("OrderDate must be a valid date in the 'MM/dd/yyyy' format.", "Error");
                    return;
                }
                DateOnly orderDate = DateOnly.FromDateTime(dateTime);

                // Parse other fields
                if (!int.TryParse(CustomerId.Text, out int customerId) ||
                    !int.TryParse(TotalAmount.Text, out int totalAmount) ||
                    !int.TryParse(Discount.Text, out int discount))
                {
                    MessageBox.Show("Invalid input for CustomerId, TotalAmount, or Discount. Please enter valid numbers.", "Error");
                    return;
                }

                int? promotionId = int.TryParse(PromotionId.Text, out int tempPromotionId) ? tempPromotionId : (int?)null;

                // Create or update the order
                SiOrder order;
                var existingOrder = await _order.GetById(orderId);

                if (existingOrder.Data == null)
                {
                    // Create new order
                    order = new SiOrder
                    {
                        OrderId = orderId,
                        CustomerId = customerId,
                        PromotionId = promotionId,
                        OrderDate = orderDate,
                        TotalAmount = totalAmount,
                        Discount = discount,
                        PaymentMethod = PaymentMethod.Text,
                        PaymentStatus = PaymentStatus.Text,
                        ShipmentStatus = ShipmentStatus.Text
                    };

                    var saveResult = await _order.Save(order);
                    MessageBox.Show(saveResult.Message, "Save");
                }
                else
                {
                    // Update existing order
                    order = existingOrder.Data as SiOrder;
                    if (order != null)
                    {
                        order.CustomerId = customerId;
                        order.PromotionId = promotionId;
                        order.OrderDate = orderDate;
                        order.TotalAmount = totalAmount;
                        order.Discount = discount;
                        order.PaymentMethod = PaymentMethod.Text;
                        order.PaymentStatus = PaymentStatus.Text;
                        order.ShipmentStatus = ShipmentStatus.Text;

                        var updateResult = await _order.Update(order);
                        MessageBox.Show(updateResult.Message, "Update");
                    }
                }

                clearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error");
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