using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using TwinCAT.Ads;

namespace ViewModel
{
    public class VM_MainWindow : VM_BaseViewModel
    {
        private static TcAdsClient tcClient;

        private AdsStream dataStream;

        int Upi;

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

        private string _PlcValue;

        public string PlcValue
        {
            get { return _PlcValue; }
            set { _PlcValue = value; OnPropertyChanged(); }
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
                IP = "172.18.212.30.1.1";
                Port = 851;
                tcClient.Connect(IP, Port);

                Upi = tcClient.AddDeviceNotification("GVL_WCS.C101_ToWCS.UPI1", dataStream, AdsTransMode.OnChange, 100, 0, null);

                tcClient.AdsNotification += new AdsNotificationEventHandler(tcClient_OnNotification);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Cannot connect with PLC(ConnPLC)");
            }
        }

        public void tcClient_OnNotification(object sender, AdsNotificationEventArgs e)
        {
            try
            {
                e.DataStream.Position = e.Offset;

                #region plcUpi
                if (e.NotificationHandle == Upi)
                {
                    var hvar1 = tcClient.CreateVariableHandle("GVL_WCS.C101_ToWCS.UPI1");
                    PlcValue = tcClient.ReadAny(hvar1, typeof(string), new int[] { 20 }).ToString();
                }  
                #endregion

                PlcValue += PlcValue + "/-/-/";
            }
            catch (Exception ex)
            {

            }
        }
    }
}
