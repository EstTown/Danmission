using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using DanmissionManager.Simulator.Interfaces;

namespace DanmissionManager.Simulator
{
    class TransactionGenerator : ITransactionGenerator
    {
        public TransactionGenerator(int numberoftransaction, int days)
        {
            this.Days = days;
            this.NumberOfTransactions = numberoftransaction;
            using (var ctx = new ServerContext())
            {
                this.AllProducts = ctx.Products.ToList();
            }
        }
        
        private List<Product> AllProducts { get; set; }
        private int NumberOfTransactions;
        private int Days;

        public void GenerateTransactions()
        {
            List<Transaction> listOfTransactions = new List<Transaction>();

            for (int i = 0; i < this.NumberOfTransactions; i++)
            {
                Transaction transaction = new Transaction(GetRandomSubsetOfProducts());
                listOfTransactions.Add(transaction);
            }
            

        }

        public List<Product> GetRandomSubsetOfProducts()
        {
            Random rdn = new Random();
            List<Product> productList = new List<Product>();
            

            for (int i = 0; i < rdn.Next(1, 7); i++)
            {
                int next = rdn.Next(this.AllProducts.Count);
                productList.Add(this.AllProducts[next]);
                ChangeProduct(this.AllProducts[next]);
            }

            return productList;
        } 
        public void AddSoldProductsToDatabase(List<SoldProduct> soldproducts)
        {
            try
            {
                using (var ctx = new ServerContext())
                {
                    ctx.Soldproducts.AddRange(soldproducts);
                    ctx.SaveChanges();
                }
            }
            catch (System.Data.DataException)
            {
                MessageBox.Show("Kunne ikke oprette forbindelse til databasen. Tjek din konfiguration og internet adgang.", "Error!");
            }
        }

        public void AddTransactionsToDatabase(List<Transaction> listOfTransactions)
        {
            try
            {
                using (var ctx = new ServerContext())
                {
                    ctx.Transaction.AddRange(listOfTransactions);
                    ctx.SaveChanges();
                }
            }
            catch (System.Data.DataException)
            {
                MessageBox.Show("Kunne ikke oprette forbindelse til databasen. Tjek din konfiguration og internet adgang.", "Error!");
            }
        }

        public SoldProduct ChangeProduct(Product product)
        {
            SoldProduct soldProduct = new SoldProduct(product);
            try
            {
                using (var ctx = new ServerContext())
                {
                    if (product.quantity > 1)
                    {
                        product.quantity--;
                        ctx.SaveChanges();
                    }
                    else
                    {
                        ctx.Products.Remove(product);
                        ctx.SaveChanges();
                    }
                }
            }
            catch (System.Data.DataException)
            {
                MessageBox.Show("Kunne ikke oprette forbindelse til databasen. Tjek din konfiguration og internet adgang.", "Error!");
            }
            return soldProduct;

        }
        public void RemoveTransaction(int id)
        {
            using (var ctx = new ServerContext())
            {
                List<Transaction> list = new List<Transaction>(ctx.Transaction.Where(x => x.id > id));
                ctx.Transaction.RemoveRange(list);
                ctx.SaveChanges();
            }
        }

        public void RemoveSoldProducts(int id)
        {
            using (var ctx = new ServerContext())
            {
                List<SoldProduct> list = new List<SoldProduct>(ctx.Soldproducts.Where(x => x.id > id));
                ctx.Soldproducts.RemoveRange(list);
                ctx.SaveChanges();
            }
        }
        
    }
}
