using Foundation.ObjectHydrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DanmissionManager.ViewModels
{
    class MainViewModel : BaseViewModel
    {

        public MainViewModel()
        {
            ConsoleManager.Show();
            LoadSettings();

            Generator a = new Generator();
            

        }

        private void LoadSettings()
        {
            new SettingsViewModel();
        }
    }
}
