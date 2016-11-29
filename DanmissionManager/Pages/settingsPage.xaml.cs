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
using System.Configuration;

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

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();

            Console.WriteLine("before: " + ConfigurationManager.ConnectionStrings["ServerContext"].ConnectionString);
            //Save new connection data


            string updatedConnection = "server=" + Properties.Settings.Default.IP1 + "." + Properties.Settings.Default.IP2 + "." + Properties.Settings.Default.IP3 + "." + Properties.Settings.Default.IP4+";user id=" + Properties.Settings.Default.USER + ";password=" + Properties.Settings.Default.PASSWORD + ";persistsecurityinfo=True;database ="+ Properties.Settings.Default.SCHEMA;

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            connectionStringsSection.ConnectionStrings["ServerContext"].ConnectionString = updatedConnection;
            config.Save();
            ConfigurationManager.RefreshSection("connectionStrings");

            Console.WriteLine("after: " + ConfigurationManager.ConnectionStrings["ServerContext"].ConnectionString);

            //Console.WriteLine(updatedConnection);
            MessageBox.Show("Dine ændringer er blevet gemt.", "Gemt!");
        }

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.GUICOLOR = "#FF37BA5D";
        }

    }
}
