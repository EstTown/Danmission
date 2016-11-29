using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DanmissionManager.ViewModels
{
    class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel()
        {
            List<string> languages = new List<string>
            {
                "Dansk",
                "English"
            };
            _languages = new ObservableCollection<string>(languages);
            SelectedItem = Languages.First();
        }
        private ObservableCollection<string> _languages;
        public ObservableCollection<string> Languages
        {
            get { return _languages; }
            set { _languages = value; OnPropertyChanged("Language"); }
        }

        private string selectedItem = string.Empty;
        public string SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; setProgramLanguage(value); OnPropertyChanged("SelectedItem"); }
        }

        public void setProgramLanguage(string LanguageName)
        {
            string langDictPath;

            switch (LanguageName)
            {
                case "Dansk":
                    langDictPath = "/Resources/StringResources.DK.xaml";
                    break;
                case "English":
                    langDictPath = "/Resources/StringResources.en-GB.xaml";
                    break;

                default:
                    throw new ArgumentException("Choosen language does not fit available language choices");
            }

            Uri langDictUri = new Uri(langDictPath, UriKind.Relative);
            ResourceDictionary langDict = Application.LoadComponent(langDictUri) as ResourceDictionary;
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(langDict);
        }

        //private void SetLanguageDictionary(int value)
        //{
        //    // First, make it work. Then, make it look up information dynamically on different computers. Find program path and then work in that way?

        //    ResourceDictionary languageDictionary = new ResourceDictionary();

        //    switch (value)
        //    {
        //        case 1: languageDictionary.Source = new Uri(@"C:\Users\Jonathan\Google Drive\University\Projects\P - 3\Programming\Danmission\DanmissionManager\Resources\StringResources.DK.xaml", UriKind.Relative);
        //            break;
        //        case 2: languageDictionary.Source = new Uri(@"C:\Users\Jonathan\Google Drive\University\Projects\P - 3\Programming\Danmission\DanmissionManager\Resources\StringResources.EN-GB.xaml", UriKind.Relative);
        //            break;
        //        default: throw new ArgumentException();
        //    }
        //}
    }
}
