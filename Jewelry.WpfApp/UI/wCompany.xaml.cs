using Jewelry.Business;
using Jewelry.Business.Base;
using Jewelry.Business.Company;
using Jewelry.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security;
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
using System.Xml.Linq;

namespace Jewelry.WpfApp.UI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class wCompany : Window
    {
        private readonly CompanyBusiness _business;
        private SiCompany _selectedCompany;
        public wCompany()
        {
            InitializeComponent();
            _business = new CompanyBusiness();
            LoadCompany();
        }
        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = await _business.GetById(int.Parse(CompanyId.Text));

                if (item.Data == null)
                {
                    var company = new SiCompany()
                    {
                        CompanyId = Convert.ToInt32(CompanyId.Text),
                        CompanyName = CompanyName.Text,

                        CompanyAddress = CompanyAddress.Text
                    };

                    var result = await _business.Save(company);
                    MessageBox.Show(result.Message, "Save");
                }
                else
                {
                    var company = item.Data as SiCompany;
                    //customer.customerCode = txtcustomerCode.Text;
                    company.CompanyName = CompanyName.Text;

                    company.CompanyAddress = CompanyAddress.Text;

                    var result = await _business.Update(company);
                    MessageBox.Show(result.Message, "Save");
                }

                ClearForm();
                LoadCompany();
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

        private async Task LoadCompany()
        {
            var result = await _business.GetAll();

            if (result.Status > 0 && result.Data != null)
            {
                grdCompany.ItemsSource = result.Data as List<SiCompany>;
            }
            else
            {
                grdCompany.ItemsSource = new List<SiCompany>();
            }
        }

        private async void LoadGrdCompany(object sender, SelectionChangedEventArgs e)
        {
            await LoadCompany();
        }
        private async void ButtonSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                _selectedCompany = button?.DataContext as SiCompany;

                if (_selectedCompany != null)
                {
                    CompanyId.Text = _selectedCompany.CompanyId.ToString();
                    CompanyName.Text = _selectedCompany.CompanyName;
                    CompanyAddress.Text = _selectedCompany.CompanyAddress;

                    ButtonUpdate.Visibility = Visibility.Visible;
                    ButtonSave.Visibility = Visibility.Collapsed;
                }
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
                if (_selectedCompany != null)
                {
                    int newCompanyId = int.Parse(CompanyId.Text);
                    String newCompanyName = CompanyName.Text;
                    String newAddress = CompanyAddress.Text;

                    if (_selectedCompany.CompanyId == newCompanyId)
                    {

                        _selectedCompany.CompanyName = newCompanyName;
                        _selectedCompany.CompanyAddress = newAddress;
                    }

                    var result = await _business.Update(_selectedCompany);
                    MessageBox.Show(result.Message, "Update");

                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private async void grdCompany_ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            string CompanyId = btn.CommandParameter.ToString();

            //MessageBox.Show(currencyCode);

            if (!string.IsNullOrEmpty(CompanyId))
            {
                if (MessageBox.Show("Do you want to delete this item?", "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var result = await _business.DeleteById(int.Parse(CompanyId));
                    MessageBox.Show($"{result.Message}", "Delete");
                    LoadCompany();
                }
            }
        }

        private async void grdCompany_MouseDouble_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Double Click on Grid");
            DataGrid grd = sender as DataGrid;
            if (grd != null && grd.SelectedItems != null && grd.SelectedItems.Count == 1)
            {
                var row = grd.ItemContainerGenerator.ContainerFromItem(grd.SelectedItem) as DataGridRow;
                if (row != null)
                {
                    var item = row.Item as SiCompany;
                    if (item != null)
                    {
                        var companyResult = await _business.GetById(item.CompanyId);

                        if (companyResult.Status > 0 && companyResult.Data != null)
                        {
                            item = companyResult.Data as SiCompany;
                            CompanyId.Text = Convert.ToString(item.CompanyId);
                            CompanyName.Text = item.CompanyName;

                            CompanyAddress.Text = item.CompanyAddress;
                        }
                    }
                }
            }
        }

        private void ClearForm()
        {
            CompanyId.Text = string.Empty;
            CompanyName.Text = string.Empty;

            CompanyAddress.Text = string.Empty;
            _selectedCompany = null;

            ButtonUpdate.Visibility = Visibility.Collapsed;
            ButtonSave.Visibility = Visibility.Visible;
        }
    }
}