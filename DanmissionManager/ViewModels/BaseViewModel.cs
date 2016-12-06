using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanmissionManager.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged //, INotifyDataErrorInfo
    {
        //InotifyDataErrorInfo implementatation
        /*
        public bool HasErrors
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public IEnumerable GetErrors(string propertyName)
        {
            throw new NotImplementedException();
        }
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        */

        //INotifyPropertyChanged implementation
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
    }
}
