using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DanmissionManager.ViewModels
{
    class SettingsViewModel : BaseViewModel
    {
        private int selectedLanguage;

        public int Language
        {
            get { return selectedLanguage; }
            set
            {
                if (value != selectedLanguage)
                {
                    SetLanguageDictionary(value);
                    selectedLanguage = value;
                }
            }
        }
        
        private void SetLanguageDictionary(int value)
        {
            // First, make it work. Then, make it look up information dynamically on different computers. Find program path and then work in that way?

            ResourceDictionary languageDictionary = new ResourceDictionary();

            switch (value)
            {
                case 1: languageDictionary.Source = new Uri(@"C:\Users\Jonathan\Google Drive\University\Projects\P - 3\Programming\Danmission\DanmissionManager\Resources\StringResources.DK.xaml", UriKind.Relative);
                    break;
                case 2: languageDictionary.Source = new Uri(@"C:\Users\Jonathan\Google Drive\University\Projects\P - 3\Programming\Danmission\DanmissionManager\Resources\StringResources.EN-GB.xaml", UriKind.Relative);
                    break;
                default: throw new ArgumentException();
            }
        }
    }
}
