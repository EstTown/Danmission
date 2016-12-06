﻿using System;
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
        public StatisticsViewModel()
        {
            _databaseSearcher = new DatabaseSearcher();
            Products = new ObservableCollection<Product>(FindAllProducts());
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
                case "Elementer solgt per kategori":
                    ShowChartItems();
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
                if (category.Sum != 0)
                {
                    salesValue.Add(new KeyValuePair<string, int>(category.name, category.Sum));
                }
            }
            PieChart = salesValue;
        }

        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set { _products = value; OnPropertyChanged("Products"); }
        }

        private DatabaseSearcher _databaseSearcher;
        public List<Product> FindAllProducts()
        {
            return _databaseSearcher.FindProducts(x => true);
        }

        private void ShowChartItems()
        {
            CalculateSum();
            List<KeyValuePair<string, int>> itemsValue = new List<KeyValuePair<string, int>>();
            foreach (Category category in this.AllCategories)
            {
                int numberOfProducts = 0;
                if (category.Sum != 0)
                {
                    foreach (Product product in Products)
                    {
                        if (category.id == product.category)
                        {
                            numberOfProducts++;
                        }
                    }
                    itemsValue.Add(new KeyValuePair<string, int>(category.name, numberOfProducts));
                }
            }

            PieChart = itemsValue;
        }

        private void ShowChartInventory()
        {
            List<KeyValuePair<string, int>> inventoryValue = new List<KeyValuePair<string, int>>();
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
            data.Add("Elementer solgt per kategori");

            return data;
        }
    }
}
