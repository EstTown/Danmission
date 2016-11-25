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
using DanmissionManager.TestClasses;
using Color = System.Drawing.Color;

namespace DanmissionManager.ViewModels
{
    class TemplateViewModel : BaseViewModel
    {
        public TemplateViewModel()
        {
            this._buttonText = "ThisCameFromTheViewModel";
            this._input = String.Empty;
            this.Product = null;

            this.UpdateCurrentProduct = new RelayCommand2(UpdateProduct);
            
            Product2 product = new Product2();
            product.date = new DateTime(2015, 4, 22);
            product.id = 111111;
            product.category = 2;
            product.ProductImage = GenerateRandomImage();
            this.Product = product;
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


        public RelayCommand2 UpdateCurrentProduct { get; set; }
        private void UpdateProduct()
        {
            using (var ctx = new ServerContext())
            {
                ObservableCollection<Product> collection = new ObservableCollection<Product>(ctx.Products.ToList());
                this.Products = collection;
            }
        }



        private string _buttonText;
        public string ButtonText
        {
            get
            {
                return _buttonText;
            }
            set
            {
                _buttonText = value;
                OnPropertyChanged("ButtonText");
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
        private Product2 _product;
        public Product2 Product
        {
            get { return _product; }
            set
            {
                _product = value;
                OnPropertyChanged("Product");
            }
        }

        public void GetRandomProduct()
        {
            //
        }
        
        
        


        //make method for generating random image
        private Bitmap GenerateRandomImage()
        {
            const int a = 40;
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
    }
}

