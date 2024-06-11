using System.Windows;

namespace Jewelry.WpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Open_jProduct_Click(object sender, RoutedEventArgs e)
        {
            var p = new Jewelry.WpfApp.UI.wProduct();
            p.Owner = this;
            p.Show();
        }

        private void Open_jCategory_Click(object sender, RoutedEventArgs e)
        {
            var p = new Jewelry.WpfApp.UI.wCategory();
            p.Owner = this;
            p.Show();
        }

        private void Open_wOrderItem_Click(object sender, RoutedEventArgs e)
        {
            var p = new Jewelry.WpfApp.UI.wOrderItem();
            p.Owner = this;
            p.Show();
        }

        private void Open_wCustomer_Click(object sender, RoutedEventArgs e)
        {
            var p = new Jewelry.WpfApp.UI.wCustomer();
            p.Owner = this;
            p.Show();
        }

        private void Open_wCompany_Click(object sender, RoutedEventArgs e)
        {
            var p = new Jewelry.WpfApp.UI.wCompany();
            p.Owner = this;
            p.Show();
        }

        private void Open_wOrder_Click(object sender, RoutedEventArgs e)
        {
            var p = new Jewelry.WpfApp.UI.wOrder();
            p.Owner = this;
            p.Show();
        }

    }
}
