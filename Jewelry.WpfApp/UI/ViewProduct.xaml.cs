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
    /// <summary>
    /// Interaction logic for ViewProduct.xaml
    /// </summary>
    public partial class ViewProduct : Window
    {
        public ViewProduct(SiProduct product)
        {
            InitializeComponent();
            if (product != null)
            {
                TextProductId.Text = product.ProductId.ToString();
                TextName.Text = product.Name;
                TextCategoryId.Text = product.CategoryId.ToString();
                TextDescription.Text = product.Description;
                TextBarcode.Text = product.Barcode;
                TextWeight.Text = product.Weight?.ToString();
                TextCostPrice.Text = product.CostPrice?.ToString();
                TextGoldPrice.Text = product.GoldPrice?.ToString();
                TextLaborCost.Text = product.LaborCost?.ToString();
                TextStoneCost.Text = product.StoneCost?.ToString();
                TextSellPriceRatio.Text = product.SellPriceRatio?.ToString();
            }
        }
    }
}
