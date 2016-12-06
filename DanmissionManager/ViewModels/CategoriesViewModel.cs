using DanmissionManager.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DanmissionManager.ViewModels
{
    class CategoriesViewModel : BaseViewModel
    {
        public CategoriesViewModel()
        {
            //instantiate member variables
            this.CreatedCategory = new Category();
            this.CreatedStandardprice = new Standardprice();
            
            this.CommandGetCategories = new RelayCommand2(GetCategoriesFromDatabase);
            this.CommandAddCategory = new RelayCommand2(AddCategory);
            this.CommandAddSubCategory = new RelayCommand2(AddSubCategory);
            this.CommandRemoveCategory = new RelayCommand2(RemoveCategory);
            this.CommandRemoveSubCategory = new RelayCommand2(RemoveSubCategory);

            GetCategoriesFromDatabase();
        }

        public RelayCommand2 CommandGetCategories { get; set; }
        public void GetCategoriesFromDatabase()
        {
            try
            {
                using (var ctx = new ServerContext())
                {
                    this.AllCategories = new ObservableCollection<Category>(ctx.Category.ToList());
                    this.AllSubCategories = new ObservableCollection<Standardprice>(ctx.Standardprices.ToList());
                }
            }
            catch (System.Data.DataException)
            {
                MessageBox.Show("Kunne ikke oprette forbindelse til databasen. Tjek din konfiguration og internet adgang.", "Error!");
            }
            ShownCategories = AllCategories;
            ShownSubCategories = AllSubCategories;
        }

        public RelayCommand2 CommandAddCategory { get; set; }
        public void AddCategory()
        {
            using (var ctx = new ServerContext())
            {
                ctx.Category.Add(this.CreatedCategory);
                ctx.SaveChanges();
            }
            AllCategories.Add(CreatedCategory);
            this.CreatedCategory = new Category();
        }

        public RelayCommand2 CommandAddSubCategory { get; set; }
        public void AddSubCategory()
        {
            //this.CreatedStandardprice.Parent_id = SelectedNewCategory.id;
            //this.CreatedStandardprice.CorrespondingCategoryString = SelectedNewCategory.name;
            this.CreatedStandardprice.Parent_id = SelectedCategory.id;
            this.CreatedStandardprice.CorrespondingCategoryString = SelectedCategory.name;
            using (var ctx = new ServerContext())
            {
                ctx.Standardprices.Add(this.CreatedStandardprice);
                ctx.SaveChanges();
            }
            this.AllSubCategories.Add(CreatedStandardprice);
            this.ShownSubCategories.Add(CreatedStandardprice);
            this.CreatedStandardprice = new Standardprice();
        }

        public RelayCommand2 CommandRemoveCategory { get; set; }
        private void RemoveCategory()
        {
            using (var ctx = new ServerContext())
            {
                List<Category> categoryList = ctx.Category.Where(x => x.id.CompareTo(SelectedCategory.id) == 0).ToList();
                Category category = categoryList.First();

                ctx.Category.Remove(category);
                ctx.SaveChanges();
                RemoveChildCategories();
            }
            this.AllCategories.Remove(this.SelectedCategory);
            this.ShownSubCategories = new ObservableCollection<Standardprice>();
        }
        private void RemoveChildCategories()
        {
            using (var ctx = new ServerContext())
            {
                List<Standardprice> subCategoryList = ctx.Standardprices.Where(x => x.Parent_id.CompareTo(SelectedCategory.id) == 0).ToList();

                //kill the child categories
                foreach (Standardprice subcategory in subCategoryList)
                {
                    Standardprice tmp = new Standardprice();
                    tmp = subcategory;
                    ctx.Standardprices.Remove(tmp);
                    ctx.SaveChanges();
                }
            }
        }

        public RelayCommand2 CommandRemoveSubCategory { get; set; }
        private void RemoveSubCategory()
        {
            using (var ctx = new ServerContext())
            {
                List<Standardprice> subCategoryList = ctx.Standardprices.Where(x => x.id.CompareTo(SelectedSubCategory.id) == 0).ToList();
                Standardprice subCategory = subCategoryList.First();
                
                ctx.Standardprices.Remove(subCategory);
                ctx.SaveChanges();
            }
            this.AllSubCategories.Remove(this.SelectedSubCategory);
            this.ShownSubCategories.Remove(this.SelectedSubCategory);
        }

        private ObservableCollection<Standardprice> _shownSubCategories;
        public ObservableCollection<Standardprice> ShownSubCategories
        {
            get { return _shownSubCategories; }
            set
            {
                _shownSubCategories = value;
                OnPropertyChanged("ShownSubCategories");
            }
        }

        private ObservableCollection<Standardprice> _allSubCategories;
        public ObservableCollection<Standardprice> AllSubCategories
        {
            get
            {
                return _allSubCategories;
            }
            set
            {
                _allSubCategories = value;
                OnPropertyChanged("AllSubCategories");
            }
        }
        private ObservableCollection<Category> _allCategories;
        public ObservableCollection<Category> AllCategories
        {
            get { return _allCategories; }
            set
            {
                _allCategories = value;
                OnPropertyChanged("AllCategories");
            }
        }

        private ObservableCollection<Category> _shownCategories;
        public ObservableCollection<Category> ShownCategories
        {
            get { return _shownCategories; }
            set
            {
                _shownCategories = value;
                OnPropertyChanged("ShownCategories");
            }
        }

        //private string _subCategorySearchParameter = string.Empty;
        //public string SubCategorySearchParameter
        //{
        //    get { return _subCategorySearchParameter; }
        //    set { _subCategorySearchParameter = value; UpdateShownSubCategories(SubCategorySearchParameter); OnPropertyChanged("SubCategorySearchParameter"); }
        //}

        //private string _categorySearchParameter = string.Empty;
        //public string CategorySearchParameter
        //{
        //    get { return _categorySearchParameter; }
        //    set { _categorySearchParameter = value; UpdateShownCategories(CategorySearchParameter); OnPropertyChanged("CategorySearchParameter"); }
        //}

        //private void UpdateShownSubCategories(string subCategorySearchParameter)
        //{
        //    ShownSubCategories = new ObservableCollection<Standardprice>(AllSubCategories.Where(x => x.name.Contains(subCategorySearchParameter)));
        //}

        //private void UpdateShownCategories(string categorySearchParameter)
        //{
        //    ShownCategories = new ObservableCollection<Category>(AllCategories.Where(x => x.name.Contains(categorySearchParameter)));
        //}

        //properties for containing the newly created category and subcategory
        private Category _createdCategory;
        public Category CreatedCategory
        {
            get { return _createdCategory; }
            set
            {
                _createdCategory = value;
                OnPropertyChanged("CreatedCategory");
            }
        }
        private Standardprice _createdStandardprice;
        public Standardprice CreatedStandardprice
        {
            get { return _createdStandardprice; }
            set
            {
                _createdStandardprice = value;
                OnPropertyChanged("CreatedStandardPrice");
            }
        }
        //there are two properties, because it is possible to select a category from multiple sources
        //maybe both those selection should change the same property, but that can be changed later if that makes more
        //sense

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                OnPropertyChanged("SelectedCategory");
                ChangeCollection();
            }
        }
        private void ChangeCollection()
        {
            List<Standardprice> list = new List<Standardprice>();
            list = this.AllSubCategories.Where(x => this.SelectedCategory.id == x.Parent_id).ToList();
            
            ObservableCollection<Standardprice> collection = new ObservableCollection<Standardprice>(list);
            this.ShownSubCategories = collection;
        }

        //private Category _selectedNewCategory;
        //public Category SelectedNewCategory
        //{
        //    get { return _selectedNewCategory; }
        //    set
        //    {
        //        _selectedNewCategory = value;
        //        OnPropertyChanged("SelectedNewCategory");
        //    }
        //}

        //property for selected sub category item
        private Standardprice _selectedSubCategory;
        public Standardprice SelectedSubCategory
        {
            get
            {
                return _selectedSubCategory;
            }
            set
            {
                _selectedSubCategory = value;
                OnPropertyChanged("SelectedSubCategory");
            }
        }
    }
}
