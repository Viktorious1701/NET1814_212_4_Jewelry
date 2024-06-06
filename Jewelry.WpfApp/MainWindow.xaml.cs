using Jewelry.WpfApp.UI;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Jewelry.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Open_jProduct_Click(object sender, RoutedEventArgs e)
        {
            var p = new wProduct();
            p.Owner = this;
            p.Show(); 
        }
        private void Open_jCategory_Click(object sender, RoutedEventArgs e)
        {
             var p = new wCategory();
             p.Owner = this;
             p.Show();
        }
        private void Open_wOrderItem_Click(object sender, RoutedEventArgs e)
        {
            var p = new wOrderItem();
            p.Owner = this;
            p.Show();
        }
        private void Open_wCustomer_Click(object sender, RoutedEventArgs e)
        {
            var p = new wCustomer();
            p.Owner = this;
            p.Show();
        }
        private void Open_wCompany_Click(object sender, RoutedEventArgs e)
        {
            var p = new wCompany();
            p.Owner = this;
            p.Show();
        }
    }

}