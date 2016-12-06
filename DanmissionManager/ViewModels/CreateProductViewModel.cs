using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using DanmissionManager.Commands;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Data;
using DanmissionManager.Converters;

namespace DanmissionManager.ViewModels
{
    class CreateProductViewModel : BaseViewModel
    {
        public CreateProductViewModel()
        {
            this.Product = new Product();
            this.Product.price = 0.0;
            this.Product.isUnique = true;

            //command for adding a product to the server
            RelayCommand2 commandAddProduct = new RelayCommand2(AddProduct);
            this.CommandAddProduct = commandAddProduct;
            //Command for getting image from user, via dialog
            RelayCommand2 commandGetImage = new RelayCommand2(GetImage);
            this.CommandGetImage = commandGetImage;
            //get all categories and subcategories from database
            try
            {
                using (var ctx = new ServerContext())
                {
                    ObservableCollection<Category> categories = new ObservableCollection<Category>(ctx.Category.ToList());
                    this.Categories = categories;
                    ObservableCollection<Standardprice> allsubcategories = new ObservableCollection<Standardprice>(ctx.Standardprices.ToList());
                    this.AllSubCategories = allsubcategories;
                }
            }
            catch (System.Data.DataException)
            {
                MessageBox.Show("Kunne ikke oprette forbindelse til databasen. Tjek din konfiguration og internet adgang.", "Error!");
            }
        }
        private Standardprice _selectedSubCategory;
        public Standardprice SelectedSubCategory
        {
            get { return _selectedSubCategory; }
            set
            {
                _selectedSubCategory = value;
                OnPropertyChanged("SelectedSubCategory");
            }
        }
        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                OnPropertyChanged("SelectedCategory");

                //run method that changes subcategories collection, based on selectedcategory
                ChangeCollection();
            }
        }
        private ObservableCollection<Standardprice> AllSubCategories { get; }

        private ObservableCollection<Standardprice> _subCategories;
        public ObservableCollection<Standardprice> SubCategories
        {
            get { return _subCategories;}
            set
            {
                _subCategories = value; 
                OnPropertyChanged("SubCategories");
            }
        }
        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                OnPropertyChanged("Categories");
            }
        }
        private string _productName;
        public string ProductName
        {
            get
            {
                return _productName;
            }
            set
            {
                _productName = value;
                OnPropertyChanged("ProductName");
            }
        }
        private Product _product;
        public Product Product
        {
            get
            {
                return _product;
            }
            set
            {
                _product = value;
                OnPropertyChanged("Product");

            }
        }
        public RelayCommand2 CommandAddProduct { get; set; }
        public void AddProduct()
        {
            Product product = new Product();
            product.date = DateTime.Now;
            product.name = this.Product.name;
            product.category = this.SelectedCategory.id;
            product.isUnique = this.Product.isUnique;
            product.desc = this.Product.desc;
            if (product.isUnique == false)
            {
                product.quantity = this.AmountOfProducts;
            }
            product.expiredate = product.date.Value.AddDays(this.Weeks*7);
            if (Image != null)
            {
                product.image = this.imageToByteArray(Image);
            }
            if (this.Product.price.Equals(0.0) == true)
            {
                product.price = this.SelectedSubCategory.standardprice;
            }
            else
            {
                product.price = this.Product.price;
            }
            using (var ctx = new ServerContext())
            {
                ctx.Products.Add(product);
                ctx.SaveChanges();
                MessageBox.Show("Assigned ID: " + product.id, "Success!");
            }
        }
        private int _weeks;
        public int Weeks
        {
            get { return _weeks; }
            set
            {
                _weeks = value;
                OnPropertyChanged("Weeks");
            }
        }

        private int _amountOfProducts;
        public int AmountOfProducts
        {
            get { return _amountOfProducts; }
            set
            {
                _amountOfProducts = value;
                OnPropertyChanged("AmountOfProducts");
            }
        }

        private void ChangeCollection()
        {
            List<Standardprice> list = new List<Standardprice>();
            list = this.AllSubCategories.Where(x => this.SelectedCategory.id == x.Parent_id).ToList();
            ObservableCollection<Standardprice> collection = new ObservableCollection<Standardprice>(list);
            this.SubCategories = collection;

        }
        public BitmapImage Image {
            get { return _image; }
            set
            {
                _image = value;
                OnPropertyChanged("Image");
            }
        }
        private BitmapImage _image { get; set; }
        public RelayCommand2 CommandGetImage { get; set; }
        public void GetImage()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.Title = "Open Image";
            dlg.Filter = "PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg";

            if (dlg.ShowDialog() == true)
            {
                var uri = new Uri(dlg.FileName);
                //Resizes image, due to performance concerns
                Image = BitmapResizer.Scaler(new BitmapImage(uri), 500, 500);
            }
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
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

        public byte[] imageToByteArray(BitmapImage bitmapImage)
        {
            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }

    }
}