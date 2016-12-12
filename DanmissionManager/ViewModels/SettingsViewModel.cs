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
        public SettingsViewModel(Popups popupService) : base(popupService)
        {
            SetupLanguages();
            SelectLanguage();
            DaysToProductExpiration = Properties.Settings.Default.DAYSTOEXPIRATION;
        }

        private void SelectLanguage()
        {
            try
            {
                SelectedItem = Properties.Settings.Default.LANGUAGE;
                // Try if the language in the application settings is valid.
            }
            catch (Exception)
            {
                // Anything goes wrong, we just choose the first as the default language
                SelectedItem = Languages.FirstOrDefault();
            }
        }

        private void SetupLanguages()
        {
            List<string> languages = new List<string>
            {
                "Dansk",
                "English"
            };
            _languages = new ObservableCollection<string>(languages);
        }

        private ObservableCollection<string> _languages;
        public ObservableCollection<string> Languages
        {
            get { return _languages; }
            set { _languages = value; OnPropertyChanged("Language"); }
        }

        private static string _selectedItem = string.Empty;
        public string SelectedItem
        {
            get { return _selectedItem; }
            set { setProgramLanguage(value); _selectedItem = value; SaveLanguageInSettings(value); OnPropertyChanged("SelectedItem"); }
        }

        private void SaveLanguageInSettings(string language)
        {
            Properties.Settings.Default.LANGUAGE = language;
        }

        public void setProgramLanguage(string LanguageName)
        {
            try
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
            catch (ArgumentException)
            {
                PopupService.PopupMessage(Application.Current.FindResource("OCouldNotFindLanguage").ToString(), Application.Current.FindResource("Error").ToString());
            }
        }

        private int _daysToProductExpiration;
        public int DaysToProductExpiration
        {
            get { return _daysToProductExpiration; }
            set { _daysToProductExpiration = value; SaveDaysInSettings(value); }
        }

        private void SaveDaysInSettings(int days)
        {
            Properties.Settings.Default.DAYSTOEXPIRATION = days;
        }
    }
}
