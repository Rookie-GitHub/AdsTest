using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TwinCAT.Ads;

namespace ViewModel
{
    public class VM_MainWindow : VM_BaseViewModel
    {
        private static TcAdsClient tcClient;

        private AdsStream dataStream;

        int parameterOne;

        int parameterTwo;

        private string _IP;
        public string IP
        {
            get { return _IP; }
            set { _IP = value; OnPropertyChanged(); }
        }

        private int _Port;

        public int Port
        {
            get { return _Port; }
            set { _Port = value; OnPropertyChanged(); }
        }

        private string _ParaOne;
        public string ParaOne
        {
            get { return _ParaOne; }
            set { _ParaOne = value; OnPropertyChanged(); }
        }
        private string _ParaTwo;

        public string ParaTwo
        {
            get { return _ParaTwo; }
            set { _ParaTwo = value; OnPropertyChanged(); }
        }

        private string _parameterOneValue;

        public string ParameterOneValue
        {
            get { return _parameterOneValue; }
            set { _parameterOneValue = value; OnPropertyChanged(); }
        }

        private string _parameterTwoValue;

        public string ParameterTwoValue
        {
            get { return _parameterTwoValue; }
            set { _parameterTwoValue = value; OnPropertyChanged(); }
        }

        public VM_MainWindow()
        {

            dataStream = new AdsStream(15);
        }

        /// <summary>
        /// Mian Page Connect Button Click Event
        /// </summary>
        public BaseCommand ConnectCommand
        {
            get
            {
                return new BaseCommand(ConnectCommand_Executed);
            }
        }

        void ConnectCommand_Executed(Object Parameter)
        {
            tcClient = new TcAdsClient();
            try
            {
                //IP = "172.18.226.186.1.1";
                //Port = 851;
                tcClient.Connect(IP, Port);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Cannot connect with PLC(ConnPLC)");
            }
        }


        public BaseCommand MonitorOpenCommand
        {
            get
            {
                return new BaseCommand(MonitorCommand_Executed);
            }
        }

        void MonitorCommand_Executed(Object Parameter)
        {
            #region 处理页面控件联动

            Button CurrentButton = (Button)Parameter;

            string Param = CurrentButton.Name;

            CurrentButton.IsEnabled = false;

            DependencyObject parent = VisualTreeHelper.GetParent(CurrentButton);
            List<Button> list = this.FindVisualChildren<Button>(parent);

            Button RelatedButton = new Button();

            var RelatedButtonName = Param.Split('_')[0].ToString() + "_Close";

            foreach (var item in list)
            {
                if (item.Name == RelatedButtonName)
                {
                    RelatedButton = item;
                }
            }

            RelatedButton.IsEnabled = true;
            #endregion


            //StartMonitoring();

        }

        public void StartMonitoring()
        {

            parameterOne = tcClient.AddDeviceNotification(ParaOne, dataStream, AdsTransMode.OnChange, 100, 0, null);

            parameterTwo = tcClient.AddDeviceNotification(ParaTwo, dataStream, AdsTransMode.OnChange, 100, 0, null);

            tcClient.AdsNotification += new AdsNotificationEventHandler(tcClient_OnNotification);
        }


        public void tcClient_OnNotification(object sender, AdsNotificationEventArgs e)
        {
            try
            {
                e.DataStream.Position = e.Offset;

                #region plcUpi
                if (e.NotificationHandle == parameterOne)
                {
                    var hvar1 = tcClient.CreateVariableHandle("GVL_WCS.C47_FrWCS.UPI");
                    var UpiValue = tcClient.ReadAny(hvar1, typeof(string), new int[] { 20 }).ToString();
                    ParameterOneValue += UpiValue + "/-/-/";
                }
                #endregion

                #region UpiDataOk
                if (e.NotificationHandle == parameterTwo)
                {
                    var hvar2 = tcClient.CreateVariableHandle("GVL_WCS.C47_ToWCS.UPI_OK");
                    var UpiDataOkValue = (bool)(tcClient.ReadAny(hvar2, typeof(bool)));
                    ParameterTwoValue += UpiDataOkValue + "/-/-/";
                }
                #endregion
            }
            catch (Exception ex)
            {

            }
        }

        #region Close The Monitoring
        public BaseCommand MonitorCloseCommand
        {
            get
            {
                return new BaseCommand(MonitorClose_Executed);
            }
        }

        public void MonitorClose_Executed(object obj)
        {
            Button CurrentButton = (Button)obj;

            string Param = CurrentButton.Name;
            CurrentButton.IsEnabled = false;

            DependencyObject parent = VisualTreeHelper.GetParent(CurrentButton);
            List<Button> list = this.FindVisualChildren<Button>(parent);

            Button RelatedButton = new Button();

            var RelatedButtonName = Param.Split('_')[0].ToString() + "_Open";

            foreach (var item in list)
            {
                if (item.Name == RelatedButtonName)
                {
                    RelatedButton = item;
                }
            }

            RelatedButton.IsEnabled = true;
        }
        #endregion

        /// <summary>
        /// Mian Page Clear Button Click Event
        /// </summary>
        public BaseCommand ClearCommand
        {
            get
            {
                return new BaseCommand(ClearCommand_Executed);
            }
        }

        void ClearCommand_Executed(Object Parameter)
        {
            switch (Parameter.ToString())
            {
                case "ParameterOneValue":
                    ParameterOneValue = string.Empty;
                    break;
                case "ParameterTwoValue":
                    ParameterTwoValue = string.Empty;
                    break;
                default:
                    break;
            }
        }


        #region 查找页面某部分子标签
        private List<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            List<T> list = new List<T>();
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        list.Add((T)child);
                    }

                    List<T> childItems = FindVisualChildren<T>(child); // 递归
                    if (childItems != null && childItems.Count() > 0)
                    {
                        foreach (var item in childItems)
                        {
                            list.Add(item);
                        }
                    }
                }
            }
            return list;
        }
        #endregion
    }
}
