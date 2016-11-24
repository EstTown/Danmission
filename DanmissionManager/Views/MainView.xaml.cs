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
using DanmissionManager.Pages;

namespace DanmissionManager.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            Main.Content = new logoPage();
        }

        private void btn_addProduct_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new addProductPage();
        }

        private void btn_findProduct_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new findProductPage();
        }

        private void btn_removeProduct_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new removeProductPage();
        }

        private void btn_inventoryOverview_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new inventoryOverviewPage();
        }

        private void btn_categories_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new categoriesPage();
        }

        private void btn_statistics_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new statisticsPage();
        }

        private void btn_settings_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new settingsPage();
        }

    }
}
