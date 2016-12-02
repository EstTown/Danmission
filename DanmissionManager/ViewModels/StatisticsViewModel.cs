using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanmissionManager.Commands;
using System.IO;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows;

namespace DanmissionManager.ViewModels
{
    class StatisticsViewModel : BaseViewModel
    {
        public StatisticsViewModel()
        {
            /*Combobox*/
            ObservableCollection<string> statistics = new ObservableCollection<string>(statCombobox());
            this.Statistics = statistics;

            RelayCommand2 commandDisplayChart = new RelayCommand2(ChangeChart);
            this.CommandDisplayChart = commandDisplayChart;

            //get all categories and soldproducts
            using(var ctx = new ServerContext())
            {
                this.AllCategories = new List<Category>(ctx.Category.ToList());
                this.AllSoldProducts = new List<SoldProduct>(ctx.Soldproducts.ToList());
            }
        }
        public List<SoldProduct> AllSoldProducts { get; set; }  

        private List<Category> _allCategories;
        public List<Category> AllCategories
        {
            get { return _allCategories; }
            set
            {
                _allCategories = value;
                OnPropertyChanged("Allcategories");
            }
        }

        private void CalculateSum()
        {
            
            for (int i = 0; i < this.AllCategories.Count; i++)
            {
                List<SoldProduct> list = new List<SoldProduct>();
                list = AllSoldProducts.Where(x => x.category.CompareTo(AllCategories[i].id) == 0).ToList();

                AllSoldProducts.Where(x => x.category.CompareTo(AllCategories[i].id) == 0).Sum(x => (int)x.price);

                this.AllCategories[i].Sum = list.Sum(y => (int) y.price);
            }
            
            
            
            /*
                list = AllSoldProducts.Where(x => x.id.CompareTo(category.id) == 0).ToList();
                category.Sum = (int)list.Sum(y => y.price);

                int a = AllSoldProducts.Where(x => x.id.CompareTo(AllCategories[i].id) == 0).Sum(x => (int)x.price);
                */
                //category.Sum = (int)AllSoldProducts.Sum(x => x.price);

            }
            /*
            {
                category.Sum = AllSoldProducts.Where(x => x.id.CompareTo(category.id) == 0).Sum(y => (int)y.price);
                
                
                
                list = AllSoldProducts.Where(x => x.id.CompareTo(category.id) == 0).ToList();
                category.Sum = (int)list.Sum(y => y.price);
                
                category.Sum = (int)AllSoldProducts.Sum(x => x.price);
                Console.WriteLine(category.Sum);
            }
            */
        private string _selectedChart;
        public string SelectedChart
        {
            get { return _selectedChart; }
            set
            {
                _selectedChart = value;
                OnPropertyChanged("SelectedChart");
            }
                
        }
        private void ChangeChart()
        {
            switch (this.SelectedChart)
            {
                case "Inventar":
                    ShowChartItems();
                    break;
                case "Salg":
                    ShowChartSales();
                    break;
                case "Udgået":
                    ShowChartExpired();
                    break;
                default:
                    break;
            }
        }

        private ObservableCollection<string> _statistics;
        public ObservableCollection<string> Statistics
        {
            get { return _statistics; }
            set
            {
                _statistics = value;
                OnPropertyChanged("Statistics");
            }
        }

        private void ShowChartItems()
        {
            CalculateSum();
            
            List<KeyValuePair<string, int>> itemValue = new List<KeyValuePair<string, int>>();
            foreach (Category category in this.AllCategories)
            {
                itemValue.Add(new KeyValuePair<string, int>(category.name, category.Sum));
            }
            /*
            List<KeyValuePair<string, int>> ItemsValue = new List<KeyValuePair<string, int>>();
            ItemsValue.Add(new KeyValuePair<string, int>("Herretøj", 140));
            ItemsValue.Add(new KeyValuePair<string, int>("Dametøj", 360));
            ItemsValue.Add(new KeyValuePair<string, int>("Smykker", 89));
            ItemsValue.Add(new KeyValuePair<string, int>("Køkken", 80));
            ItemsValue.Add(new KeyValuePair<string, int>("Boger", 100));
            */
            PieChart = itemValue;
        }

        private void ShowChartSales()
        {
            List<KeyValuePair<string, int>> SalesValue = new List<KeyValuePair<string, int>>();
            SalesValue.Add(new KeyValuePair<string, int>("Herretøj", 120));
            SalesValue.Add(new KeyValuePair<string, int>("Dametøj", 90));
            SalesValue.Add(new KeyValuePair<string, int>("Smykker", 70));
            SalesValue.Add(new KeyValuePair<string, int>("Køkken", 20));
            SalesValue.Add(new KeyValuePair<string, int>("Boger", 30));

            //PieChart = SalesValue;
        }

        private void ShowChartExpired()
        {
            List<KeyValuePair<string, int>> ExpiredValue = new List<KeyValuePair<string, int>>();
            ExpiredValue.Add(new KeyValuePair<string, int>("Herretøj", 20));
            ExpiredValue.Add(new KeyValuePair<string, int>("Dametøj", 10));
            ExpiredValue.Add(new KeyValuePair<string, int>("Smykker", 30));
            ExpiredValue.Add(new KeyValuePair<string, int>("Køkken", 5));
            ExpiredValue.Add(new KeyValuePair<string, int>("Boger", 20));

            //PieChart = ExpiredValue;
        }

        private List<KeyValuePair<string, int>> _pieChart;
        public List<KeyValuePair<string, int>> PieChart
        {
            get
            {
                return _pieChart;
            }
            set
            {
                _pieChart = value;
                OnPropertyChanged("PieChart");
            }
        }
        public RelayCommand2 CommandDisplayChart { get; set; }

        private List<string> statCombobox()
        {
            List<string> data = new List<string>();

            data.Add("Inventar");
            data.Add("Salg");
            data.Add("Udgået");

            return data;
        }
    }
}
