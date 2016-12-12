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
using DanmissionManager.Models;

namespace DanmissionManager.ViewModels
{
    class StatisticsViewModel : BaseViewModel
    {
        public StatisticsViewModel(Popups popupService) : base(popupService)
        {
            /*Combobox*/
            ObservableCollection<string> statistics = new ObservableCollection<string>(statCombobox());
            this.Statistics = statistics;

            RelayCommand2 commandDisplayChart = new RelayCommand2(ChangeChart);
            this.CommandDisplayChart = commandDisplayChart;


            TimeSpan timespan = new TimeSpan(30, 0, 0, 0);

            this.dateFrom = DateTime.Now - timespan;
            this.dateTo = DateTime.Now;

            //get all categories and soldproducts
            using(var ctx = new ServerContext())
            {
                this.AllCategories = new List<Category>(ctx.Category.ToList());
                this.AllSoldProducts = new List<SoldProduct>(ctx.Soldproducts.ToList());
                this.AllProducts = new List<Product>(ctx.Products.ToList());
            }
        }

        public List<SoldProduct> AllSoldProducts { get; set; }
        public List<Product> AllProducts { get; set; }

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

                AllSoldProducts.Where(x => x.category.CompareTo(AllCategories[i].id) == 0).Sum(x => (int) x.price);

                this.AllCategories[i].Sum = list.Sum(y => (int) y.price);
            }
        }
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
                case "Solgt for per kategori":
                    ShowChartSales();
                    break;
                case "Produkter solgt per kategori":
                    ShowChartItems();
                    break;
                case "Produkter per kategori":
                    ShowChartInventory();
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
        private void ShowChartSales()
        {
            CalculateSum();
            List<KeyValuePair<string, int>> salesValue = new List<KeyValuePair<string, int>>();
            foreach (Category category in this.AllCategories)
            {
                int amountPerCategory = 0;
                foreach(SoldProduct product in AllSoldProducts)
                {
                    if(product.date >= dateFrom && product.date <= dateTo && category.id == product.category)
                    {
                        amountPerCategory += (int)product.price;
                    }
                }
                if (amountPerCategory != 0)
                {
                    salesValue.Add(new KeyValuePair<string, int>(category.name, amountPerCategory));
                }
            }
            PieChart = salesValue;
        }

        private void ShowChartItems()
        {
            CalculateSum();
            List<KeyValuePair<string, int>> itemsValue = new List<KeyValuePair<string, int>>();
            foreach (Category category in this.AllCategories)
            {
                int numberOfProducts = 0;
                foreach (SoldProduct product in AllSoldProducts)
                {
                    if (category.id == product.category && product.date >= dateFrom && product.date <= dateTo)
                    {
                        numberOfProducts++;
                    }
                }
                if (numberOfProducts != 0)
                {
                    itemsValue.Add(new KeyValuePair<string, int>(category.name, numberOfProducts));
                }
            }

            PieChart = itemsValue;
        }

        private void ShowChartInventory()
        {
            List<KeyValuePair<string, int>> inventoryValue = new List<KeyValuePair<string, int>>();
            foreach (Category category in AllCategories)
            {
                int numberOfProducts = 0;
                foreach (Product product in AllProducts)
                {
                    if(category.id == product.category /*&& product.date >= dateFrom && product.date <= dateTo*/)
                    {
                        numberOfProducts++;
                    }
                }
                if (numberOfProducts != 0)
                {
                    inventoryValue.Add(new KeyValuePair<string, int>(category.name, numberOfProducts));
                }
            }

            PieChart = inventoryValue;
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

            data.Add("Solgt for per kategori");
            data.Add("Produkter solgt per kategori");
            data.Add("Produkter per kategori");

            return data;
        }

        private DateTime _dateFrom;
        public DateTime dateFrom
        {
            get
            {
                return _dateFrom;
            }
            set
            {
                _dateFrom = value;
                OnPropertyChanged("dateFrom");
            }
        }

        private DateTime _dateTo;
        public DateTime dateTo
        {
            get
            {
                return _dateTo;
            }
            set
            {
                _dateTo = value;
                OnPropertyChanged("dateTo");
            }
        }
    }
}
