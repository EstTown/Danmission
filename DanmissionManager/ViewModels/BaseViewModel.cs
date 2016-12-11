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
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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

        public BaseViewModel()
        {
            PopupService = new Popups((x, y) => { }, (x, y) => true);
        }

        public BaseViewModel(Popups popupService)
        {
            PopupService = popupService;
        }
    }
}
