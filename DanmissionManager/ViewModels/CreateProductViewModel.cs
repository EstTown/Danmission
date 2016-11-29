using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanmissionManager.Commands;

namespace DanmissionManager.ViewModels
{
    class CreateProductViewModel : BaseViewModel
    {
        public CreateProductViewModel()
        {
            this.ProductName = string.Empty;
            this.Product = new Product();
            this.Product.name = string.Empty;
            
            //command for adding a product to the server
            RelayCommand2 commandAddProduct = new RelayCommand2(AddProduct);
            this.CommandAddProduct = commandAddProduct;
            
            //get all categories from server
            using (var ctx = new ServerContext())
            {
                ObservableCollection<Category> categories = new ObservableCollection<Category>(ctx.Category.ToList());
                this.Categories = categories;
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
                Console.WriteLine(SelectedCategory.name);
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
            product.category = 5;
            product.isUnique = true;
            product.price = 50;
            product.desc = this.Product.desc;

            
            using (var ctx = new ServerContext())
            {
                ctx.Products.Add(product);
                ctx.SaveChanges();
            }
            
        }

    }
}