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
        public InventoryOverviewViewModel(Popups popupService) : base(popupService)
        {
            _databaseSearcher = new DatabaseSearcher();
            _currentDate = DateTime.Now;
            _allowedAge = TimeSpan.FromDays(Properties.Settings.Default.DAYSTOEXPIRATION);

            Products = new ObservableCollection<Product>(FindAllProducts());
            SplitProducts(Products);
            Transactions = new ObservableCollection<Transaction>(FindAllTransactions());
            SoldProducts = new ObservableCollection<SoldProduct>(FindAllSoldProducts());

            this.CommandFindUniqueProducts = new RelayCommand2(() => SortBySearchParameter(1));
            this.CommandFindNonUniqueProducts = new RelayCommand2(() => SortBySearchParameter(2));
            this.CommandFindExpiredProducts = new RelayCommand2(() => SortBySearchParameter(3));
            this.CommandRemoveExpiredProduct = new RelayCommand2(CommandRemoveSelectedExpiredProduct);
        }

        private DatabaseSearcher _databaseSearcher;
        private DateTime _currentDate;
        private TimeSpan _allowedAge;

        //===========================================================================//
        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                OnPropertyChanged("SelectedProduct");
            }
        }

        public RelayCommand2 CommandRemoveExpiredProduct { get; set; }
        public void CommandRemoveSelectedExpiredProduct()
        {
            try
            {
                using (var ctx = new ServerContext())
                {
                    if (this.SelectedProduct != null)
                    {
                        List<Product> productlist = ctx.Products.Where(x => x.id.CompareTo(SelectedProduct.id) == 0).ToList();
                        Product product = productlist.First();
                        ctx.Products.Remove(product);
                        ctx.SaveChanges();
                        PopupService.PopupMessage("Produkt er blevet fjernet fra systemet", "Fjern produkt");
                    }
                    else
                    {
                        PopupService.PopupMessage("Intet produkt er markeret", "Fjern produkt");
                    }
                }
            }
            catch (System.Data.DataException)
            {
                PopupService.PopupMessage(Application.Current.FindResource("CouldNotConnectToDatabase").ToString(), Application.Current.FindResource("Error").ToString());
            }
            //also remove product from current observablecollection
            this.ExpiredProducts.Remove(SelectedProduct);
        }

        //===========================================================================//

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

        public RelayCommand2 CommandFindUniqueProducts { get; private set; }
        public RelayCommand2 CommandFindNonUniqueProducts { get; private set; }
        public RelayCommand2 CommandFindExpiredProducts { get; private set; }

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
            var products = new List<Product>();
            try
            {
                products = _databaseSearcher.FindProducts(x => true);
            }
            catch (System.Data.DataException)
            {
                PopupService.PopupMessage(Application.Current.FindResource("CouldNotConnectToDatabase").ToString(), Application.Current.FindResource("Error").ToString());
            }
            return products;
        }

        private void SplitProducts(ObservableCollection<Product> productList)
        {
            UniqueProducts = new ObservableCollection<Product>(productList.Where(x => x.isUnique));
            NonUniqueProducts = new ObservableCollection<Product>(productList.Where(x => !x.isUnique));
            ExpiredProducts = new ObservableCollection<Product>(productList.Where(expiredProductsPredicate));
        }

        private bool expiredProductsPredicate(Product product)
        {
            return ProductAge(product) > _allowedAge && product.expiredate != null;
        }

        private TimeSpan ProductAge(Product product)
        {
            return _currentDate.Subtract(product.date.Value);
        }

        private List<SoldProduct> FindAllSoldProducts()
        {
            var soldProducts = new List<SoldProduct>();
            try
            {
                soldProducts = _databaseSearcher.FindSoldProducts(x => true);
            }
            catch (System.Data.DataException)
            {
                PopupService.PopupMessage(Application.Current.FindResource("CouldNotConnectToDatabase").ToString(), Application.Current.FindResource("Error").ToString());
            }
            return soldProducts;
        }

        private List<Transaction> FindAllTransactions()
        {
            var transactions = new List<Transaction>();
            try
            {
                transactions = _databaseSearcher.FindTransactions(x => true);
            }
            catch (System.Data.DataException)
            {
                PopupService.PopupMessage(Application.Current.FindResource("CouldNotConnectToDatabase").ToString(), Application.Current.FindResource("Error").ToString());
            }
            return transactions;
        }
    }
}
