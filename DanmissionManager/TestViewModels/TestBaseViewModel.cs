using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DanmissionManager.TestViewModels
{
    public class TestBaseViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Implementation of INotifyDataErrorInfo + more

        //contains all errors, for a given property, where the property is the key accessor
        private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !_errors.ContainsKey(propertyName))
            {
                return null;
            }
            return _errors[propertyName];
        }
        public bool HasErrors
        {
            get { return _errors.Count > 0; }
        }

        //error message defined as constant string
        private const string NAME_ERROR = "THIS SHOULD NOT BE NAME";
        private const string NAME_WARNING = "CAREFUL WITH DAT NAME THO";

        //basically need a bunch of methods, that validate a certain property each.
        //for example a method that validates a name property
        public bool IsNamevalid(string value)
        {
            bool isValid = true;

            if (value.Contains(" "))
            {
                AddError("Name", NAME_ERROR, false);
                isValid = false;
            }
            else
            {
                RemoveError("Name", NAME_ERROR);
            }

            if (value.Length > 5)
            {
                AddError("Name", NAME_WARNING, false);
            }
            else
            {
                RemoveError("Name", NAME_WARNING);
            }
            return isValid;
        }
        //method for raising new event
        public void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        public void AddError(string propertyName, string error, bool isWarning)
        {
            if (!_errors.ContainsKey(propertyName))
            {
                _errors[propertyName] = new List<string>();
            }
            if (!_errors[propertyName].Contains(error))
            {
                if (isWarning)
                {
                    _errors[propertyName].Add(error);
                }
                else
                {
                    _errors[propertyName].Insert(0, error);
                    RaiseErrorsChanged(propertyName);
                }
            }
        }
        public void RemoveError(string propertyName, string error)
        {
            if (_errors.ContainsKey(propertyName) && _errors[propertyName].Contains(error))
            {
                _errors[propertyName].Remove(error);
                if (_errors[propertyName].Count == 0)
                {
                    _errors.Remove(propertyName);
                    RaiseErrorsChanged(propertyName);
                }
            }
        }
        #endregion
    }
}
