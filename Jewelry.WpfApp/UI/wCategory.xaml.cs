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
    /// Interaction logic for wCategory.xaml
    /// </summary>
    public partial class wCategory : Window
    {
        private readonly ICategoryBusiness _business;
        private Category _selectedCategory;

        public wCategory()
        {
            InitializeComponent();
            _business = new CategoryBusiness();
            LoadGrdCategories();
        }

        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var category = new Category()
                {
                    CategoryId = int.Parse(CategoryId.Text),
                    CategoryName = CategoryName.Text
                };

                var result = await _business.Save(category);
                MessageBox.Show(result.Message, "Save");

                ClearForm();
                LoadGrdCategories();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private async void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedCategory != null)
                {
                    int newCategoryId = int.Parse(CategoryId.Text);
                    string newCategoryName = CategoryName.Text;

              
                        _selectedCategory.CategoryId = newCategoryId;
                        _selectedCategory.CategoryName = newCategoryName;
                    

                    var result = await _business.Update(_selectedCategory);
                    MessageBox.Show(result.Message, "Update");

                    ClearForm();
                    LoadGrdCategories();
                }else
                {
                   throw new Exception(e.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }


        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
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

        private async void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = grdCategory.SelectedItem as Category;
                if (result != null && result.SiProducts == null)
                {
                    await _business.DeleteById(result.CategoryId);
                    LoadGrdCategories();
                }
                else
                {
                    throw new Exception("Cannot delete the category because it is associated with products.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void ButtonSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                _selectedCategory = button?.DataContext as Category;

                if (_selectedCategory != null)
                {
                    CategoryId.Text = _selectedCategory.CategoryId.ToString();
                    CategoryName.Text = _selectedCategory.CategoryName;

                    ButtonUpdate.Visibility = Visibility.Visible;
                    ButtonSave.Visibility = Visibility.Collapsed;
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
            _selectedCategory = null;

            ButtonUpdate.Visibility = Visibility.Collapsed;
            ButtonSave.Visibility = Visibility.Visible;
        }
    }
}
