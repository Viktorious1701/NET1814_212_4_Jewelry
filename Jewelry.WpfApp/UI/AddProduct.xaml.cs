using Jewelry.Business;
using Jewelry.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Jewelry.WpfApp.UI
{
    /// <summary>
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        private readonly IProductBusiness _productBusiness;
        private readonly ICategoryBusiness _categoryBusiness;
        private SiProduct _updateProduct;

        public AddProduct()
        {
            InitializeComponent();
            _productBusiness = new ProductBusiness();
            _categoryBusiness = new CategoryBusiness();
            LoadCategories();
        }

        public AddProduct(SiProduct selectedProduct) : this()
        {
            _updateProduct = selectedProduct;
            DataContext = _updateProduct; // Set the DataContext to the selected product
            LoadProductData(_updateProduct.ProductId);
        }

        private async void LoadCategories()
        {
            try
            {
                var categories = await _categoryBusiness.GetAllCategoriesAsync();
                CategoryComboBox.ItemsSource = (System.Collections.IEnumerable)categories;
                CategoryComboBox.DisplayMemberPath = "CategoryName";
                CategoryComboBox.SelectedValuePath = "CategoryId";

                if (_updateProduct?.CategoryId != null)
                {
                    CategoryComboBox.SelectedValue = _updateProduct.CategoryId;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load categories: {ex.Message}", "Error");
            }
        }

        private async void LoadProductData(int productId)
        {
            try
            {
                var item = await _productBusiness.GetById(productId);
                if (item?.Data is SiProduct product)
                {
                    // Set values directly to the UI controls
                    ProductId.Text = productId.ToString();
                    Name.Text = product.Name;
                    CategoryComboBox.SelectedValue = product.CategoryId; // Set the selected category
                    Description.Text = product.Description;
                    Barcode.Text = product.Barcode;
                    Weight.Text = product.Weight.ToString();
                    CostPrice.Text = product.CostPrice.ToString();
                    GoldPrice.Text = product.GoldPrice.ToString();
                    LaborCost.Text = product.LaborCost.ToString();
                    StoneCost.Text = product.StoneCost.ToString();
                    SellPriceRatio.Text = product.SellPriceRatio.ToString();
                }
                else
                {
                    MessageBox.Show("Product not found.", "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load product data: {ex.Message}", "Error");
            }
        }

        private async void grdProduct_ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var productId = int.Parse(ProductId.Text);
                var existingProduct = await _productBusiness.GetById(productId);

                if (existingProduct.Data == null)
                {
                    var newProduct = new SiProduct
                    {
                        ProductId = productId,
                        Name = Name.Text,
                        CategoryId = (int?)CategoryComboBox.SelectedValue,
                        Description = Description.Text,
                        Barcode = Barcode.Text,
                        Weight = double.Parse(Weight.Text),
                        CostPrice = int.Parse(CostPrice.Text),
                        GoldPrice = int.Parse(GoldPrice.Text),
                        LaborCost = int.Parse(LaborCost.Text),
                        StoneCost = int.Parse(StoneCost.Text),
                        SellPriceRatio = double.Parse(SellPriceRatio.Text)
                    };

                    var result = await _productBusiness.Save(newProduct);
                    MessageBox.Show(result.Message, "Save");
                }
                else
                {
                    var product = existingProduct.Data as SiProduct;
                    if (product != null)
                    {
                        product.Name = Name.Text;
                        product.CategoryId = (int?)CategoryComboBox.SelectedValue;
                        product.Description = Description.Text;
                        product.Barcode = Barcode.Text;
                        product.Weight = double.Parse(Weight.Text);
                        product.CostPrice = int.Parse(CostPrice.Text);
                        product.GoldPrice = int.Parse(GoldPrice.Text);
                        product.LaborCost = int.Parse(LaborCost.Text);
                        product.StoneCost = int.Parse(StoneCost.Text);
                        product.SellPriceRatio = double.Parse(SellPriceRatio.Text);

                        var result = await _productBusiness.Update(product);
                        MessageBox.Show(result.Message, "Save");
                    }
                }

                ClearForm();
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

        private void ClearForm()
        {
            ProductId.Text = string.Empty;
            Name.Text = string.Empty;
            CategoryComboBox.SelectedIndex = -1; // Reset the combo box
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
