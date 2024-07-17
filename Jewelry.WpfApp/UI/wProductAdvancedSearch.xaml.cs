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

namespace Jewelry.WpfApp.UI
{
    /// <summary>
    /// Interaction logic for wProductAdvancedSearch.xaml
    /// </summary>
    public partial class wProductAdvancedSearch : Window
    {
        // this is the full products
        public List<SiProduct> Products { get; set; }
        // This is the 
        public List<SiProduct> FilteredProducts { get; set; }
        public wProductAdvancedSearch(List<SiProduct>? products)
        {
            InitializeComponent();
            Products = products;
            FilteredProducts = new List<SiProduct>();
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            FilteredProducts = Products;

            if (!string.IsNullOrEmpty(ProductIdTextBox.Text))
            {
                // change to string, contains will check if a set of character is in it, and make it to list after that
                FilteredProducts = FilteredProducts.Where(p => p.ProductId.ToString().Contains(ProductIdTextBox.Text)).ToList();
            }

            if (!string.IsNullOrEmpty(NameTextBox.Text))
            {
                FilteredProducts = FilteredProducts.Where(p => p.Name.Contains(NameTextBox.Text, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(CategoryIdTextBox.Text))
            {
                FilteredProducts = FilteredProducts.Where(p => p.CategoryId.ToString().Contains(CategoryIdTextBox.Text)).ToList();
            }

            if (!string.IsNullOrEmpty(DescriptionTextBox.Text))
            {
                FilteredProducts = FilteredProducts.Where(p => p.Description.Contains(DescriptionTextBox.Text, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(BarcodeTextBox.Text))
            {
                FilteredProducts = FilteredProducts.Where(p => p.Barcode.Contains(BarcodeTextBox.Text, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(WeightTextBox.Text))
            {
                FilteredProducts = FilteredProducts.Where(p => p.Weight.ToString().Contains(WeightTextBox.Text)).ToList();
            }

            if (!string.IsNullOrEmpty(CostPriceTextBox.Text))
            {
                FilteredProducts = FilteredProducts.Where(p => p.CostPrice.ToString().Contains(CostPriceTextBox.Text)).ToList();
            }

            if (!string.IsNullOrEmpty(GoldPriceTextBox.Text))
            {
                FilteredProducts = FilteredProducts.Where(p => p.GoldPrice.ToString().Contains(GoldPriceTextBox.Text)).ToList();
            }

            if (!string.IsNullOrEmpty(LaborCostTextBox.Text))
            {
                FilteredProducts = FilteredProducts.Where(p => p.LaborCost.ToString().Contains(LaborCostTextBox.Text)).ToList();
            }

            if (!string.IsNullOrEmpty(StoneCostTextBox.Text))
            {
                FilteredProducts = FilteredProducts.Where(p => p.StoneCost.ToString().Contains(StoneCostTextBox.Text)).ToList();
            }

            if (!string.IsNullOrEmpty(SellPriceRatioTextBox.Text))
            {
                FilteredProducts = FilteredProducts.Where(p => p.SellPriceRatio.ToString().Contains(SellPriceRatioTextBox.Text)).ToList();
            }

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
