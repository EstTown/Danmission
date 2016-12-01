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
