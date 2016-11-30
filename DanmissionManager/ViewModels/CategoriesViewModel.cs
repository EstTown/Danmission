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
            this.CommandGetCategories = new RelayCommand2(GetCategoriesFromDatabase);
        }

        public RelayCommand2 CommandGetCategories { get; set; }

        private ObservableCollection<Category> _category;
        public ObservableCollection<Category> Category
        {
            get { return _category; }
            set
            {
                _category = value;
                OnPropertyChanged("Category");
            }
        }

        public void GetCategoriesFromDatabase()
        {
            try
            {
                using (var ctx = new ServerContext())
                {
                    ObservableCollection<Category> collection = new ObservableCollection<Category>(ctx.Category.ToList());
                    this.Category = collection;
                }
            }
            catch (System.Data.DataException)
            {
                MessageBox.Show("Kunne ikke oprette forbindelse til databasen. Tjek din konfiguration og internet adgang.", "Error!");
            }

        }
        /*
        private Product _selectedCategory;
        public Product SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                OnPropertyChanged("SelectedCategory");
            }
        }
        */

    }
}
