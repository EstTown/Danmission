using DanmissionManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DanmissionManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs args)
        {
            // messagebox
            var popup = (Action<string, string>)((msg, capt) => MessageBox.Show(msg, capt));

            // confirm box
            var confirm = (Func<string, string, bool>)((msg, capt) =>
                MessageBox.Show(msg, capt, MessageBoxButton.YesNo) == MessageBoxResult.Yes);

            Views.MainView view = new Views.MainView();
            ((BaseViewModel)view.DataContext).PopupService = new BaseViewModel.Popups(popup, confirm);
            view.Show();
        }
    }
}
