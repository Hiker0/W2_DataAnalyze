
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;

using U32 = System.UInt32;
using U8 = System.Byte;
using U16 = System.UInt16;
using S32 = System.Int32;
using System.IO;
using System.Collections;

namespace W2_DataAnalyze
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>

    public partial class MainWindow : Window
    {

        const U32 PLAN_ID_LENGTH = 12;
        const U32 MAX_PLAN_NAME_LENGTH = 16;
        const U32 MAX_AIM_NUM = 20;
        const U32 MAX_PLAN_NUM = 20;

        Anylazer fileAnylazer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void textChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void dataTypeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combox = sender as ComboBox;
            //combox.SelectedIndex;
            Debug.WriteLine($"Select index = {combox.SelectedIndex}");

            switch (combox.SelectedIndex)
            {
                case 0:
                    fileAnylazer = new HeartRateAnylazer();;
                    break;
                case 1:
                    fileAnylazer = new DailySleepAnylazer();
                    break;
                case 2:
                    fileAnylazer = new SleepAnylazer();
                    break;
                case 3:
                    fileAnylazer = new PlanAnylazer();
                    break;
                case 4:
                    fileAnylazer = new PlanRetAnylazer();
                    break;
                case 5:
                    fileAnylazer = new StatisticAnylazer();
                    break;
                case 6:
                    fileAnylazer = new DailyStepAnylazer();
                    break;
            }

            if (this.fielpath != null && this.fielpath.Text.Length > 0)
            {
                this.result.Text = fileAnylazer.anylzeFile(this.fielpath.Text);
            }
        }

        private void fileSelectClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Excel Files (*)|*"
            };
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                Debug.WriteLine($"Select file = {openFileDialog.FileName}");
                //this.tbErrorCodeTableFile.Text = openFileDialog.FileName;
                this.fielpath.Text = openFileDialog.FileName;
                if (fileAnylazer != null)
                {
                    this.result.Text = fileAnylazer.anylzeFile(this.fielpath.Text);
                }
            }
        }

        void analyzeFile(int index, String filePath)
        {
            Debug.WriteLine("analyzeFile");
            if (filePath == null || filePath.Length == 0)
            {
                Debug.WriteLine("filePath is none");
                return;
            }
            switch (index)
            {
                case 0:
                    this.result.Text = readHeartRateData(filePath);
                    break;
                case 1:
                    this.result.Text = readDailySleepData(filePath);
                    break;
                case 2:
                    this.result.Text = readSleepData(filePath);
                    break;
                case 5:
                    this.result.Text = readStatistic(filePath);
                    break;
            }
            
        }


        public static DateTime GetDateTimeFrom1970Ticks(long curSeconds)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddSeconds(curSeconds);
        }

        String readHeartRateData(String filePath)
        {
            Debug.WriteLine("readCommonData start >> \n");
            StringBuilder infoBuilder = new StringBuilder();

            return infoBuilder.ToString();
        }

        String readSleepData(String filePath)
        {
            Debug.WriteLine("readSleepData");
            StringBuilder infoBuilder = new StringBuilder();
            return infoBuilder.ToString();
        }


        String readDailySleepData(String filePath)
        {
            Debug.WriteLine("read statistic");
            StringBuilder infoBuilder = new StringBuilder();
            return infoBuilder.ToString();
        }

        String readStatistic(String filePath)
        {
            Debug.WriteLine("read statistic");
            StringBuilder infoBuilder = new StringBuilder();
         
            return infoBuilder.ToString();
        }
    }
}
