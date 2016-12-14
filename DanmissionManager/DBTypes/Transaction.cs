using System.Linq;
using System.Windows;

namespace DanmissionManager
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("data.transactions")]
    public partial class Transaction
    {
        public Transaction()
        {
            
        }
        public Transaction(List<Product> productlist)
        {
            this.ProductsInTransaction = productlist;
            this.sum = productlist.Sum(x => x.price);
            this.date = DateTime.Now;
        }

        public Transaction(List<List<Product>> productlist)
        {
            foreach (List<Product> x in productlist)
            {
                this.ProductsInTransaction = x;
                this.sum = x.Sum(k => k.price);
                this.date = DateTime.Now;
            }
        }

        public Transaction(List<Product> productlist, DateTime date)
        {
            this.ProductsInTransaction = productlist;
            this.sum = productlist.Sum(x => x.price);
            this.date = date;
        }
        public int id { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime? date { get; set; }
        public double sum { get; set; }

        [NotMapped]
        private readonly List<Product> ProductsInTransaction;
        public void ExecuteTransaction()
        {
            try
            {
                using (var ctx = new ServerContext())
                {
                    //Commit transaction
                    ctx.Transaction.Add(this);
                    ctx.SaveChanges();
                }
            }
            catch (System.Data.DataException)
            {
                MessageBox.Show("Kunne ikke oprette forbindelse til databasen. Tjek din konfiguration og internet adgang.", "Error!");
            }
        }
    }
}
