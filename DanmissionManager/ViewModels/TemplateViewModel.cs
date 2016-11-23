using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace DanmissionManager.ViewModels
{
    class TemplateViewModel : BaseViewModel
    {
        public TemplateViewModel()
        {
            this._buttonText = "ThisCameFromTheViewModel";
            this._input = String.Empty;


            List<Product> list = new List<Product>();

            Product product = new Product();
        }

        private string _buttonText;

        public string ButtonText
        {
            get
            {
                return _buttonText;
            }
            set
            {
                _buttonText = value;
                OnPropertyChanged("ButtonText");
            }
        }


        private string _input;
        public string Input
        {
            get
            {
                return _input;
            }
            set { _input = value; OnPropertyChanged("Input"); }
        }
        


    }
}

