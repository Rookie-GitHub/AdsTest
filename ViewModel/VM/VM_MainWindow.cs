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

        int UpiDataOk;

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

        private string _PlcDataOkValue;

        public string PlcDataOkValue
        {
            get { return _PlcDataOkValue; }
            set { _PlcDataOkValue = value; OnPropertyChanged(); }
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
                IP = "172.18.226.186.1.1";
                Port = 851;
                tcClient.Connect(IP, Port);

                Upi = tcClient.AddDeviceNotification("GVL_WCS.C47_FrWCS.UPI", dataStream, AdsTransMode.OnChange, 100, 0, null);

                UpiDataOk = tcClient.AddDeviceNotification("GVL_WCS.C47_ToWCS.UPI_OK", dataStream, AdsTransMode.OnChange, 100, 0, null);

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
                    var hvar1 = tcClient.CreateVariableHandle("GVL_WCS.C47_FrWCS.UPI");
                    var UpiValue = tcClient.ReadAny(hvar1, typeof(string), new int[] { 20 }).ToString();
                    PlcValue += UpiValue + "/-/-/";
                }
                #endregion

                #region UpiDataOk
                if (e.NotificationHandle == UpiDataOk)
                {
                    var hvar2 = tcClient.CreateVariableHandle("GVL_WCS.C47_ToWCS.UPI_OK");
                    var UpiDataOkValue = (bool)(tcClient.ReadAny(hvar2, typeof(bool)));
                    PlcDataOkValue += UpiDataOkValue + "/-/-/";
                }
                #endregion
            }
            catch (Exception ex)
            {

            }
        }

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
                case "Upi":
                    PlcValue = string.Empty;
                    break;
                case "DataOk":
                    PlcDataOkValue = string.Empty;
                    break;
                default:
                    break;
            }
        }

    }
}
