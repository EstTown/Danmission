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

            this.SearchParameter = string.Empty;
            this.CommandGetProducts = new RelayCommand2(GetProductsFromDatabase);
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

        private Collection<Product> _products;

        public Collection<Product> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                OnPropertyChanged("Products");
            }
        }

        //property that will contain the command/method and executes it
        //does not need a backing field
        public RelayCommand2 CommandGetProducts { get; set; }
        //method get products
        public void GetProductsFromDatabase()
        {
            using (var ctx = new ServerContext())
            {

                //List<Product> list = ctx.Products.Where(x => x.name.ToLower().CompareTo(SearchParameter.ToLower()) == 0).ToList();

                //This is more dyniamic, although it runs smoothly, the initial query seems to lag, causing a small stutter
                List<Product> list = ctx.Products.Where(x => x.name.ToLower().Contains(SearchParameter.ToLower())).ToList();

                ObservableCollection<Product> collection = new ObservableCollection<Product>(list);

                
                this.Products = collection;
            }

        }

    }
}

