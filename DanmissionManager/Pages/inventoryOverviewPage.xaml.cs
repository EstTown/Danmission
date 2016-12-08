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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DanmissionManager.Pages
{
    /// <summary>
    /// Interaction logic for inventoryOverviewPage.xaml
    /// </summary>
    public partial class inventoryOverviewPage : Page
    {

        public inventoryOverviewPage()
        {
            InitializeComponent();
        }

        private void UniqueProducts_Click(object sender, RoutedEventArgs e)
        {
            lvProducts.Visibility = Visibility.Visible;
            TransActions.Visibility = Visibility.Collapsed;
            SoldProducts.Visibility = Visibility.Collapsed;
        }

        private void NotUniqueProducts_Click(object sender, RoutedEventArgs e)
        {
            lvProducts.Visibility = Visibility.Visible;
            TransActions.Visibility = Visibility.Collapsed;
            SoldProducts.Visibility = Visibility.Collapsed;
        }

        private void ExpiredProducts_Click(object sender, RoutedEventArgs e)
        {
            lvProducts.Visibility = Visibility.Visible;
            TransActions.Visibility = Visibility.Collapsed;
            SoldProducts.Visibility = Visibility.Collapsed;
        }

        private void Transactions_Click(object sender, RoutedEventArgs e)
        {
            lvProducts.Visibility = Visibility.Collapsed;
            TransActions.Visibility = Visibility.Visible;
            SoldProducts.Visibility = Visibility.Collapsed;
        }

        private void SoldProducts_Click(object sender, RoutedEventArgs e)
        {
            lvProducts.Visibility = Visibility.Collapsed;
            TransActions.Visibility = Visibility.Collapsed;
            SoldProducts.Visibility = Visibility.Visible;
        }

    }
}
