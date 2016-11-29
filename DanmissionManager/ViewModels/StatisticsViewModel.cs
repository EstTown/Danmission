using DanmissionManager.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.DataVisualization.Charting;

namespace DanmissionManager.ViewModels
{
    class StatisticsViewModel : BaseViewModel
    {
        public StatisticsViewModel()
        {
            showChart();
            //this.command = new RelayCommand2(showChart);
            //this.command = command;
        }

        private void showChart()
        {
            List<KeyValuePair<string, int>> MyValue = new List<KeyValuePair<string, int>>();
            MyValue.Add(new KeyValuePair<string, int>("Administration", 20));
            MyValue.Add(new KeyValuePair<string, int>("Management", 36));
            MyValue.Add(new KeyValuePair<string, int>("Development", 89));
            MyValue.Add(new KeyValuePair<string, int>("Support", 270));
            MyValue.Add(new KeyValuePair<string, int>("Sales", 140));

            PieChart = MyValue;
        }

        public List<KeyValuePair<string, int>> PieChart { get; set; }

        public RelayCommand2 command { get; set; }
    }
}
