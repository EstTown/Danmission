using DanmissionManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace DanmissionManager.ViewModels
{
    public class InventoryOverviewViewModel : BaseViewModel
    {
        public InventoryOverviewViewModel()
        {
            _databaseSearcher = new DatabaseSearcher();
            _currentDate = DateTime.Now;
            _allowedAge = TimeSpan.FromDays(60);

            _productList = FindAllProducts();
            SplitProductList(_productList);
            _transactionList = FindAllTransactions();
            _soldProductList = FindAllSoldProducts();
        }

        private DatabaseSearcher _databaseSearcher;
        private List<Product> _productList;
        private List<Transaction> _transactionList;
        private List<Product> _soldProductList;
        private List<Product> _uniqueProducts;
        private List<Product> _nonUniqueProducts;
        private List<Product> _expiredProducts;
        private DateTime _currentDate;
        private TimeSpan _allowedAge;

        public List<Product> FindAllProducts()
        {
            return _databaseSearcher.SearchProducts(x => true);
        }

        private void SplitProductList(List<Product> productList)
        {
            _uniqueProducts = productList.Where(x => x.isUnique).ToList();
            _nonUniqueProducts = productList.Where(x => !x.isUnique).ToList();
            _expiredProducts = productList.Where(expiredProductsPredicate).ToList();
        }

        private bool expiredProductsPredicate(Product product)
        {
            return ProductAge(product) > _allowedAge;
        }

        private TimeSpan ProductAge(Product product)
        {
            return _currentDate.Subtract(product.date.Value);
        }

        private List<Product> FindAllSoldProducts()
        {
            throw new NotImplementedException();
        }

        private List<Transaction> FindAllTransactions()
        {
            throw new NotImplementedException();
        }

    }
}
