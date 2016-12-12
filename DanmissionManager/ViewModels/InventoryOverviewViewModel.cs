using DanmissionManager.Commands;
using DanmissionManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace DanmissionManager.ViewModels
{

    // Check whether I can get Product-count to display in the view

    public class InventoryOverviewViewModel : BaseViewModel
    {
        public InventoryOverviewViewModel()
        {
            _databaseSearcher = new DatabaseSearcher();
            _currentDate = DateTime.Now;
            _allowedAge = TimeSpan.FromDays(1);

            Products = new ObservableCollection<Product>(FindAllProducts());
            SplitProducts(Products);
            Transactions = new ObservableCollection<Transaction>(FindAllTransactions());
            SoldProducts = new ObservableCollection<SoldProduct>(FindAllSoldProducts());

            this.CommandFindUniqueProducts = new RelayCommand2(() => SortBySearchParameter(1));
            this.CommandFindNonUniqueProducts = new RelayCommand2(() => SortBySearchParameter(2));
            this.CommandFindExpiredProducts = new RelayCommand2(() => SortBySearchParameter(3));
        }
        
        private DatabaseSearcher _databaseSearcher;
        private DateTime _currentDate;
        private TimeSpan _allowedAge;

        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set { _products = value; OnPropertyChanged("Products"); }
        }

        private ObservableCollection<Product> _uniqueProducts;
        public ObservableCollection<Product> UniqueProducts
        {
            get { return _uniqueProducts; }
            set { _uniqueProducts = value; OnPropertyChanged("UniqueProducts"); }
        }

        private ObservableCollection<Product> _nonUniqueProducts;
        public ObservableCollection<Product> NonUniqueProducts
        {
            get { return _nonUniqueProducts; }
            set { _nonUniqueProducts = value; OnPropertyChanged("NonUniqueProducts"); }
        }

        private ObservableCollection<Product> _expiredProducts;
        public ObservableCollection<Product> ExpiredProducts
        {
            get { return _expiredProducts; }
            set { _expiredProducts = value; OnPropertyChanged("ExpiredProducts"); }
        }

        private ObservableCollection<Transaction> _transactions;
        public ObservableCollection<Transaction> Transactions
        {
            get { return _transactions; }
            set { _transactions = value; OnPropertyChanged("Transactions"); }
        }

        private ObservableCollection<SoldProduct> _soldProducts;
        public ObservableCollection<SoldProduct> SoldProducts
        {
            get { return _soldProducts; }
            set { _soldProducts = value; OnPropertyChanged("SoldProducts"); }
        }

        private ObservableCollection<Product> _searchParameter;
        public ObservableCollection<Product> SearchParameter
        {
            get { return _searchParameter; }
            set { _searchParameter = value; OnPropertyChanged("SearchParameter"); }
        }
        // Rename to SortedProducts ?

        public RelayCommand2 CommandFindUniqueProducts { get; private set; }
        public RelayCommand2 CommandFindNonUniqueProducts { get; private set; }
        public RelayCommand2 CommandFindExpiredProducts { get; private set; }

        // Just remove FindTransactions and FindSoldProducts and bind the properties directly to the hidden/shown tables

        private void SortBySearchParameter(int searchParameter)
        {
            switch (searchParameter)
            {
                case 1: SearchParameter = UniqueProducts; break;
                case 2: SearchParameter = NonUniqueProducts; break;
                case 3: SearchParameter = ExpiredProducts; break;
                default: SearchParameter = null;
                    break;
            }
        }

        public List<Product> FindAllProducts()
        {
            return _databaseSearcher.FindProducts(x => true);
        }

        private void SplitProducts(ObservableCollection<Product> productList)
        {
            UniqueProducts = new ObservableCollection<Product>(productList.Where(x => x.isUnique));
            NonUniqueProducts = new ObservableCollection<Product>(productList.Where(x => !x.isUnique));
            ExpiredProducts = new ObservableCollection<Product>(productList.Where(expiredProductsPredicate));
        }

        

        private bool expiredProductsPredicate(Product product)
        {
            return ProductAge(product) > _allowedAge;
        }

        private TimeSpan ProductAge(Product product)
        {
            return _currentDate.Subtract(product.date.Value);
        }

        private List<SoldProduct> FindAllSoldProducts()
        {
            return _databaseSearcher.FindSoldProducts(x => true);
        }

        private List<Transaction> FindAllTransactions()
        {
            return _databaseSearcher.FindTransactions(x => true);
        }

    }
}
