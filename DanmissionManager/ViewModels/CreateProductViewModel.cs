﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using DanmissionManager.Commands;
using DanmissionManager.DBTypes.NewFolder1;

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
            
            //get all categories from database
            using (var ctx = new ServerContext())
            {
                ObservableCollection<Category> categories = new ObservableCollection<Category>(ctx.Category.ToList());
                this.Categories = categories;
            }
            //get all subcategories from database
            using (var ctx = new ServerContext())
            {
                ObservableCollection<Standardprice> allsubcategories = new ObservableCollection<Standardprice>(ctx.Standardprices.ToList());
                this.AllSubCategories = allsubcategories;
            }
            double d = 0;
            this.Product.price = d;
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

                //method that changes subcategories collection, based on selectedcategory
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
            product.category = this.SelectedSubCategory.id;
            product.isUnique = this.Product.isUnique;
            product.desc = this.Product.desc;
            double d = 0;
            if (this.Product.price.Equals(d) == true)
            {
                Product.price = this.SelectedSubCategory.standardprice;
            }
            else
            {
                product.price = this.Product.price;
            }
            
            using (var ctx = new ServerContext())
            {
                ctx.Products.Add(product);
                ctx.SaveChanges();
            }


            
        }
        //checkbox
        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged("IsChecked");
                Console.WriteLine(IsChecked);
            }
        }
        private void ChangeCollection()
        {
            
            List<Standardprice> list = new List<Standardprice>();
            list = this.AllSubCategories.Where(x => this.SelectedCategory.id == x.Parent_id).ToList();

            
            ObservableCollection<Standardprice> collection = new ObservableCollection<Standardprice>(list);
            this.SubCategories = collection;

        }

    }
}