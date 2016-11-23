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

namespace DanmissionManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ConsoleManager.Show();


            //Test - loading the product table to a list, and printing to console
            /*
            using (var ctx = new ServerContext())
            {
                product a = new product();
                a.name = "Tallerken";
                a.price = 22;
                a.isUnique = true;
                a.category = 3;

                ctx.products.Add(a);
                ctx.SaveChanges();

                List<product> b = ctx.products.ToList();
                foreach (product x in b)
                {
                    Console.WriteLine(x.name);
                }
            }
            */


        }
    }
}
