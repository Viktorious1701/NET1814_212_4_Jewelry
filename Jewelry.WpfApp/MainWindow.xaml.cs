using Jewelry.WpfApp.UI;
using System.Net;
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
using System.Xml.Linq;

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
        private void Open_wCustomer_Click(object sender, RoutedEventArgs e)
        {
            var c = new wCustomer();
            c.Owner = this;
            c.Show();
        }

        //private void Open_wName_Click(object sender, RoutedEventArgs e)
        //{
        //    var p = new wName();
        //    p.Owner = this;
        //    p.Show();
        //}

        //private void Open_wPhone_Click(object sender, RoutedEventArgs e)
        //{
        //    var p = new wPhone();
        //    p.Owner = this;
        //    p.Show();
        //}

        //private void Open_wAddress_Click(object sender, RoutedEventArgs e)
        //{
        //    var p = new wAddress();
        //    p.Owner = this;
        //    p.Show();
        //}

   
    }
}