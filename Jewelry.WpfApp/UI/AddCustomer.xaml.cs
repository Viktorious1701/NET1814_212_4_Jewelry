using Jewelry.Business;
using Jewelry.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Xps;
using System.Xml.Linq;

namespace Jewelry.WpfApp.UI
{
    /// <summary>
    /// Interaction logic for AddCustomer.xaml
    /// </summary>
    public partial class AddCustomer : Window
    {
        private readonly ICustomerBusiness _business;
        public AddCustomer()
        {
            InitializeComponent();
            _business = new CustomerBusiness();

        }

        private async void grdCustomer_ButtonSave_Click(object sender, RoutedEventArgs e)
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
                    customer.AlterContact = txtAltContact.Text;
                    customer.Country = txtCountry.Text;
                    customer.ZipCode = txtZipCode.Text;

                    var result = await _business.Update(customer);
                    MessageBox.Show(result.Message, "Save");
                }

                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void ClearForm()
        {
            txtCustomerId.Text = string.Empty;
            txtName.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtAddress.Text = string.Empty;
            chkGender.IsChecked = false;
            txtDateOfBirth.SelectedDate = null;
            txtEmail.Text = string.Empty;
            txtAltContact.Text = string.Empty;
            txtCountry.Text = string.Empty;
            txtZipCode.Text = string.Empty;
        }

        private void grdCustomer_ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

    }
}
