
using Jewelry.Business;
using Jewelry.Data.Models;
using System.Windows;
using System.Windows.Controls;

namespace Jewelry.WpfApp.UI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class wProduct : Window
    {
        private SiProduct _selectedProduct;
        private readonly IProductBusiness _business;
        public wProduct()
        {
            InitializeComponent();
            _business = new ProductBusiness();
            Loaded += OnWindowLoaded;
        }
        private async void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedProduct != null)
                {
                    int newProductId = int.Parse(ProductId.Text);
                    string newProductName = Name.Text;

                    int newCategoryId = int.Parse(CategoryId.Text);
                    string newDescription = Description.Text;
                    string newBarcode = Barcode.Text;
                    double newWeight = double.Parse(Weight.Text);
                    int newCostPrice = int.Parse(CostPrice.Text);
                    int newGoldPrice = int.Parse(GoldPrice.Text);
                    int newLaborCost = int.Parse(LaborCost.Text);
                    int newStoneCost = int.Parse(StoneCost.Text);
                    double newSellPriceRatio = double.Parse(SellPriceRatio.Text);
              
                    _selectedProduct.ProductId = newProductId;
                    _selectedProduct.Name = newProductName;
                    _selectedProduct.CategoryId = newCategoryId;
                    _selectedProduct.Description = newDescription;
                    _selectedProduct.Barcode = newBarcode;
                    _selectedProduct.Weight = newWeight;
                    _selectedProduct.CostPrice = newCostPrice;
                    _selectedProduct.GoldPrice = newGoldPrice;
                    _selectedProduct.LaborCost = newLaborCost;
                    _selectedProduct.StoneCost = newStoneCost;
                    _selectedProduct.SellPriceRatio = newSellPriceRatio;
                    var result = await _business.Update(_selectedProduct);
                    MessageBox.Show(result.Message, "Update");

                   
                    LoadGrdProducts();
                }
                else
                {
                    throw new Exception("No product selected for update.");
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
                _selectedProduct = button?.DataContext as SiProduct;

                if (_selectedProduct != null)
                {
                    ProductId.Text = _selectedProduct.ProductId.ToString();
                    Name.Text = _selectedProduct.Name;

                    ButtonUpdate.Visibility = Visibility.Visible;
                    ButtonSave.Visibility = Visibility.Collapsed;
                    LoadGrdProducts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }
        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
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

                ProductId.Text = string.Empty;
                CategoryId.Text = string.Empty;
                Description.Text = string.Empty;
                Barcode.Text = string.Empty;
                Weight.Text = string.Empty;
                CostPrice.Text = string.Empty;
                GoldPrice.Text = string.Empty;
                LaborCost.Text = string.Empty;
                StoneCost.Text = string.Empty;
                SellPriceRatio.Text = string.Empty;

                LoadGrdProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }

        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            ProductId.Text = string.Empty;
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
        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {

        }
        private async void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            await LoadGrdProducts();
        }

        private async Task LoadGrdProducts()
        {
            var result = await _business.GetAll();

            if (result.Status > 0 && result.Data != null)
            {
                grdProduct.ItemsSource = result.Data as List<SiProduct>;
            }
            else
            {
                grdProduct.ItemsSource = new List<SiProduct>();
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var product = button?.DataContext as SiProduct;

                if (product != null)
                {
                    // Assuming you have a TextBox named `productName` in your XAML to edit the product name
                    product.Name = Name.Text;

                    // Refresh the DataGrid
                    LoadGrdProducts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private async void ButtonDelete_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = grdProduct.SelectedItem as SiProduct;
                if (result != null)
                {
                    await _business.DeleteById(result.ProductId);
                    LoadGrdProducts();
                }
                else
                {
                    throw new Exception(e.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }
    }
}
