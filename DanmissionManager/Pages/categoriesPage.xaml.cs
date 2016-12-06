using DanmissionManager.Views;
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
    /// Interaction logic for categoriesPage.xaml
    /// </summary>
    public partial class categoriesPage : Page
    {
        public categoriesPage()
        {
            InitializeComponent();
        }

        private void button_CategoriesAdd_Click(object sender, RoutedEventArgs e)
        {
            AddCategoryWindow popup = new AddCategoryWindow(this.DataContext);
            popup.ShowDialog();
        }

        private void button_SubcategoriesAdd_Click(object sender, RoutedEventArgs e)
        {
            AddSubCategoryWindow popup = new AddSubCategoryWindow(this.DataContext);
            popup.ShowDialog();
        }
    }
}
