using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using DanmissionManager.Simulator.Interfaces;

namespace DanmissionManager.Simulator
{
    class ProductGenerator : IProductGenerator
    {
        //purpose is to generate products and put them on the server
        public ProductGenerator(int amountOfProducts, int days)
        {
            this.AmountOfProducts = amountOfProducts;
            this.Days = days;
            //need all categories and sub categories from the server
            using (var ctx = new ServerContext())
            {
                this.AllCategories = new List<Category>(ctx.Category.ToList());
                this.AllSubCategories = new List<Standardprice>(ctx.Standardprices.ToList());
            }
        }
        private List<Category> AllCategories { get; set; }
        private List<Standardprice> AllSubCategories { get; set; }
        private int AmountOfProducts;
        private int Days;
        
        public List<Product> GenerateProducts()
        {
            List<Product> list = new List<Product>();
            Random rdn = new Random();
            
            for (int i = 0; i < this.AmountOfProducts; i++)
            {
                TimeSpan timespan = new TimeSpan(this.Days*7-(rdn.Next(this.Days*7)), rdn.Next(0, 12), rdn.Next(0, 59));

                Product product = new Product();
                product.category = AllCategories[rdn.Next(0, AllCategories.Count)].id;
                
                int next = rdn.Next(0, AllSubCategories.Count);
                product.price = AllSubCategories[next].standardprice;
                product.name = AllSubCategories[next].name;
                product.isUnique = Convert.ToBoolean(rdn.Next(0, 2));

                product.date = DateTime.Now.Subtract(timespan);
                product.expiredate = product.date.Value.AddDays(7*6);
                if (product.isUnique == false)
                {
                    product.quantity = rdn.Next(1, 5);
                }
                list.Add(product);
            }
            return list;
        }

        public void SaveProducts(List<Product> productlist)
        {
            using (var ctx = new ServerContext())
            {
                ctx.Products.AddRange(productlist);
                ctx.SaveChanges();
            }
        }

        public void RemoveProducts(int id)
        {
            using (var ctx = new ServerContext())
            {
                List<Product> list = new List<Product>(ctx.Products.Where(x => x.id > id));
                ctx.Products.RemoveRange(list);
                ctx.SaveChanges();
            }
        }
    }
}
