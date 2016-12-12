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
            List<Transaction> list = new List<Transaction>();

            Random rdn = new Random();

            for (int i = 0; i < this.NumberOfTransactions; i++)
            {
                List<Product> listPerTransaction = new List<Product>();
                List<SoldProduct> listofSoldProducts = new List<SoldProduct>();
                TimeSpan timespan = new TimeSpan(this.Days * 7 - (rdn.Next(this.Days * 7)), rdn.Next(0, 12), rdn.Next(0, 59));

                using (var ctx = new ServerContext())
                {
                    //puts random products in a "basket", also random amount of products
                    for (int j = 0; j < rdn.Next(1, 5); j++)
                    {
                        //declared next to make sure no duplicates get used
                        int next = rdn.Next(this.AllProducts.Count);

                        listPerTransaction.Add(this.AllProducts[next]);

                        if (this.AllProducts[next].isUnique == true)
                        {
                            this.AllProducts.Remove(this.AllProducts[next]);
                            ctx.Products.Remove(AllProducts[next]);
                        }
                        else if (AllProducts[next].quantity <= 1)
                        {
                            this.AllProducts.Remove(this.AllProducts[next]);
                            ctx.Products.Remove(AllProducts[next]);
                        }
                        else
                        {
                            this.AllProducts[next].quantity--;
                            ctx.Products.Find(AllProducts[next]).quantity--;
                        }
                        ctx.SaveChanges();
                    }
                }

                Transaction transaction = new Transaction(listPerTransaction, DateTime.Now.Subtract(timespan));
                transaction.ExecuteTransaction();

                for (int j = 0; j < listPerTransaction.Count; j++)
                {
                    SoldProduct soldproduct = new SoldProduct(listPerTransaction[j]);
                    soldproduct.transactionid = transaction.id;
                    listofSoldProducts.Add(soldproduct);
                }
                AddSoldProductsToDatabase(listofSoldProducts);
            }
        }

        public void AddSoldProductsToDatabase(List<SoldProduct> soldproducts)
        {
            try
            {
                using (var ctx = new ServerContext())
                {
                    //save all products as products sold.
                    ctx.Soldproducts.AddRange(soldproducts);
                    ctx.SaveChanges();
                }
            }
            catch (System.Data.DataException)
            {
                MessageBox.Show("Kunne ikke oprette forbindelse til databasen. Tjek din konfiguration og internet adgang.", "Error!");
            }
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
