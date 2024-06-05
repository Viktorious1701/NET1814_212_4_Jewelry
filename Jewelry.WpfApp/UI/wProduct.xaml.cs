
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
        private readonly IProductBusiness _business;
        public wProduct()
        {
            InitializeComponent();
            _business = new ProductBusiness();
            Loaded += OnWindowLoaded;
        }
        private void ButtonEdit_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var item = button?.DataContext as SiProduct;

                if (item != null)
                {
                    ProductId.Text = Convert.ToString(item.ProductId);
                    CategoryId.Text = Convert.ToString(item.CategoryId);
                    Name.Text = item.Name;
                    Description.Text = item.Description;
                    Barcode.Text = item?.Barcode;
                    Weight.Text = Convert.ToString(item.Weight);
                    CostPrice.Text = Convert.ToString(item.CostPrice);
                    GoldPrice.Text = Convert.ToString(item.GoldPrice);
                    LaborCost.Text = Convert.ToString(item.LaborCost);
                    StoneCost.Text = Convert.ToString(item.StoneCost);
                    SellPriceRatio.Text = Convert.ToString(item.SellPriceRatio);

                    LoadGrdProducts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }
        private async void grdProduct_ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                var item = await _business.GetById(int.Parse(ProductId.Text));
                if(item.Data == null) {

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
                    if(product !=null)
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
                LoadGrdProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
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
            clearForm();   
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
        private async void grdProduct_ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            string productId = btn.CommandParameter.ToString();

            //MessageBox.Show(productId);

            if (!string.IsNullOrEmpty(productId))
            {
                if (MessageBox.Show("Do you want to delete this item?", "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var result = await _business.DeleteById(int.Parse(productId));
                    MessageBox.Show($"{result.Message}", "Delete");
                    this.LoadGrdProducts();
                }
            }
        }
        private async void grdProduct_MouseDouble_Click (object sender, RoutedEventArgs e)
        {
            DataGrid grd = sender as DataGrid;
            if (grd != null && grd.SelectedItem != null && grd.SelectedItems.Count == 1)
            {
                var row = grd.ItemContainerGenerator.ContainerFromItem(grd.SelectedItem) as DataGridRow;
                if (row != null)
                {
                    var item = row.Item as SiProduct;
                    if (item != null)
                    {
                        var productResult = await _business.GetById(item.ProductId);

                        if (productResult.Status > 0 && productResult.Data != null)
                        {
                            item = productResult.Data as SiProduct;
                            ProductId.Text = Convert.ToString(item.ProductId);
                            CategoryId.Text = Convert.ToString(item.CategoryId);
                            Name.Text = item.Name;
                            Description.Text = item.Description;
                            Barcode.Text = item?.Barcode;
                            Weight.Text = Convert.ToString(item.Weight);
                            CostPrice.Text = Convert.ToString(item.CostPrice);
                            GoldPrice.Text = Convert.ToString(item.GoldPrice);
                            LaborCost.Text = Convert.ToString(item.LaborCost);
                            StoneCost.Text = Convert.ToString(item.StoneCost);
                            SellPriceRatio.Text = Convert.ToString(item.SellPriceRatio);
                        }
                    }
                }
            }
        }
    }
}
