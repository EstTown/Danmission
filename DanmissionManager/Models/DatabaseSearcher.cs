using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DanmissionManager.Models
{
    public class DatabaseSearcher : DatabaseConnector
    {
        public List<Product> FindProducts(Expression<Func<Product, bool>> predicate)
        {
            List<Product> list = new List<Product>();
            using (var ctx = new ServerContext())
            {
                list = ctx.Products.Where(predicate).ToList();
            }
            return list;
        }

        public List<Transaction> FindTransactions(Expression<Func<Transaction, bool>> predicate)
        {
            List<Transaction> list = new List<Transaction>();
            using (var ctx = new ServerContext())
            {
                list = ctx.Transaction.Where(predicate).ToList();
            }
            return list;
        }

        internal List<SoldProduct> FindSoldProducts(Expression<Func<SoldProduct, bool>> predicate)
        {
            List<SoldProduct> list = new List<SoldProduct>();
            using (var ctx = new ServerContext())
            {
                list = ctx.Soldproducts.Where(predicate).ToList();
            }
            return list;
        }
    }
}
