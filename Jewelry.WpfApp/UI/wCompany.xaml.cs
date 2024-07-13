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
using System.Windows.Media.Media3D;
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

                        CompanyAddress = CompanyAddress.Text,
                        EmailAddress= EmailAddress.Text,
                        Hotline= Hotline.Text,
                        ZipCode= ZipCode.Text,
                        NoOfYearsInBusiness= Convert.ToInt32(NoOfYearsInBusiness),
                        Policy= Policy.Text,
                        EmployeeNum= Convert.ToInt32(EmployeeNum.Text),
                        ContactPerson= ContactPerson.Text,
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
                    company.EmailAddress = EmailAddress.Text;
                    company.Hotline = Hotline.Text;
                    company.ZipCode = ZipCode.Text;
                    company.NoOfYearsInBusiness = Convert.ToInt32(NoOfYearsInBusiness.Text);
                    company.Policy = Policy.Text;
                    company.EmployeeNum = Convert.ToInt32(EmployeeNum.Text);
                    company.ContactPerson = ContactPerson.Text;
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
                    EmailAddress.Text = _selectedCompany.EmailAddress;
                    Hotline.Text = _selectedCompany.Hotline;
                    ZipCode.Text = _selectedCompany.ZipCode;
                    NoOfYearsInBusiness.Text= _selectedCompany.NoOfYearsInBusiness.ToString();
                    Policy.Text= _selectedCompany.Policy;
                    EmployeeNum.Text= _selectedCompany.EmployeeNum.ToString();
                    ContactPerson.Text = _selectedCompany.ContactPerson;
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
                    String newEmail= EmailAddress.Text;
                    String newHotline= Hotline.Text;
                    String newZipCode= ZipCode.Text;
                    int newNoOfYears= int.Parse(NoOfYearsInBusiness.Text);
                    String newPolicy= Policy.Text;
                    int newEmployeeNum = int.Parse(EmployeeNum.Text);
                    String newContactPerson= ContactPerson.Text;
                    if (_selectedCompany.CompanyId == newCompanyId)
                    {

                        _selectedCompany.CompanyName = newCompanyName;
                        _selectedCompany.CompanyAddress = newAddress;
                        _selectedCompany.EmailAddress = newEmail;
                        _selectedCompany.Hotline = newHotline;
                        _selectedCompany.ZipCode= newZipCode;
                        _selectedCompany.NoOfYearsInBusiness = newNoOfYears;
                        _selectedCompany.Policy = newPolicy;
                        _selectedCompany.EmployeeNum = newEmployeeNum;
                        _selectedCompany.ContactPerson = newContactPerson;
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
                            EmailAddress.Text = item.EmailAddress;
                            Hotline.Text = item.Hotline;
                            ZipCode.Text = item.ZipCode;
                            NoOfYearsInBusiness.Text= Convert.ToString(item.NoOfYearsInBusiness);
                            Policy.Text= item.Policy;
                            EmployeeNum.Text = Convert.ToString(item.EmployeeNum);
                            ContactPerson.Text = item.ContactPerson;
                        }
                    }
                }
            }
        }
        private async void ButtonView_Click(object sender, RoutedEventArgs e)
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
                    EmailAddress.Text = _selectedCompany.EmailAddress;
                    Hotline.Text = _selectedCompany.Hotline;
                    ZipCode.Text = _selectedCompany.ZipCode;
                    NoOfYearsInBusiness.Text = _selectedCompany.NoOfYearsInBusiness.ToString();
                    Policy.Text = _selectedCompany.Policy;
                    EmployeeNum.Text = _selectedCompany.EmployeeNum.ToString();
                    ContactPerson.Text = _selectedCompany.ContactPerson;
                    CompanyId.IsReadOnly = true;
                    CompanyName.IsReadOnly = true;
                    
                    CompanyAddress.IsReadOnly = true;
                    EmailAddress.IsReadOnly = true;
                    Hotline.IsReadOnly = true;
                    ZipCode.IsReadOnly = true;
                    NoOfYearsInBusiness.IsReadOnly = true;
                    Policy.IsReadOnly = true;
                    EmployeeNum.IsReadOnly = true;
                    ContactPerson.IsReadOnly = true;


                    ButtonUpdate.Visibility = Visibility.Collapsed;
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
            CompanyId.Text = string.Empty;
            CompanyName.Text = string.Empty;

            CompanyAddress.Text = string.Empty;
            EmailAddress.Text = string.Empty;
            Hotline.Text = string.Empty;
            ZipCode.Text = string.Empty;
            NoOfYearsInBusiness.Text= string.Empty;
            Policy.Text = string.Empty;
            EmployeeNum.Text = string.Empty;
            ContactPerson.Text = string.Empty;

            _selectedCompany = null;

            ButtonUpdate.Visibility = Visibility.Collapsed;
            ButtonSave.Visibility = Visibility.Visible;
        }

        private async void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Fetch all products
                var result = await _business.GetAll();

                // Assuming result.Data contains the list of products
                var list = result.Data as IEnumerable<SiCompany>;

                // Check if the list is null or empty
                if (list == null || !list.Any())
                {
                    MessageBox.Show("No Data Found");
                    return;
                }

                // Filter the list based on the provided input fields
                var filterList = list.Where(company =>
                {
                    bool isMatch = true;

                    if (!string.IsNullOrWhiteSpace(CompanyId.Text))
                    {
                        isMatch &= company.CompanyId == int.Parse(CompanyId.Text);
                    }
                    if (!string.IsNullOrWhiteSpace(CompanyName.Text))
                    {
                        isMatch &= company.CompanyName.Contains(CompanyName.Text, StringComparison.OrdinalIgnoreCase);
                    }
                    if (!string.IsNullOrWhiteSpace(CompanyAddress.Text))
                    {
                        isMatch &= company.CompanyAddress.Contains(CompanyAddress.Text, StringComparison.OrdinalIgnoreCase);
                    }
                    if (!string.IsNullOrWhiteSpace(EmailAddress.Text))
                    {
                        isMatch &= company.EmailAddress.Contains(EmailAddress.Text, StringComparison.OrdinalIgnoreCase);
                    }
                    if (!string.IsNullOrWhiteSpace(Hotline.Text))
                    {
                        isMatch &= company.Hotline.Contains(Hotline.Text, StringComparison.OrdinalIgnoreCase);
                    }
                    if (!string.IsNullOrWhiteSpace(ZipCode.Text))
                    {
                        isMatch &= company.ZipCode.Contains(ZipCode.Text, StringComparison.OrdinalIgnoreCase);
                    }
                    if (!string.IsNullOrWhiteSpace(NoOfYearsInBusiness.Text))
                    {
                        isMatch &= company.NoOfYearsInBusiness == int.Parse(NoOfYearsInBusiness.Text);
                    }
                    if (!string.IsNullOrWhiteSpace(Policy.Text))
                    {
                        isMatch &= company.Policy.Contains(Policy.Text, StringComparison.OrdinalIgnoreCase);
                    }
                    if (!string.IsNullOrWhiteSpace(EmployeeNum.Text))
                    {
                        isMatch &= company.EmployeeNum == int.Parse(EmployeeNum.Text);
                    }
                    if (!string.IsNullOrWhiteSpace(ContactPerson.Text))
                    {
                        isMatch &= company.ContactPerson.Contains(ContactPerson.Text, StringComparison.OrdinalIgnoreCase);
                    }
                   

                    return isMatch;
                }).ToList();

                // Check if there are any results
                if (!filterList.Any())
                {
                    MessageBox.Show("No Data Found");
                    return;
                }

                // Display the filtered results
                DisplaySearchResults(filterList);
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        // Sample method to display search results in a grid (you need to implement this)
        private void DisplaySearchResults(IEnumerable<SiCompany> companies)
        {
            // Set the ItemsSource to null first to allow modification
            grdCompany.ItemsSource = null;

            // Set the ItemsSource to the filtered list
            grdCompany.ItemsSource = companies;
        }

    }
}