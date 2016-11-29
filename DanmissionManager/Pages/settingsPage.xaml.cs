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
    /// Interaction logic for settingsPage.xaml
    /// </summary>
    public partial class settingsPage : Page
    {
        public settingsPage()
        {
            InitializeComponent();
        }

        private void btn_saveColor_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
            MessageBox.Show("Gemt", "Dine ændringer er blevet gemt.");
        }

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.GUICOLOR = "#FF37BA5D";
        }
    }
}
