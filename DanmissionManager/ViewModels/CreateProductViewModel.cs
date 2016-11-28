using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanmissionManager.ViewModels
{
    class CreateProductViewModel : BaseViewModel
    {
        public CreateProductViewModel()
        {
            this.ProductName = string.Empty;
            this.Product = new Product();
            this.Product.name = string.Empty;
            
            
            //get all categories from server
            using (var ctx = new ServerContext())
            {
                ObservableCollection<Category> categories = new ObservableCollection<Category>(ctx.Categories.ToList());
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
    }
}