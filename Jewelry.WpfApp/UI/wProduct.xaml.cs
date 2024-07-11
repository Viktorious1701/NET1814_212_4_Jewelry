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
            addProductWindow.ShowDialog();
            LoadGrdProducts(); // Refresh the product list after adding/updating a product
        }

        private void ButtonEdit_Click_1(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                var productId = (int)button.CommandParameter;
                var addProductWindow = new AddProduct(productId);
                addProductWindow.ShowDialog();
                LoadGrdProducts(); // Refresh the product list after adding/updating a product
            }
        }

        private async void grdProduct_ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                var productId = (int)button.CommandParameter;
                var result = await _business.DeleteById(productId);
                if (result.Status > 0)
                {
                    LoadGrdProducts(); // Refresh the product list after deletion
                }
                else
                {
                    MessageBox.Show("Failed to delete the product.");
                }
            }
        }

        private void grdProduct_MouseDouble_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedProduct = grdProduct.SelectedItem as SiProduct;
            if (selectedProduct != null)
            {
                var addProduct = new AddProduct();
                addProduct.ShowDialog();
                LoadGrdProducts(); // Refresh the product list after adding/updating a product
            }
        }
    }
}
