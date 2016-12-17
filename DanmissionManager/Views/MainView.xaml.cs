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
using System.Xml;
using DanmissionManager.Pages;
using DanmissionManager.ViewModels;

namespace DanmissionManager.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(BaseViewModel.Popups standardPopupService)
        {
            InitializeComponent();

            _standardPopupService = standardPopupService;
            this._Fullscreen = false;

            OpenLogoPage();

            ConsoleManager.Show();
        }

        private BaseViewModel.Popups _standardPopupService;
        private bool _Fullscreen { get; set; }


        private void OpenLogoPage()
        {
            Main.Content = new logoPage();
        }

        private void btn_addProduct_Click(object sender, RoutedEventArgs e)
        {
            var newpage = new addProductPage();
            newpage.DataContext = new AddProductViewModel(_standardPopupService);
            Main.Content = newpage;
        }

        private void btn_findProduct_Click(object sender, RoutedEventArgs e)
        {
            var newpage = new findProductPage();
            newpage.DataContext = new FindProductViewModel(_standardPopupService);
            Main.Content = newpage;
        }

        private void btn_removeProduct_Click(object sender, RoutedEventArgs e)
        {
            var newpage = new checkoutPage();
            newpage.DataContext = new CheckoutViewModel(_standardPopupService);
            Main.Content = newpage;
        }

        private void btn_inventoryOverview_Click(object sender, RoutedEventArgs e)
        {
            var newpage = new inventoryOverviewPage();
            newpage.DataContext = new InventoryOverviewViewModel(_standardPopupService);
            Main.Content = newpage;
        }

        private void btn_categories_Click(object sender, RoutedEventArgs e)
        {
            var newpage = new categoriesPage();
            newpage.DataContext = new CategoriesViewModel(_standardPopupService);
            Main.Content = newpage;
        }

        private void btn_statistics_Click(object sender, RoutedEventArgs e)
        {
            var newpage = new statisticsPage();
            newpage.DataContext = new StatisticsViewModel(_standardPopupService);
            Main.Content = newpage;
        }

        private void btn_settings_Click(object sender, RoutedEventArgs e)
        {
            var newpage = new settingsPage();
            newpage.DataContext = new SettingsViewModel(_standardPopupService);
            Main.Content = newpage;
        }

        //Functionallity for fullscreen-toggle
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F11)
            {
                if (_Fullscreen == false)
                {
                    WindowStyle = WindowStyle.None;
                    WindowState = WindowState.Maximized;
                    ResizeMode = ResizeMode.NoResize;
                }

                if (_Fullscreen == true)
                {
                    WindowStyle = WindowStyle.SingleBorderWindow;
                    WindowState = WindowState.Normal;
                    ResizeMode = ResizeMode.CanResize;
                }

                _Fullscreen = !_Fullscreen;
            }

            if (e.Key == Key.Escape)
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
                WindowState = WindowState.Normal;
                ResizeMode = ResizeMode.CanResize;
            }
        }
    }
}
