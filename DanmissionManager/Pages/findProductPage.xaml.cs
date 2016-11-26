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
using System.Windows.Forms;

namespace DanmissionManager.Pages
{
    /// <summary>
    /// Interaction logic for findProductPage.xaml
    /// </summary>
    public partial class findProductPage : Page
    {
        public findProductPage()
        {
            InitializeComponent();
        }

        //When 'Enter' is pressed in searchbox, the "search" command is executed
        private void txtbox_seach_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                btn_search.Command.Execute(null);
            }
        }
    }
}
