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
using System.Windows.Shapes;

namespace Jewelry.WpfApp.UI
{
    /// <summary>
    /// Interaction logic for ViewCustomer.xaml
    /// </summary>
    public partial class ViewCustomer : Window
    {
        private readonly CustomerBusiness _customer;
        public ViewCustomer(int customerId)
        {
            InitializeComponent();
            _customer = new CustomerBusiness();
            LoadCustomerData(customerId);
        }

        private async void LoadCustomerData(int customerId)
        {
            var item = await _customer.GetById(customerId);
            if(item?.Data is SiCustomer customer)
            {
                txtCustomerID.Text = customer.CustomerId.ToString();
                txtName.Text = customer.Name.ToString();
                txtPhone.Text = customer.Phone.ToString(); 
                txtAddress.Text = customer.Address.ToString();
                chkGender.Text = customer.Gender.ToString();    
                txtDateOfBirth.Text = customer.DateOfBirth?.ToString("MM/dd/yyyy") ?? "N/A";
                txtEmail.Text = customer.Email.ToString();
                txtCountry.Text = customer.Country.ToString();
                txtAltContact.Text = customer.AlterContact.ToString();
                txtZipCode.Text = customer.ZipCode.ToString();
            }
            else
            {
                MessageBox.Show("Customer not found.", "Error");
                this.Close();

            }
        }
    }
}
