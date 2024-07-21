using Jewelry.Business;
using Jewelry.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Jewelry.WpfApp.UI
{
    public partial class wProduct : Window
    {
        private readonly IProductBusiness _business;
        public wProduct()
        {
            InitializeComponent();
            _business = new ProductBusiness();
            Loaded += OnWindowLoaded;
        }

        private async void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            await LoadGrdProducts();
            LoadColumns();
        }

        private async Task RefreshProductData()
        {
            await LoadGrdProducts();
        }

        public async Task LoadGrdProducts()
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


        private void LoadColumns()
        {
            // Adding column names to the ComboBox
            var columns = new List<string>
            {
                "ProductId",
                "Name",
                "CategoryId",
                "Description",
                "Barcode",
                "Weight",
                "CostPrice",
                "GoldPrice",
                "LaborCost",
                "StoneCost",
                "SellPriceRatio"
            };
            ColumnComboBox.ItemsSource = columns;
        }

        private async void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            var selectedColumn = ColumnComboBox.SelectedItem as string;
            var searchText = SearchTextBox.Text;

            var result = await _business.GetAll(); // Assume this gets all products
            if (result.Status > 0 && result.Data != null)
            {
                var filteredData = result.Data as List<SiProduct>;

                if (!string.IsNullOrEmpty(selectedColumn) && !string.IsNullOrEmpty(searchText))
                {
                    filteredData = filteredData.Where(p =>
                    {
                        var property = p.GetType().GetProperty(selectedColumn);
                        if (property != null)
                        {
                            var value = property.GetValue(p, null)?.ToString();
                            return value != null && value.Contains(searchText, StringComparison.OrdinalIgnoreCase);
                        }
                        return false;
                    }).ToList();
                }

                grdProduct.ItemsSource = filteredData;
            }
            else
            {
                grdProduct.ItemsSource = new List<SiProduct>();
            }
        }


        private void ButtonAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            var addProductWindow = new AddProduct();
            addProductWindow.Closed += async (s, args) => await RefreshProductData(); // Refresh data when the window is closed
            addProductWindow.ShowDialog();
        }
        private async void ButtonView_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                var productId = (int)button.CommandParameter;
                var selectedProduct = await _business.GetById(productId);
                if (selectedProduct != null && selectedProduct.Data != null)
                {
                    var viewProductWindow = new ViewProduct(selectedProduct.Data as SiProduct);
                    viewProductWindow.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Something went wrong with this record");
                }
            }
        }



        private async void grdProduct_ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                var productId = (int)button.CommandParameter;
                var result = MessageBox.Show("Are you sure you want to delete this product?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    var deleteResult = await _business.DeleteById(productId);
                    if (deleteResult.Status > 0)
                    {
                        await RefreshProductData(); // Refresh the product list after deletion
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete the product.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void grdProduct_MouseDouble_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedProduct = grdProduct.SelectedItem as SiProduct;
            if (selectedProduct != null)
            {
                var addProduct = new AddProduct(selectedProduct);
                addProduct.Closed += async (s, args) => await RefreshProductData(); // Refresh data when the window is closed
                addProduct.ShowDialog();
            }
        }
        private async void ButtonAdvancedSearch_Click(object sender, RoutedEventArgs e)
        {
            var result = await _business.GetAll();
            if (result.Status > 0 && result.Data != null)
            {
                var products = result.Data as List<SiProduct>;
                var filteredProducts = products;
                if(filteredProducts == null)
                {
                    MessageBox.Show("No product list");
                    return;
                }
                // Apply filters
                if (!string.IsNullOrEmpty(ProductIdTextBox.Text))
                {
                    filteredProducts = filteredProducts.Where(p => p.ProductId.ToString().Contains(ProductIdTextBox.Text)).ToList();
                }

                if (!string.IsNullOrEmpty(NameTextBox.Text))
                {
                    filteredProducts = filteredProducts.Where(p => p.Name.Contains(NameTextBox.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (!string.IsNullOrEmpty(CategoryIdTextBox.Text))
                {
                    filteredProducts = filteredProducts.Where(p => p.CategoryId.ToString().Contains(CategoryIdTextBox.Text)).ToList();
                }

                if (!string.IsNullOrEmpty(DescriptionTextBox.Text))
                {
                    filteredProducts = filteredProducts.Where(p => p.Description.Contains(DescriptionTextBox.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (!string.IsNullOrEmpty(BarcodeTextBox.Text))
                {
                    filteredProducts = filteredProducts.Where(p => p.Barcode.Contains(BarcodeTextBox.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (!string.IsNullOrEmpty(WeightTextBox.Text))
                {
                    filteredProducts = filteredProducts.Where(p => p.Weight.ToString().Contains(WeightTextBox.Text)).ToList();
                }

                if (!string.IsNullOrEmpty(CostPriceTextBox.Text))
                {
                    filteredProducts = filteredProducts.Where(p => p.CostPrice.ToString().Contains(CostPriceTextBox.Text)).ToList();
                }

                if (!string.IsNullOrEmpty(GoldPriceTextBox.Text))
                {
                    filteredProducts = filteredProducts.Where(p => p.GoldPrice.ToString().Contains(GoldPriceTextBox.Text)).ToList();
                }

                if (!string.IsNullOrEmpty(LaborCostTextBox.Text))
                {
                    filteredProducts = filteredProducts.Where(p => p.LaborCost.ToString().Contains(LaborCostTextBox.Text)).ToList();
                }

                if (!string.IsNullOrEmpty(StoneCostTextBox.Text))
                {
                    filteredProducts = filteredProducts.Where(p => p.StoneCost.ToString().Contains(StoneCostTextBox.Text)).ToList();
                }

                if (!string.IsNullOrEmpty(SellPriceRatioTextBox.Text))
                {
                    filteredProducts = filteredProducts.Where(p => p.SellPriceRatio.ToString().Contains(SellPriceRatioTextBox.Text)).ToList();
                }

                // Update the DataGrid with filtered products
                grdProduct.ItemsSource = filteredProducts;
            }
            else
            {
                MessageBox.Show("Failed to load products.");
            }
        }


    }
}
