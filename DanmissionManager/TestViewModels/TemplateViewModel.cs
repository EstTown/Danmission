using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using DanmissionManager.Commands;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using DanmissionManager.ViewModels;
using Color = System.Drawing.Color;
using DanmissionManager.Simulator;

namespace DanmissionManager.TestViewModels
{
    public class TemplateViewModel : TestBaseViewModel
    {
        public TemplateViewModel()
        {
            this._input = String.Empty;
            this.Product = null;

            this.UpdateCurrentProduct = new RelayCommand2(UpdateProduct);
            
            Product product = new Product();
            product.date = new DateTime(2015, 4, 22);
            product.id = 111111;
            product.category = 2;
            
            this.Product = product;
            
        }

        public bool CanExecuteUpdateProduct()
        {
            return !HasErrors;
        }
        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get
            {
                return _products;
            }
            set
            {
                _products = value;
                OnPropertyChanged("Products");
            }
        }

        private RelayCommand2 _updateCurrentProduct;
        public RelayCommand2 UpdateCurrentProduct
        {
            get{return _updateCurrentProduct;}
            set
            {
            _updateCurrentProduct = value;
            OnPropertyChanged("UpdateCurrentProduct");
            }
        }
        private void UpdateProduct()
        {
            
            ProductGenerator productGenerator = new ProductGenerator(100, 100);
            productGenerator.SaveProducts(productGenerator.GenerateProducts());
            Console.WriteLine("Generated Products");

            Console.ReadKey();
            TransactionGenerator transactionGenerator = new TransactionGenerator(20, 10);
            transactionGenerator.GenerateTransactions();
            Console.WriteLine("Generated transaction and moved products");


            Console.ReadKey();
            //delete everything
            productGenerator.RemoveProducts(28);
            transactionGenerator.RemoveTransaction(62);
            transactionGenerator.RemoveSoldProducts(99);
            /*
            List<Product> list = new List<Product>(productGenerator.GenerateProducts());
            foreach (Product product in list)
            {
                Console.WriteLine(product.name+"           "+product.date+"         "+product.expiredate);
            }
            */

        }
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                OnPropertyChanged("ButtonText");
                if (IsNamevalid(value) && _name!= value)
                {
                    _name = value;
                }
            }
        }
        private string _input;
        public string Input
        {
            get
            {
                return _input;
            }
            set { _input = value; OnPropertyChanged("Input"); }
        }
        
        //method get random prduct
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
        //method for generating random image
        private Bitmap GenerateRandomImage()
        {
            const int a = 140;
            Bitmap bitmap = new Bitmap(a, a);

            Random rdn = new Random();

            for (int j = 0; j < a; j++)
            {
                for (int i = 0; i < a; i++)
                {
                    bitmap.SetPixel(j, i, Color.FromArgb(rdn.Next(255), rdn.Next(255), rdn.Next(255), rdn.Next(255)));
                }
            }
            return bitmap;
        }
        //converts bitmaps to bitmapimages. Bitmaps cannot be used in wpf, but bitmapimages can.
        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
    }
}

