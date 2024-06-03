﻿using Jewelry.Business;
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
                        Address = txtAddress.Text
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
                    String newCustomerName = txtName.Text;
                    String newPhone = txtPhone.Text;
                    String newAddress = txtAddress.Text;

                    if (_selectedCustomer.CustomerId == newCustomerId)
                    {
                        _selectedCustomer.Name = newCustomerName;
                        _selectedCustomer.Phone = newPhone;
                        _selectedCustomer.Address = newAddress;
                    }

                    var result = await _business.Update(_selectedCustomer);
                    MessageBox.Show(result.Message, "Update");

                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private async void grdCustomer_ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            try { 
                var result = grdCustomer.SelectedItem as SiCustomer;
                if (result != null)
                {
                await _business.DeleteById(result.CustomerId);
                LoadCustomers();
                }
            }catch (Exception ex)
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
            _selectedCustomer = null;

            ButtonUpdate.Visibility = Visibility.Collapsed;
            ButtonSave.Visibility = Visibility.Visible;
        }
    }
}
