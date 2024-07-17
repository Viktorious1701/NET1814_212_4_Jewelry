using Jewelry.Data.Models;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Jewelry.WpfApp
{

    public partial class ViewOrderItem : Window
    {
        public ViewOrderItem(OrderItem orderItem)
        {
            InitializeComponent();
            if (orderItem != null)
            {
                TextOrderItemId.Text = orderItem.OrderItemId.ToString();
                TextOrderId.Text = orderItem.OrderId.ToString();
                TextProductId.Text = orderItem.ProductId.ToString();
                TextQuantity.Text = orderItem.Quantity.ToString();
                TextPrice.Text = orderItem.Price.ToString();
                TextStatus.Text = orderItem.Status;
                TextCustomerId.Text = orderItem.CustomerId.ToString();
            }
        }
    }
}
