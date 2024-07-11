using Jewelry.Business;
using Jewelry.Business.Base;
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

namespace Jewelry.WpfApp.UI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class wCustomer : Window
    {
        private readonly CustomerBusiness _business;
        private SiCustomer _selectedCustomer;
        public wCustomer()
        {
            InitializeComponent();
            _business = new CustomerBusiness();
            LoadCustomers();
        }
        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = await _business.GetById(int.Parse(txtCustomerId.Text));

                if (item.Data == null)
                {
                    var customer = new SiCustomer()
                    {
                        CustomerId = Convert.ToInt32(txtCustomerId.Text),
                        Name = txtName.Text,
                        Phone = txtPhone.Text,
                        Address = txtAddress.Text,
                        Gender = chkGender.IsChecked,
                        DateOfBirth = DateTime.TryParse(txtDateOfBirth.Text, out var date) ? date : (DateTime?)null,
                        Email = txtEmail.Text,
                        Job = txtJob.Text,
                        AlterContact = txtAltContact.Text,
                        Country = txtCountry.Text,
                        ZipCode = txtZipCode.Text,

                    };

                    var result = await _business.Save(customer);
                    MessageBox.Show(result.Message, "Save");
                }
                else
                {
                    var customer = item.Data as SiCustomer;
                    //customer.customerCode = txtcustomerCode.Text;
                    customer.Name = txtName.Text;
                    customer.Phone = txtPhone.Text;
                    customer.Address = txtAddress.Text;
                    customer.Gender = chkGender.IsChecked;
                    customer.DateOfBirth = DateTime.TryParse(txtDateOfBirth.Text, out var date) ? date : (DateTime?)null;
                    customer.Email = txtEmail.Text;
                    customer.Job = txtJob.Text;
                    customer.AlterContact = txtAltContact.Text;
                    customer.Country = txtCountry.Text;
                    customer.ZipCode = txtZipCode.Text;

                    var result = await _business.Update(customer);
                    MessageBox.Show(result.Message, "Save");
                }

                ClearForm();
                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();

            if (_selectedCustomer != null)
            {
                ButtonUpdate.Visibility = Visibility.Visible;
                ButtonSave.Visibility = Visibility.Collapsed;
            }
            else
            {
                ButtonUpdate.Visibility = Visibility.Collapsed;
                ButtonSave.Visibility = Visibility.Visible;
            }
        }

        private async Task LoadCustomers()
        {
            var result = await _business.GetAll();

            if (result.Status > 0 && result.Data != null)
            {
                grdCustomer.ItemsSource = result.Data as List<SiCustomer>;
            }
            else
            {
                grdCustomer.ItemsSource = new List<SiCustomer>();
            }
        }

        private async void LoadGrdCustomer(object sender, SelectionChangedEventArgs e)
        {
            await LoadCustomers(); 
        }

        private async void ButtonSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                _selectedCustomer = button?.DataContext as SiCustomer; 

                if(_selectedCustomer != null)
                {
                    txtCustomerId.Text = _selectedCustomer.CustomerId.ToString();
                    txtName.Text = _selectedCustomer.Name;
                    txtPhone.Text = _selectedCustomer.Phone;
                    txtAddress.Text = _selectedCustomer.Address;
                    chkGender.IsChecked = _selectedCustomer.Gender;
                    txtDateOfBirth.Text = _selectedCustomer.DateOfBirth?.ToString("yyyy-MM-dd");
                    txtCountry.Text = _selectedCustomer.Country;
                    txtAltContact.Text = _selectedCustomer.AlterContact;
                    txtEmail.Text = _selectedCustomer.Email;
                    txtZipCode.Text = _selectedCustomer.ZipCode;
                    txtJob.Text = _selectedCustomer.Job;

                    ButtonUpdate.Visibility = Visibility.Visible;
                    ButtonSave.Visibility = Visibility.Collapsed;
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private async void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedCustomer != null)
                {
                    int newCustomerId = int.Parse(txtCustomerId.Text);
                    string newCustomerName = txtName.Text;
                    string newPhone = txtPhone.Text;
                    string newAddress = txtAddress.Text;
                    bool? newGender = chkGender.IsChecked;
                    DateTime? newDateOfBirth = DateTime.TryParse(txtDateOfBirth.Text, out var date) ? date : (DateTime?)null;
                    string newCountry = txtCountry.Text;
                    string newJob = txtJob.Text;
                    string newAltContact = txtAltContact.Text;
                    string newEmail = txtEmail.Text;
                    string newZipCode = txtZipCode.Text;

                    if (_selectedCustomer.CustomerId == newCustomerId)
                    {
                        _selectedCustomer.Name = newCustomerName;
                        _selectedCustomer.Phone = newPhone;
                        _selectedCustomer.Address = newAddress;
                        _selectedCustomer.Gender = newGender;
                        _selectedCustomer.DateOfBirth = newDateOfBirth;
                        _selectedCustomer.Country = newCountry;
                        _selectedCustomer.Job = newJob;
                        _selectedCustomer.AlterContact = newAltContact;
                        _selectedCustomer.Email = newEmail;
                        _selectedCustomer.ZipCode = newZipCode;
                    }

                    var result = await _business.Update(_selectedCustomer);
                    MessageBox.Show(result.Message, "Update");

                    ClearForm();
                    LoadCustomers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private async void grdCustomer_ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            string txtCustomerId = btn.CommandParameter.ToString();

            //MessageBox.Show(currencyCode);

            if (!string.IsNullOrEmpty(txtCustomerId))
            {
                if (MessageBox.Show("Do you want to delete this item?", "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var result = await _business.DeleteById(int.Parse(txtCustomerId));
                    MessageBox.Show($"{result.Message}", "Delete");
                    LoadCustomers();
                }
            }
        }

        private async void grdCustomer_MouseDouble_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Double Click on Grid");
            DataGrid grd = sender as DataGrid;
            if (grd != null && grd.SelectedItems != null && grd.SelectedItems.Count == 1)
            {
                var row = grd.ItemContainerGenerator.ContainerFromItem(grd.SelectedItem) as DataGridRow;
                if (row != null)
                {
                    var item = row.Item as SiCustomer;
                    if (item != null)
                    {
                        var customerResult = await _business.GetById(item.CustomerId);

                        if (customerResult.Status > 0 && customerResult.Data != null)
                        {
                            item = customerResult.Data as SiCustomer;
                            txtCustomerId.Text = Convert.ToString(item.CustomerId);
                            txtName.Text = item.Name;
                            txtPhone.Text = item.Phone;
                            txtAddress.Text = item.Address;
                            chkGender.IsChecked = item.Gender;
                            txtDateOfBirth.Text = item.DateOfBirth?.ToString("yyyy-MM-dd");
                            txtCountry.Text = item.Country;
                            txtAltContact.Text = item.AlterContact;
                            txtEmail.Text = item.Email;
                            txtJob.Text = item.Job;
                            txtZipCode.Text = item.ZipCode;
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
                _selectedCustomer = button?.DataContext as SiCustomer;
                if(_selectedCustomer != null)
                {
                    txtCustomerId.Text = _selectedCustomer.CustomerId.ToString();
                    txtName.Text = _selectedCustomer.Name;
                    txtPhone.Text = _selectedCustomer.Phone;
                    txtAddress.Text = _selectedCustomer.Address;
                    chkGender.IsChecked = _selectedCustomer.Gender;
                    txtDateOfBirth.SelectedDate = _selectedCustomer.DateOfBirth; // Fixed binding
                    txtCountry.Text= _selectedCustomer.Country;
                    txtAltContact.Text= _selectedCustomer.AlterContact;
                    txtEmail.Text= _selectedCustomer.Email;
                    txtJob.Text= _selectedCustomer.Job;
                    txtZipCode.Text= _selectedCustomer.ZipCode;


                    txtCustomerId.IsReadOnly = true;
                    txtName.IsReadOnly = true;
                    txtPhone.IsReadOnly = true;
                    txtAddress.IsReadOnly = true;
                    chkGender.IsEnabled = false;
                    txtDateOfBirth.IsEnabled  = false;
                    txtCountry.IsReadOnly = true;
                    txtAltContact.IsReadOnly = true;
                    txtEmail.IsReadOnly = true;
                    txtJob.IsReadOnly = true;
                    txtZipCode.IsReadOnly = true;


                    ButtonUpdate.Visibility = Visibility.Collapsed;
                    ButtonSave.Visibility = Visibility.Collapsed;

                }
            }catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private async void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = await _business.GetAll();
                var list = result.Data as IEnumerable<SiCustomer>;

                if(list == null || !list.Any())
                {
                    MessageBox.Show("No Data Found.");
                    return;
                }
                var filterList = list.Where(customer =>
                {
                    bool isMatch = true;
                    if(!string.IsNullOrWhiteSpace(txtCustomerId.Text))
                    {
                        isMatch &= customer.CustomerId == int.Parse(txtCustomerId.Text);
                    }
                    if (string.IsNullOrWhiteSpace(txtName.Text))
                    {
                        isMatch &= customer.Name.Contains(txtName.Text, StringComparison.OrdinalIgnoreCase);
                    }
                    if (string.IsNullOrWhiteSpace(txtPhone.Text))
                    {
                        isMatch &= customer.Phone.Contains(txtPhone.Text, StringComparison.OrdinalIgnoreCase);
                    }
                    if (string.IsNullOrWhiteSpace(txtEmail.Text))
                    {
                        isMatch &= customer.Email.Contains(txtEmail.Text, StringComparison.OrdinalIgnoreCase);
                    }
                    if (string.IsNullOrWhiteSpace(txtAddress.Text))
                    {
                        isMatch &= customer.Address.Contains(txtAddress.Text, StringComparison.OrdinalIgnoreCase);
                    }
                    if (string.IsNullOrWhiteSpace(txtCountry.Text))
                    {
                        isMatch &= customer.Country.Contains(txtCountry.Text, StringComparison.OrdinalIgnoreCase);
                    }
                    if (string.IsNullOrWhiteSpace(txtJob.Text))
                    {
                        isMatch &= customer.Job.Contains(txtJob.Text, StringComparison.OrdinalIgnoreCase);
                    }
                    if (string.IsNullOrWhiteSpace(txtDateOfBirth.Text))
                    {
                        DateTime dateOfBirth;
                        if (DateTime.TryParse(txtDateOfBirth.Text, out dateOfBirth))
                        {
                            isMatch &= customer.DateOfBirth.HasValue && customer.DateOfBirth.Value.Date == dateOfBirth.Date;
                        }
                    }
                    if(chkGender.IsChecked == true)
                    {
                        isMatch &= customer.Gender == true;
                    }
                    if(chkGender.IsChecked != true)
                    {
                        isMatch &= customer.Gender == false;
                    }
                    if(string.IsNullOrWhiteSpace(txtAltContact.Text))
                    {
                        isMatch &= customer.AlterContact.Contains(txtAltContact.Text, StringComparison.OrdinalIgnoreCase);
                    }
                    if(string.IsNullOrWhiteSpace(txtZipCode.Text))
                    {
                        isMatch &= customer.ZipCode.Contains(txtZipCode.Text, StringComparison.OrdinalIgnoreCase);
                    }
                    return isMatch;
                }).ToList();
                if (!filterList.Any())
                {
                    MessageBox.Show("No Data Found");
                    return;
                }
                DisplaySearchResults(filterList);
                ClearForm();

                ButtonUpdate.Visibility = Visibility.Collapsed;
                ButtonSave.Visibility = Visibility.Collapsed;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void DisplaySearchResults(IEnumerable<SiCustomer> customers)
        {
            // Set the ItemsSource to null first to allow modification
            grdCustomer.ItemsSource = null;

            // Set the ItemsSource to the filtered list
            grdCustomer.ItemsSource = customers;
        }

        private void ClearForm()
        {
            txtCustomerId.Text = string.Empty;
            txtName.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtAddress.Text = string.Empty;
            chkGender.IsEnabled = false;
            txtDateOfBirth.SelectedDate = null;
            txtCountry.Text = string.Empty;
            txtAltContact.Text =string.Empty;
            txtEmail.Text = string.Empty;
            txtJob.Text = string.Empty;
            txtZipCode.Text = string.Empty;
            _selectedCustomer = null;

            txtCustomerId.IsReadOnly = false;
            txtName.IsReadOnly = false;
            txtPhone.IsReadOnly = false;
            txtAddress.IsReadOnly = false;
            chkGender.IsEnabled = true;
            txtDateOfBirth.IsEnabled = true;
            txtCountry.IsReadOnly = false;
            txtEmail.IsReadOnly = false;
            txtJob.IsReadOnly = false;
            txtZipCode.IsReadOnly = false;
            txtEmail.IsReadOnly=false;

            ButtonUpdate.Visibility = Visibility.Collapsed;
            ButtonSave.Visibility = Visibility.Visible;
        }

    }
}
