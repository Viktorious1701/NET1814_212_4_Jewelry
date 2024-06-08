using Jewelry.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Jewelry.Data.Validation
{
    public class OrderItemValidation
    {
        private readonly NET1814_212_4_JewelryContext _context;
        public bool ValidateOrderItem(OrderItem orderItem)
        {
            if(orderItem == null)
            {
                return false;
            }
            Regex regex = new Regex((@"^\d$"));
            if (orderItem.Price <= 0)
            {
                return false;
            }

            if (orderItem.Quantity <= 0)
            {
                return false;
            }

            if (!regex.IsMatch(orderItem.ProductId.ToString()) ||
                !regex.IsMatch(orderItem.OrderItemId.ToString()) || 
                !regex.IsMatch(orderItem.OrderId.ToString()))
            {
                return false;
            }

            if(_context.FindAsync<OrderItem>(orderItem.OrderItemId) == null)
            {
                return false;
            }

            return true;
        }
    }
}
