using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanmissionManager.Commands;
using System.IO;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows;
using DanmissionManager.Converters;

namespace DanmissionManager.ViewModels
{
    class FindProductViewModel : BaseViewModel
    {
        public FindProductViewModel(Popups popupService) : base(popupService)
        {
            this.SearchParameter = string.Empty;
            this.CommandGetProducts = new RelayCommand(GetProductsFromDatabase);
            this.CommandSaveChanges = new RelayCommand(SaveChangesToSelectedProduct, CanExecuteSaveChanges);
            this.CommandRemoveSelectedProduct = new RelayCommand(RemoveSelectedProduct);

            //Command for getting image from user, via dialog
            this.CommandGetImage = new RelayCommand(GetImage);
            SelectedProduct = null;
        }

        #region Properties

        private string _searchParameter;
        public string SearchParameter
        {
            get { return _searchParameter; }
            set { _searchParameter = value; OnPropertyChanged("SearchParameter"); }
        }

        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set { _products = value; OnPropertyChanged("Products"); }
        }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set { _selectedProduct = value; OnPropertyChanged("SelectedProduct");
                if (SelectedProduct != null)
                {
                    this.ProductName = value.name;
                    this.Image = SelectedProduct.productImage;
                }
            }
        }

        private string _productName;
        public string ProductName
        {
            get { return _productName; }
            set { _productName = value; OnPropertyChanged("ProductName");
                IsNameValid(value, nameof(this.ProductName)); CommandSaveChanges.RaiseCanExecuteChanged(); }
        }

        private double _selectedProductPrice;
        public double SelectedProductPrice
        {
            get { return _selectedProductPrice; }
            set { _selectedProductPrice = value; OnPropertyChanged("SelectedProductPrice");
                IsPriceValid(value, nameof(this.SelectedProductPrice)); CommandSaveChanges.RaiseCanExecuteChanged(); }
        }

        private BitmapImage _image { get; set; }



        #endregion

        #region CommandProperties

        public RelayCommand CommandGetImage { get; }
        public RelayCommand CommandSaveChanges { get; }
        public RelayCommand CommandRemoveSelectedProduct { get; }
        public RelayCommand CommandGetProducts { get; }

        #endregion

        #region Methods

        private void SaveChangesToSelectedProduct()
        {
            try
            {
                using (var ctx = new ServerContext())
                {
                    if (this.SelectedProduct != null)
                    {
                        List<Product> productlist =
                            ctx.Products.Where(x => x.id.CompareTo(SelectedProduct.id) == 0).ToList();
                        Product product = productlist.First();
                        product.category = SelectedProduct.category;
                        product.desc = SelectedProduct.desc;
                        product.price = SelectedProduct.price;
                        product.name = this.SelectedProduct.name;
                        if (Image != null)
                        {
                            product.image = ImageToByteArray(Image);
                        }
                        product.quantity = SelectedProduct.quantity;
                        ctx.SaveChanges();
                        PopupService.PopupMessage("Dine ændringer er blevet gemt", "Ændringer");
                    }
                    else
                    {
                        PopupService.PopupMessage("Kunne ikke gemme dine ændringer", "Ændringer");
                    }

                }
            }
            catch (System.Data.DataException)
            {
                PopupService.PopupMessage(Application.Current.FindResource("CouldNotConnectToDatabase").ToString(), Application.Current.FindResource("Error").ToString());
            }
        }

        public bool CanExecuteSaveChanges()
        {
            return !HasErrors;
        }

        public void RemoveSelectedProduct()
        {
            try
            {
                using (var ctx = new ServerContext())
                {
                    if (this.SelectedProduct != null)
                    {
                        List<Product> productlist =
                            ctx.Products.Where(x => x.id.CompareTo(SelectedProduct.id) == 0).ToList();
                        Product product = productlist.First();
                        ctx.Products.Remove(product);
                        ctx.SaveChanges();
                        PopupService.PopupMessage("Produkt er blevet fjernet fra systemet", "Fjern produkt");
                    }
                    else
                    {
                        PopupService.PopupMessage("Intet produkt er markeret", "Fjern produkt");
                    }
                }
            }
            catch (System.Data.DataException)
            {
                PopupService.PopupMessage(Application.Current.FindResource("CouldNotConnectToDatabase").ToString(), Application.Current.FindResource("Error").ToString());
            }
            //also remove product from current observablecollection
            if (this.SelectedProduct != null)
            {
                this.Products.Remove(SelectedProduct);
            }
        }

        public void GetProductsFromDatabase()
        {
            try
            {
                using (var ctx = new ServerContext())
                {
                    //This takes into account: name, id and price
                    List<Product> list = ctx.Products.Where(x => x.name.ToLower().Contains(SearchParameter.ToLower()) ||
                        (x.id.ToString()).Contains(SearchParameter.ToLower()) ||
                        (x.price.ToString()).Contains(SearchParameter.ToLower())).ToList();
                    foreach (Product x in list)
                    {
                        if (x.image != null && x.image.Length > 0)
                        {
                            x.productImage = ImageFromBuffer(x.image);
                        }
                    }
                    ObservableCollection<Product> collection = new ObservableCollection<Product>(list);
                    this.Products = collection;

                    if (this.Products.Count == 0)
                    {
                        PopupService.PopupMessage("Din søgning gav ingen resultater.", "Resultater");
                    }
                }
            }
            catch (System.Data.DataException)
            {
                PopupService.PopupMessage(Application.Current.FindResource("CouldNotConnectToDatabase").ToString(), Application.Current.FindResource("Error").ToString());
            }
        }

        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        private BitmapImage ImageFromBuffer(Byte[] bytes)
        {
            MemoryStream stream = new MemoryStream(bytes);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }

        private BitmapImage Image
        {
            get { return _image; }
            set
            {
                _image = value;
                OnPropertyChanged("Image");
            }
        }

        private void GetImage()
        {
            if (SelectedProduct != null)
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
        }

        private byte[] ImageToByteArray(BitmapImage bitmapImage)
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

