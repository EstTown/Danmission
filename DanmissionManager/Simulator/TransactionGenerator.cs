using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanmissionManager.Simulator.Interfaces;

namespace DanmissionManager.Simulator
{
    class TransactionGenerator : ITransactionGenerator
    {
        public TransactionGenerator(int numberoftransaction, int days, List<Product> listofproducts)
        {
            this.Days = days;
            this.NumberOfTransactions = numberoftransaction;
            this.AllProducts = listofproducts;
        }
        public List<Product> AllProducts { get; set; }
        private int NumberOfTransactions;
        private int Days;
        public List<Transaction> GenerateTransactions()
        {
            List<Transaction> list = new List<Transaction>();
            Random rdn = new Random();

            for (int i = 0; i < this.NumberOfTransactions; i++)
            {
                TimeSpan timespan = new TimeSpan(this.Days * 7 - (rdn.Next(this.Days * 7)), rdn.Next(0, 12), rdn.Next(0, 59));
                Transaction transaction = new Transaction();

                for (int j = 0; j < rdn.Next(1,5); j++)
                {
                    transaction.sum += AllProducts[rdn.Next(AllProducts.Count - 1)].price;
                    //should do something about the products that are contained within the transaction
                }
                transaction.date = DateTime.Now.Subtract(timespan);
            }



            return list;
        }
    }
}
