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

namespace DanmissionManager.ViewModels
{
    class FindProductViewModel : BaseViewModel
    {
        public FindProductViewModel()
        {
            this.SearchParameter = string.Empty;
            this.CommandGetProducts = new RelayCommand2(GetProductsFromDatabase);
            this.CommandSaveChanges = new RelayCommand2(SaveChangesToSelectedProduct);
            this.CommandRemoveSelectedProduct = new RelayCommand2(RemoveSelectedProduct);

            //Command for getting image from user, via dialog
            RelayCommand2 commandGetImage = new RelayCommand2(GetImage);
            this.CommandGetImage = commandGetImage;
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
        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                OnPropertyChanged("Products");
            }
        }
        public void SortCollectionCategory()
        {
            
        }
        //property that handles what happens when a product from the list gets selected,
        //after which addional information will be shown
        public RelayCommand2 CommandSelectProduct { get; set; }
        
        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get { return _selectedProduct;}
            set
            {
                _selectedProduct = value;
                this.Image = SelectedProduct.productImage;
                OnPropertyChanged("SelectedProduct");
                //CommandSelectProduct.RaiseCanExecuteChanged(); //not used right now
            }
        }
        public RelayCommand2 CommandSaveChanges { get; set; }
        //method that saves changes to selectedproduct
        private void SaveChangesToSelectedProduct()
        {
            try
            {
                using (var ctx = new ServerContext())
                {
                    List<Product> productlist = ctx.Products.Where(x => x.id.CompareTo(SelectedProduct.id) == 0).ToList();
                    Product product = productlist.First();
                    product.category = SelectedProduct.category;
                    product.desc = SelectedProduct.desc;
                    product.price = SelectedProduct.price;
                    product.image = ImageToByteArray(Image);
                    ctx.SaveChanges();
                }
            }
            catch (System.Data.DataException)
            {
                MessageBox.Show("Kunne ikke oprette forbindelse til databasen. Tjek din konfiguration og internet adgang.", "Error!");
            }
        }
        public RelayCommand2 CommandRemoveSelectedProduct { get; set; }
        public void RemoveSelectedProduct()
        {
            try
            {
                using (var ctx = new ServerContext())
                {
                    
                    List<Product> productlist = ctx.Products.Where(x => x.id.CompareTo(SelectedProduct.id) == 0).ToList();
                    Product product = productlist.First();
                    

                    ctx.Products.Remove(product);

                    ctx.SaveChanges();
                }
            }
            catch (System.Data.DataException)
            {
                MessageBox.Show("Kunne ikke oprette forbindelse til databasen. Tjek din konfiguration og internet adgang.", "Error!");
            }
            //also remove product from current obserablecollection
            this.Products.Remove(SelectedProduct);
        }
        //property that will contain the command/method and executes it
        //does not need a backing field
        public RelayCommand2 CommandGetProducts { get; set; }
        //method get products
        public void GetProductsFromDatabase()
        {
            try
            {
                using (var ctx = new ServerContext())
                {
                    //List<Product> list = ctx.Products.Where(x => x.name.ToLower().CompareTo(SearchParameter.ToLower()) == 0).ToList();

                    //This is more dyniamic, although it runs smoothly, the initial query seems to lag, causing a small stutter
                    //This takes into account: name, id and price, but delivers awful search results...
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
                }
            }
            catch (System.Data.DataException)
            {
                MessageBox.Show("Kunne ikke oprette forbindelse til databasen. Tjek din konfiguration og internet adgang.", "Error!");
            }
        }
        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
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

        public BitmapImage Image
        {
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
                Image = new BitmapImage(uri);
            }
        }

        public byte[] ImageToByteArray(BitmapImage bitmapImage)
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

