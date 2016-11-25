using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DanmissionManager.Commands;

namespace DanmissionManager.ViewModels
{
    class FindProductViewModel : BaseViewModel
    {
        public FindProductViewModel()
        {
            this.CommandGetProducts = new RelayCommand2(GetProductsFromDatabase);
        }


        private string _searchParameter;

        public string SearchParameter
        {
            get
            {
                return _searchParameter;
            }
            set
            {
                _searchParameter = value;
                OnPropertyChanged("SearchParameter");
            }
        }

        //search function


        private Collection<Product> Products { get; set; }

        //property that will contain the command/method and executes it
        //does not need a backing field
        public RelayCommand2 CommandGetProducts { get; set; }
        //method get products
        public void GetProductsFromDatabase()
        {
            using (var ctx = new ServerContext())
            {
                ObservableCollection<Product> collection = new ObservableCollection<Product>(ctx.Products.ToList());
                this.Products = collection;
            }
            
        }

    }
}
