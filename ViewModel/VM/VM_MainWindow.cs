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
using System.Configuration;
using Untility;

namespace ViewModel
{
    public class VM_MainWindow : VM_BaseViewModel
    {

        private CommonHandle commonHandle;

        #region 变量声明

        private static TcAdsClient tcClient;

        private AdsStream dataStream;

        int parameterOne;

        string TypeOneValue;

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

        private string _parameterOneValue;

        public string ParameterOneValue
        {
            get { return _parameterOneValue; }
            set { _parameterOneValue = value; OnPropertyChanged(); }
        }

        List<string> PlcParas;

        List<int> AdsParas;
        #endregion

        /// <summary>
        /// 构造函数 constructor
        /// </summary>
        public VM_MainWindow()
        {
            commonHandle = new CommonHandle();
            commonHandle.RemoveKeys("appSetting", "add");
            dataStream = new AdsStream(15);
        }

        #region Command

        #region ConnectCommand
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
                IP = "172.18.226.186.1.1";
                Port = 851;
                tcClient.Connect(IP, Port);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Cannot connect with PLC(ConnPLC)");
            }
        }

        #endregion

        #region MonitorOpenCommand
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

            List<ComboBox> ComboBox_list = this.FindVisualChildren<ComboBox>(parent);

            TypeOneValue = ComboBox_list[0].Text;

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


            StartMonitoring();
        }
        #endregion

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

        #region ClearCommand
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
                default:
                    break;
            }
        }
        #endregion

        #region AddPlcParaCommand
        /// <summary>
        /// Mian Page Clear Button Click Event
        /// 
        /// Add a new PlcPare for Monitor
        /// </summary>
        public BaseCommand AddPlcParaCommand
        {
            get
            {
                return new BaseCommand(AddPlcParaCommand_Executed);
            }
        }

        void AddPlcParaCommand_Executed(Object Parameter)
        {
            if (string.IsNullOrWhiteSpace(ParaOne))
            {
                MessageBox.Show("Please fill in the parameter");
                return;
            }

            Button CurrentButton = (Button)Parameter;

            string Param = CurrentButton.Name;

            DependencyObject parent = VisualTreeHelper.GetParent(CurrentButton);
            List<Button> list = this.FindVisualChildren<Button>(parent);

            List<ComboBox> ComboBox_list = this.FindVisualChildren<ComboBox>(parent);

            TypeOneValue = ComboBox_list[0].Text;

            if (string.IsNullOrWhiteSpace(TypeOneValue))
            {
                MessageBox.Show("Please choose the type of parameter");
                return;
            }

            Button RelatedButton = new Button();

            var RelatedButtonName = "Monitoring_Open";

            foreach (var item in list)
            {
                if (item.Name == RelatedButtonName)
                {
                    RelatedButton = item;
                }
            }

            RelatedButton.IsEnabled = true;

            commonHandle.AddConfigKey(ParaOne, TypeOneValue);
        }
        #endregion

        #endregion


        public void StartMonitoring()
        {
            parameterOne = tcClient.AddDeviceNotification(ParaOne, dataStream, AdsTransMode.OnChange, 100, 0, null);

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
                    var hvar1 = tcClient.CreateVariableHandle(ParaOne);

                    var Value = string.Empty;
                    switch (TypeOneValue)
                    {
                        case "String":
                            Value = tcClient.ReadAny(hvar1, typeof(string), new int[] { 20 }).ToString();
                            break;
                        case "Bool":
                            Value = ((bool)(tcClient.ReadAny(hvar1, typeof(bool)))).ToString();
                            break;
                        case "Int":
                            Value = ((short)(tcClient.ReadAny(hvar1, typeof(short)))).ToString();
                            break;
                        case "Real":
                            Value = ((float)(tcClient.ReadAny(hvar1, typeof(float)))).ToString();
                            break;
                        default:
                            break;
                    }

                    ParameterOneValue += Value + "/-/-/";
                }
                #endregion
            }
            catch (Exception ex)
            {

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
