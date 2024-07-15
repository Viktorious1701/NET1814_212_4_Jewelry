using Jewelry.Business;
using Jewelry.Data.Models;
using System;
using System.Windows;

namespace Jewelry.WpfApp.UI
{
    public partial class ViewOrder : Window
    {
        private readonly OrderBusiness _order;

        public ViewOrder(int orderId)
        {
            InitializeComponent();
            _order = new OrderBusiness();
            LoadOrderData(orderId);
        }

        private async void LoadOrderData(int orderId)
        {
            var item = await _order.GetById(orderId);
            if (item?.Data is SiOrder order)
            {
                txtOrderId.Text = order.OrderId.ToString();
                txtCustomerId.Text = order.CustomerId.ToString();
                txtPromotionId.Text = order.PromotionId?.ToString() ?? "N/A";
                txtOrderDate.Text = order.OrderDate?.ToString("MM/dd/yyyy") ?? "N/A";
                txtTotalAmount.Text = order.TotalAmount.ToString();
                txtDiscount.Text = order.Discount.ToString();
                txtPaymentMethod.Text = order.PaymentMethod;
                txtPaymentStatus.Text = order.PaymentStatus;
                txtShipmentStatus.Text = order.ShipmentStatus;
            }
            else
            {
                MessageBox.Show("Order not found.", "Error");
                this.Close();
            }
        }
    }
}