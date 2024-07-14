using Jewelry.Business;
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
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        private readonly IProductBusiness _business;
        private SiProduct _updateProduct;
        public AddProduct()
        {
            InitializeComponent();
            _business = new ProductBusiness();
        }
        public AddProduct(SiProduct selectedProduct)
        {
            InitializeComponent();
            _business = new ProductBusiness();
            _updateProduct = selectedProduct;
            DataContext = _updateProduct; // Set the DataContext to the selected product

        }
        // Add event handlers for save and cancel buttons
        private async void grdProduct_ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var item = await _business.GetById(int.Parse(ProductId.Text));
                if (item.Data == null)
                {

                    var products = new SiProduct()
                    {
                        ProductId = int.Parse(ProductId.Text),
                        Name = Name.Text,
                        CategoryId = int.Parse(CategoryId.Text),
                        Description = Description.Text,
                        Barcode = Barcode.Text,
                        Weight = double.Parse(Weight.Text),
                        CostPrice = int.Parse(CostPrice.Text),
                        GoldPrice = int.Parse(GoldPrice.Text),
                        LaborCost = int.Parse(LaborCost.Text),
                        StoneCost = int.Parse(StoneCost.Text),
                        SellPriceRatio = double.Parse(SellPriceRatio.Text)
                    };

                    var result = await _business.Save(products);
                    MessageBox.Show(result.Message, "Save");

                }
                else
                {
                    var product = item.Data as SiProduct;
                    if (product != null)
                    {
                        product.Name = Name.Text;
                        product.CategoryId = int.Parse(CategoryId.Text);
                        product.Description = Description.Text;
                        product.Barcode = Barcode.Text;
                        product.Weight = double.Parse(Weight.Text);
                        product.CostPrice = int.Parse(CostPrice.Text);
                        product.GoldPrice = int.Parse(GoldPrice.Text);
                        product.LaborCost = int.Parse(LaborCost.Text);
                        product.StoneCost = int.Parse(StoneCost.Text);
                        product.SellPriceRatio = double.Parse(SellPriceRatio.Text);
                    }

                    Console.WriteLine("save CHANGES");
                    var result = await _business.Update(product);
                    MessageBox.Show(result.Message, "Save");
                }


                ProductId.Text = string.Empty;
                Name.Text = string.Empty;
                CategoryId.Text = string.Empty;
                Description.Text = string.Empty;
                Barcode.Text = string.Empty;
                Weight.Text = string.Empty;
                CostPrice.Text = string.Empty;
                GoldPrice.Text = string.Empty;
                LaborCost.Text = string.Empty;
                StoneCost.Text = string.Empty;
                SellPriceRatio.Text = string.Empty;

                clearForm();
             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
            finally
            {
                this.Close();
            }

        }
        private void clearForm()
        {
            ProductId.Text = string.Empty;
            Name.Text = string.Empty;
            CategoryId.Text = string.Empty;
            Description.Text = string.Empty;
            Barcode.Text = string.Empty;
            Weight.Text = string.Empty;
            CostPrice.Text = string.Empty;
            GoldPrice.Text = string.Empty;
            LaborCost.Text = string.Empty;
            StoneCost.Text = string.Empty;
            SellPriceRatio.Text = string.Empty;
        }
        private void grdProduct_ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
