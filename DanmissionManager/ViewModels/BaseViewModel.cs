using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DanmissionManager.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public BaseViewModel()
        {
            PopupService = new Popups((x, y) => { }, (x, y) => true);
        }
        public BaseViewModel(Popups popupService)
        {
            PopupService = popupService;
        }

        #region Implementation of INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Implementation of INotifyDataErrorInfo

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

        //error message defined as a constant
        private const string NAME_ERROR = "Dette felt må ikke være tomt.";
        private const string AMOUNTOFPRODUCTS_ERROR = "Angiv mindst 2 - Eller sæt produktet til at være unikt.";
        private const string WEEKS_ERROR = "Skal være et posivitivt tal.";
        private const string PRICE_ERROR = "Pris skal være et positivt tal.";

        //basically need a bunch of methods, that validate a certain property each.
        //for example a method that validates a name property
        public bool IsNameValid(string value, string propertyName)
        {
            bool isValid = true;

            if (value == "")
            {
                AddError(propertyName, NAME_ERROR, false);
                isValid = false;
            }
            else
            {
                RemoveError(propertyName, NAME_ERROR);
            }
            return isValid;
        }

        public bool IsAmountOfProductsValid(int? value, string propertyName)
        {
            bool isValid = true;
            if (value == null)
            {
                AddError(propertyName, AMOUNTOFPRODUCTS_ERROR, false);
                isValid = false;
            }
            else
            {
                RemoveError(propertyName, AMOUNTOFPRODUCTS_ERROR);
                isValid = true;
            }
            return isValid;
        }

        public bool IsWeeksValid(int value)
        {
            bool isValid = true;
            if (value < 0)
            {
                AddError("Weeks", WEEKS_ERROR, false);
                isValid = false;
            }
            else
            {
                RemoveError("Weeks", WEEKS_ERROR);
            }
            return isValid;
        }

        public bool IsPriceValid(double value, string propertyName)
        {
            bool isValid = true;
            if (value < 0.0)
            {
                AddError(propertyName, PRICE_ERROR, false);
                isValid = false;
            }
            else
            {
                RemoveError(propertyName, PRICE_ERROR);
            }
            return isValid;
        }

        public void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        //add error to dictionary, with propertyname as key
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
                    
                }
            }
            RaiseErrorsChanged(propertyName);
        }

        public void RemoveError(string propertyName, string error)
        {
            if (_errors.ContainsKey(propertyName) && _errors[propertyName].Contains(error))
            {
                _errors[propertyName].Remove(error);
                if (_errors[propertyName].Count == 0)
                {
                    _errors.Remove(propertyName);
                    
                }
            }
            RaiseErrorsChanged(propertyName);
        }

        #endregion

        #region Popup properties and methods

        public struct Popups
        {
            public Action<string, string> PopupMessage;
            public Func<string, string, bool> PopupConfirm;
            public Popups(Action<string, string> popupMessage, Func<string, string, bool> popupConfirm)
            {
                PopupMessage = popupMessage;
                PopupConfirm = popupConfirm;
            }
        }
        
        public Popups PopupService { get; set; }

        #endregion     
    }
}
