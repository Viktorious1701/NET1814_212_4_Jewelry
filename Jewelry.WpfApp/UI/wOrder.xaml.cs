using Jewelry.Business;
using Jewelry.Data;
using Jewelry.Data.Models;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
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

        private async void LoadOrders()
        {
            try
            {
                var result = await _order.GetAll();
                if (result.Status > 0 && result.Data != null)
                {
                    grdCurrency.ItemsSource = (System.Collections.IEnumerable)result.Data;
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

        
    }
}
