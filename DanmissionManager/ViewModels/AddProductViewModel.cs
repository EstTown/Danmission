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
using DanmissionManager.Simulator;

namespace DanmissionManager.ViewModels
{
    class AddProductViewModel : BaseViewModel
    {
        public AddProductViewModel(Popups popupService) : base(popupService)
        {
            this.Product = new Product();
            this.CommandAddProduct = new RelayCommand(AddProduct, CanExecuteAddProduct);
            this.CommandGetImage = new RelayCommand(GetImage);
            this.AmountOfProducts = 2;

            //get all categories and subcategories from database
            GetFromDatabase();
        }

        #region Properties

        private Standardprice _selectedSubCategory;
        public Standardprice SelectedSubCategory
        {
            get { return _selectedSubCategory; }
            set { _selectedSubCategory = value; OnPropertyChanged("SelectedSubCategory");
                this.CommandAddProduct.RaiseCanExecuteChanged(); }
        }

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set { _selectedCategory = value; OnPropertyChanged("SelectedCategory");
                this.CommandAddProduct.RaiseCanExecuteChanged();
                //method that changes subcategories collection, based on selectedcategory
                ChangeCollection(); }
        }

        private ObservableCollection<Standardprice> AllSubCategories { get; set; }

        private ObservableCollection<Standardprice> _subCategories;
        public ObservableCollection<Standardprice> SubCategories
        {
            get { return _subCategories; }
            set { _subCategories = value; OnPropertyChanged("SubCategories"); }
        }
        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set { _categories = value; OnPropertyChanged("Categories"); }
        }

        private Product _product;
        public Product Product
        {
            get { return _product; }
            set { _product = value; OnPropertyChanged("Product"); }
        }

        private string _productName;
        public string ProductName
        {
            get { return _productName; }
            set { _productName = value; OnPropertyChanged("ProductName");
                IsNameValid(value, nameof(this.ProductName)); CommandAddProduct.RaiseCanExecuteChanged(); }
        }

        private string _productDesc;
        public string ProductDesc
        {
            get { return _productDesc; }
            set { _productDesc = value; OnPropertyChanged("ProductDesc"); }
        }

        private int _weeks;
        public int Weeks
        {
            get { return _weeks; }
            set { _weeks = value; OnPropertyChanged("Weeks"); IsWeeksValid(value);
                CommandAddProduct.RaiseCanExecuteChanged(); }
        }

        private int? _amountOfProducts;
        public int? AmountOfProducts
        {
            get { return _amountOfProducts; }
            set { _amountOfProducts = value; OnPropertyChanged("AmountOfProducts");
                IsAmountOfProductsValid(value, nameof(this.AmountOfProducts)); CommandAddProduct.RaiseCanExecuteChanged(); }
        }
        
        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set { _isChecked = value;
                    if (value == true)
                    {
                        this.AmountOfProducts = 2;
                    }
                CommandAddProduct.RaiseCanExecuteChanged();
                }
        }

        private double _price;
        public double Price
        {
            get { return _price; }
            set { _price = value; OnPropertyChanged("Price");
                IsPriceValid(value, nameof(this.Price)); CommandAddProduct.RaiseCanExecuteChanged(); }
        }

        private BitmapImage _image;
        public BitmapImage Image
        {
            get { return _image; }
            set { _image = value; OnPropertyChanged("Image"); }
        }

        #endregion

        #region Command properties

        public RelayCommand CommandAddProduct { get; }
        public RelayCommand CommandGetImage { get; }

        #endregion

        #region Methods
        
        private void ChangeCollection()
        {
            List<Standardprice> list = new List<Standardprice>();
            list = this.AllSubCategories.Where(x => this.SelectedCategory.id == x.Parent_id).ToList();
            this.SubCategories = new ObservableCollection<Standardprice>(list);
        }

        public void AddProduct()
        {
            Product product = new Product(this.ProductName, this.SelectedSubCategory.Parent_id, this.IsChecked, this.ProductDesc);

            if (product.isUnique == false)
            {
                product.quantity = this.AmountOfProducts;
            }
            else
            {
                product.quantity = 1;
            }

            if (this.Weeks > 0)
            {
                product.expiredate = product.date.Value.AddDays(this.Weeks * 7);
            }
            else
            {
                product.expiredate = null;
            }

            if (Image != null)
            {
                product.image = this.imageToByteArray(Image);
            }
            if (this.Price.Equals(0.0) == true)
            {
                product.price = this.SelectedSubCategory.standardprice;
            }
            else
            {
                product.price = this.Price;
            }
            try
            {
                using (var ctx = new ServerContext())
                {
                    ctx.Products.Add(product);
                    ctx.SaveChanges();
                    MessageBox.Show("Assigned ID: " + product.id, "Success!");
                }
            }
            catch (System.Data.DataException)
            {
                PopupService.PopupMessage(Application.Current.FindResource("CouldNotConnectToDatabase").ToString(), Application.Current.FindResource("Error").ToString());
            }
        }

        public bool CanExecuteAddProduct()
        {
            return (!HasErrors && this.SelectedCategory != null &&this.SelectedSubCategory != null);
        }

        public void GetFromDatabase()
        {
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
                PopupService.PopupMessage(Application.Current.FindResource("CouldNotConnectToDatabase").ToString(), Application.Current.FindResource("Error").ToString());
            }
        }

        public void GetImage()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.Title = "Open Image";
            dlg.Filter = "PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg";

            if (dlg.ShowDialog() == true)
            {
                var uri = new Uri(dlg.FileName);
                // Resizes image, due to performance concerns
                Image = BitmapResizer.Scaler(new BitmapImage(uri), 500, 500);
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

        #endregion
    }
}