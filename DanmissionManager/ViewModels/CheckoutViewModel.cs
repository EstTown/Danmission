using DanmissionManager.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DanmissionManager.ViewModels
{
    class CheckoutViewModel : BaseViewModel
    {
        public CheckoutViewModel()
        {
            this.SearchParameter = string.Empty;
            this.CommandGetProductByID = new RelayCommand2(CommandGetProductByIDFromDatabase);
            this.CommandAddToBasket = new RelayCommand2(CommandAddSelectedToBasket);
            this.CommandClearBasket = new RelayCommand2(CommandClearAllProductsFromBasket);
            this.CommandComplete = new RelayCommand2(CommandCompletePurchase);


            ProductsInBasket = new ObservableCollection<Product>();
        }

        private string _searchParameter;
        public string SearchParameter
        {
            get { return _searchParameter; }
            set
            {
                _searchParameter = value;
                OnPropertyChanged("SearchParameter");
            }
        }

        private double _totalPrice;
        public string TotalPrice
        {
            get { return _totalPrice.ToString(); }
            set
            {
                _totalPrice = Convert.ToDouble(value);
                OnPropertyChanged("TotalPrice");
            }
        }

        private ObservableCollection<Product> _productsInBasket;
        public ObservableCollection<Product> ProductsInBasket
        {
            get { return _productsInBasket; }
            set
            {
                _productsInBasket = value;
                OnPropertyChanged("ProductInBasket");
            }
        }

        private Product _product;
        public Product Product
        {
            get { return _product; }
            set
            {
                _product = value;
                OnPropertyChanged("Product");
            }
        }

        //does not need a backing field
        public RelayCommand2 CommandGetProductByID { get; set; }
        //method get products
        public void CommandGetProductByIDFromDatabase()
        {
            try
            {
                using (var ctx = new ServerContext())
                {
                    if (ctx.Products.Where(x => x.id.ToString() == SearchParameter).FirstOrDefault() != null)
                    {
                        Product tmp = ctx.Products.Where(x => x.id.ToString() == SearchParameter).FirstOrDefault();

                        if (tmp.image != null)
                        {
                            tmp.productImage = ImageFromBuffer(tmp.image);
                        }

                        this.Product = tmp;
                    }
                    else
                    {
                        MessageBox.Show("Produktet kunne ikke findes!", "Error!");
                    }
                    
                }

            }
            catch (System.Data.DataException)
            {
                MessageBox.Show("Kunne ikke oprette forbindelse til databasen. Tjek din konfiguration og internet adgang.", "Error!");
            }
        }

        public RelayCommand2 CommandAddToBasket { get; set; }
        public void CommandAddSelectedToBasket()
        {
            if (Product != null)
            {
                ProductsInBasket.Add(Product);
                this.TotalPrice = ProductsInBasket.Sum(x => x.price).ToString();
            }
        }

        public RelayCommand2 CommandClearBasket { get; set; }
        public void CommandClearAllProductsFromBasket()
        {
            ProductsInBasket.Clear();
            this.TotalPrice = "0";
        }

        public RelayCommand2 CommandComplete { get; set; }
        public void CommandCompletePurchase()
        {
            int transId = 0;
            double transSum = 0;
            //Make transaction

            //Move products to soldproducts
            List<SoldProduct> soldList = new List<SoldProduct>();
            foreach (Product x in ProductsInBasket)
            {
                SoldProduct tmp = new SoldProduct();
                tmp.previousid = x.id;
                tmp.name = x.name;
                tmp.price = x.price;
                tmp.desc = x.desc;
                tmp.isUnique = x.isUnique;
                tmp.image = x.image;
                tmp.category = x.category;
                soldList.Add(tmp);
            }
            try
            {
                using (var ctx = new ServerContext())
                {
                    //Date and id is assigned serverside.
                    Transaction trans = new Transaction();
                    trans.sum = this.ProductsInBasket.Sum(x => x.price);
                    transSum = trans.sum;

                    //Commit transaction
                    ctx.Transaction.Add(trans);
                    transId = trans.id;

                    //Thrown all the stuffz away!
                    foreach (SoldProduct x in soldList)
                    {
                        x.transactionid = transId;
                        Console.WriteLine("Adding products");
                    }
                    ctx.Soldproducts.AddRange(soldList);
                    ctx.SaveChanges();

                    notifyUserAboutCompletedPurchase(transId, transSum);
                }
            }
            catch (System.Data.DataException)
            {
                MessageBox.Show("Kunne ikke oprette forbindelse til databasen. Tjek din konfiguration og internet adgang.", "Error!");
            }

            
        }

        private void notifyUserAboutCompletedPurchase(int transid, double sum)
        {
            MessageBox.Show("Tranaction ID: " + transid + "/nSum: " + sum ,"Purchase completed");
        }

        public BitmapImage ImageFromBuffer(Byte[] bytes)
        {
            MemoryStream stream = new MemoryStream(bytes);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }
    }
}
