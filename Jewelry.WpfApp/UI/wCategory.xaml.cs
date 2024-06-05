using Jewelry.Business;
using Jewelry.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Jewelry.WpfApp.UI
{
    /// <summary>
    /// Interaction logic for wCategory.xaml
    /// </summary>
    public partial class wCategory : Window
    {
        private readonly ICategoryBusiness _business;

        public wCategory()
        {
            InitializeComponent();
            _business = new CategoryBusiness();
            Loaded += OnWindowLoaded;
        }

        private async void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            await LoadGrdCategories();
        }

        private async void grdCategory_ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = await _business.GetById(int.Parse(CategoryId.Text));
                if (item.Data == null)
                {
                    var category = new Category()
                    {
                        CategoryId = int.Parse(CategoryId.Text),
                        CategoryName = CategoryName.Text
                    };

                    var result = await _business.Save(category);
                    MessageBox.Show(result.Message, "Save");
                }
                else
                {
                    var category = item.Data as Category;
                    if (category != null)
                    {
                        category.CategoryName = CategoryName.Text;
                    }

                    var result = await _business.Update(category);
                    MessageBox.Show(result.Message, "Save");
                }

                ClearForm();
                await LoadGrdCategories();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void grdCategory_ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private async Task LoadGrdCategories()
        {
            var result = await _business.GetAll();

            if (result.Status > 0 && result.Data != null)
            {
                grdCategory.ItemsSource = result.Data as List<Category>;
            }
            else
            {
                grdCategory.ItemsSource = new List<Category>();
            }
        }

        private async void grdCategory_ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var categoryId = button?.CommandParameter.ToString();

                if (!string.IsNullOrEmpty(categoryId))
                {
                    if (MessageBox.Show("Do you want to delete this item?", "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        var result = await _business.DeleteById(int.Parse(categoryId));
                        MessageBox.Show(result.Message, "Delete");
                        await LoadGrdCategories();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private async void grdCategory_MouseDouble_Click(object sender, RoutedEventArgs e)
        {
            var grd = sender as DataGrid;
            if (grd != null && grd.SelectedItem != null && grd.SelectedItems.Count == 1)
            {
                var row = grd.ItemContainerGenerator.ContainerFromItem(grd.SelectedItem) as DataGridRow;
                if (row != null)
                {
                    var item = row.Item as Category;
                    if (item != null)
                    {
                        var categoryResult = await _business.GetById(item.CategoryId);

                        if (categoryResult.Status > 0 && categoryResult.Data != null)
                        {
                            item = categoryResult.Data as Category;
                            CategoryId.Text = Convert.ToString(item.CategoryId);
                            CategoryName.Text = item.CategoryName;
                        }
                    }
                }
            }
        }

        private void ButtonEdit_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var item = button?.DataContext as Category;

                if (item != null)
                {
                    CategoryId.Text = Convert.ToString(item.CategoryId);
                    CategoryName.Text = item.CategoryName;

                    LoadGrdCategories();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void ClearForm()
        {
            CategoryId.Text = string.Empty;
            CategoryName.Text = string.Empty;
        }
    }
}
