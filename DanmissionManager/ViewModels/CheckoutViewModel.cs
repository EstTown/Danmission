using DanmissionManager.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DanmissionManager.ViewModels
{
    class CheckoutViewModel : BaseViewModel
    {
        public CheckoutViewModel(Popups popupService) : base(popupService)
        {
            this.SearchParameter = string.Empty;
            this.CommandGetProductByID = new RelayCommand(GetProductByIdFromDatabase);
            this.CommandAddSelectedToBasket = new RelayCommand(AddSelectedToBasket);
            this.CommandClearBasket = new RelayCommand(ClearAllProductsFromBasket);
            this.CommandCompletePurchase = new RelayCommand(CompletePurchase);
            
            GetCategoriesFromDatabase();
            
            ProductsInBasket = new ObservableCollection<List<Product>>();
            this._selectedAmount = 1;
        }

        #region Properties

        private string _searchParameter;
        public string SearchParameter
        {
            get { return _searchParameter; }
            set { _searchParameter = value; OnPropertyChanged("SearchParameter"); }
        }
        private double _totalPrice;
        public string TotalPrice
        {
            get { return _totalPrice.ToString(); }
            set { _totalPrice = Convert.ToDouble(value); OnPropertyChanged("TotalPrice"); }
        }

        private int _selectedAmount;
        public string SelectedAmount
        {
            get { return _selectedAmount.ToString(); }
            set { _selectedAmount = Convert.ToInt32(value); OnPropertyChanged("SelectedAmount"); }
        }

        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; OnPropertyChanged("Quantity"); }
        }

        private string _categoryName;
        public string CategoryName
        {
            get { return _categoryName; }
            set { _categoryName = value; OnPropertyChanged("CategoryName"); }
        }

        private List<Category> _allCategories;

        private ObservableCollection<List<Product>> _productsInBasket;
        public ObservableCollection<List<Product>> ProductsInBasket
        {
            get { return _productsInBasket; }
            set { _productsInBasket = value; OnPropertyChanged("ProductInBasket"); }
        }
        private Product _product;
        public Product Product
        {
            get { return _product; }
            set { _product = value; OnPropertyChanged("Product");
                if (this.Product.isUnique)
                {
                    this.Quantity = 1;
                }
                else
                {
                    this.Quantity = Convert.ToInt32(value.quantity);
                }
            }
        }

        #endregion

        #region CommandProperties

        public RelayCommand CommandGetProductByID { get; set; }
        public RelayCommand CommandAddSelectedToBasket { get; set; }
        public RelayCommand CommandClearBasket { get; set; }
        public RelayCommand CommandCompletePurchase { get; set; }

        #endregion

        #region Methods

        private string AssignCorrespondingCategory()
        {
            Category correctCategory = new Category();
            correctCategory = _allCategories.Where(x => x.id == this.Product.category).FirstOrDefault();
            if (correctCategory != null)
            {
                return correctCategory.name;
            }
            return this.Product.category.ToString();
        }
        
        private void notifyUserAboutCompletedPurchase(int transid, double sum)
        {
            PopupService.PopupMessage("Transaktion ID: " + transid + "\nSum: " + sum, "Transaktion gennemført");
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

        private void GetCategoriesFromDatabase()
        {
            try
            {
                using (var ctx = new ServerContext())
                {
                    this._allCategories = new List<Category>(ctx.Category.ToList());
                }
            }
            catch (System.Data.DataException)
            {
                PopupService.PopupMessage(Application.Current.FindResource("CouldNotConnectToDatabase").ToString(), Application.Current.FindResource("Error").ToString());
            }
        }

        public void GetProductByIdFromDatabase()
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
                        this.CategoryName = AssignCorrespondingCategory();
                    }
                    else
                    {
                        PopupService.PopupMessage(
                            Application.Current.FindResource("CPProductCouldNotBeFound").ToString(),
                            Application.Current.FindResource("Error").ToString());
                        this.SearchParameter = string.Empty;
                    }
                }
            }
            catch (System.Data.DataException)
            {
                PopupService.PopupMessage(Application.Current.FindResource("CouldNotConnectToDatabase").ToString(), Application.Current.FindResource("Error").ToString());
            }
        }

        public void AddSelectedToBasket()
        {
            if (Product != null && this.Quantity >= Convert.ToInt32(this.SelectedAmount) && (this.Quantity - Convert.ToInt32(this.SelectedAmount)) >= 0)
            {
                List<Product> productList = new List<Product>();

                for (int i = 0; i < Convert.ToInt32(SelectedAmount); i++)
                {
                    Product product = new Product();
                    product = this.Product.ShallowCopy();
                    productList.Add(product);

                    this.TotalPrice = (Convert.ToDouble(this.TotalPrice) + product.price).ToString();
                }

                ProductsInBasket.Add(productList);
                //this.TotalPrice = ProductsInBasket.Sum(x => x.price).ToString();
                this.Quantity -= Convert.ToInt32(this.SelectedAmount);

                //Reset this textbox
                this.SelectedAmount = 1.ToString();
            }
            else
            {
                PopupService.PopupMessage("Du forsøger at tilføje flere produkter end der findes.", "Produkter");
            }
        }

        public void ClearAllProductsFromBasket()
        {
            ProductsInBasket.Clear();
            this.TotalPrice = "0";
        }

        public void CompletePurchase()
        {
            //Do transaction
            if (ProductsInBasket.Count > 0)
            {
                Transaction transaction = new Transaction(this.ProductsInBasket.ToList());
                transaction.ExecuteTransaction();

                //Move products to soldproducts
                List<SoldProduct> soldList = new List<SoldProduct>();
                foreach (List<Product> x in this.ProductsInBasket.ToList())
                {
                    foreach (Product product in x)
                    {
                        SoldProduct soldproduct = new SoldProduct(product);
                        soldproduct.transactionid = transaction.id;
                        soldList.Add(soldproduct);
                    }
                }
                AddSoldProductsToDatabase(soldList);
                RemoveProductsInBasketFromDatabase(ProductsInBasket.ToList());
                notifyUserAboutCompletedPurchase(transaction.id, transaction.sum);
            }
            else
            {
                PopupService.PopupMessage("Der er ikke blevet tilføjet nogen produkter til kurven.", "Ingen produkter");
            }
        }

        public void AddSoldProductsToDatabase(List<SoldProduct> soldproducts)
        {
            try
            {
                using (var ctx = new ServerContext())
                {
                    //save all products as products sold.
                    ctx.Soldproducts.AddRange(soldproducts);
                    ctx.SaveChanges();
                }
            }
            catch (System.Data.DataException)
            {
                PopupService.PopupMessage(Application.Current.FindResource("CouldNotConnectToDatabase").ToString(), Application.Current.FindResource("Error").ToString());
            }
        }

        public void RemoveProductsInBasketFromDatabase(List<List<Product>> productlist)
        {
            try
            {
                using (var ctx = new ServerContext())
                {
                    //Remove from inventory
                    foreach (List<Product> k in productlist)
                    {
                        foreach (Product x in k)
                        {
                            foreach (Product y in ctx.Products.ToList())
                            {
                                if (x.id == y.id)
                                {
                                    if (x.isUnique == true)
                                    {
                                        ctx.Products.Remove(y);
                                    }
                                    else if (y.quantity <= 1)
                                    {
                                        ctx.Products.Remove(y);
                                    }
                                    else
                                    {
                                        y.quantity--;
                                    }
                                }
                            }
                        }

                    }
                    ctx.SaveChanges();
                    ClearAllProductsFromBasket();
                }
            }
            catch (System.Data.DataException)
            {
                PopupService.PopupMessage(Application.Current.FindResource("CouldNotConnectToDatabase").ToString(), Application.Current.FindResource("Error").ToString());
            }
        }

        #endregion
    }
}
