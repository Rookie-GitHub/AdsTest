using Common.EnumType;
using Connection;
using DTO;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Utility;

namespace ViewModel
{
    public class VM_WindowNon_AutoCutting : VM_BaseViewModel
    {
        #region 变量声明 - 双向绑定
        public string _TitleName = "";

        public string TitleName
        {
            get { return _TitleName; }
            set { _TitleName = value; OnPropertyChanged(); }
        }


        public string _Ip = "";

        public string Ip
        {
            get { return _Ip; }
            set { _Ip = value; OnPropertyChanged(); }
        }

        public string _Port = "6000";

        public string Port
        {
            get { return _Port; }
            set { _Port = value; OnPropertyChanged(); }
        }


        public string _ReceiveInfo = "海油管理系统客户端";
        public string ReceiveInfo
        {
            get { return _ReceiveInfo; }
            set { _ReceiveInfo = value; OnPropertyChanged(); }
        }


        public string _NowStep = "等待发送任务请求";
        public string NowStep
        {
            get { return _NowStep; }
            set { _NowStep = value; OnPropertyChanged(); }
        }

        public string _ConnectState = "连接未开启";
        public string ConnectState
        {
            get { return _ConnectState; }
            set { _ConnectState = value; OnPropertyChanged(); }
        }
        public string _ConnectText = "连接服务";
        public string ConnectText
        {
            get { return _ConnectText; }
            set { _ConnectText = value; OnPropertyChanged(); }
        }

        //声明BUTTON基类
        public Button<DeviceTypeViewModel> Button { get; set; }
        //Socket对象
        AsySocketClientHelper SocketClient;
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public VM_WindowNon_AutoCutting()
        {
            //Window window = Window.GetWindow(this);//关闭父窗体 获取当前窗体

            Button = new Button<DeviceTypeViewModel>();

            switch (Utility.Globle.CuttingOneOrTwo)
            {
                case 1:
                    TitleName = "一号切割机手动模式";
                    break;
                case 2:
                    TitleName = "二号切割机手动模式";
                    break;
                case 3:
                    TitleName = "三号切割机手动模式";
                    break;
                default:
                    break;
            }
        }

        #region Socket-Connect命令
        /// <summary>
        /// 
        /// </summary>
        public BaseCommand ConnectCommand
        {
            get
            {
                return new BaseCommand(ConnectCommand_Executed);
            }
        }

        void ConnectCommand_Executed(object obj)
        {
            if (SocketClient != null)
            {
                if (SocketClient.tcpClient != null)
                {
                    if (SocketClient.tcpClient.Connected)
                    {
                        MessageBox.Show("连接已在开启状态,请勿重复连接!");
                        return;
                    }
                }
            }

            var SocketState = false;
            var Times = 0;
            SocketClient = new AsySocketClientHelper(Ip, Convert.ToInt32(Port));

            while (!SocketState)
            {
                if (Times <= 5)
                {
                    if (SocketClient.AsynConnect())
                    {
                        SocketState = true;
                    }
                }
                else
                {
                    MessageBox.Show("连接超时,请确认网络正常后再次连接!");
                    return;
                }
                Times++;
            }
            Times = 0;
            ConnectState = "连接已开启成功!";
            ConnectText = "连接已开启";
            SocketClient.ClientReceiveDataCompleteEvent += ReceiveDataFromClient;
        }

        #endregion


        #region SocketSend命令
        /// <summary>
        /// 
        /// </summary>
        public BaseCommand ButtonCommand
        {
            get
            {
                return new BaseCommand(ButtonCommand_Executed);
            }
        }

        void ButtonCommand_Executed(object obj)
        {
            var CommandParameter = obj.ToString();
            switch (CommandParameter)
            {
                case "TaskInfo"://请求任务信息

                    break;

                case "InMaterial"://请求上料

                    break;

                case "ProcessInfo"://过程信息反馈

                    break;

                case "OutMaterial"://请求下料

                    break;

                default:

                    break;
            }

            MessageBox.Show(CommandParameter);
        }

        #endregion

        void ReceiveDataFromClient(string Data)
        {
            ReceiveInfo = Data;
            CuttingSendEntity entity = new CuttingSendEntity();
            bool Result = false;
            try
            {
                entity = Newtonsoft.Json.JsonConvert.DeserializeObject<CuttingSendEntity>(Data.ToString().Substring(64, Data.Length - 64));

                switch (entity.Header)
                {
                    case (int)EMachineAutoRunState.RequestNC://请求程序 -> 中控回复任务信息

                        NowStep = "";
                        break;
                    case (int)EMachineAutoRunState.ReceivedNCCode://收到任务 -> 中控回复收到 OK

                        NowStep = "";
                        break;
                    case (int)EMachineAutoRunState.RequestLoad://请求上料 -> 中控回复收到  OK

                        NowStep = "";
                        break;
                    case (int)EMachineAutoRunState.LoadFinish://上料完成 -> 中控回复上料完成 

                        NowStep = "";
                        break;
                    case (int)EMachineAutoRunState.StartNPA://开始寻边  -> 中控回复收到  OK

                        NowStep = "";
                        break;
                    case (int)EMachineAutoRunState.NPAFinish://寻边结束 -> 中控回复收到  OK

                        NowStep = "";
                        break;
                    case (int)EMachineAutoRunState.StartCut://开始切割  ->  中控回复收到 OK

                        NowStep = "";
                        break;
                    case (int)EMachineAutoRunState.CutFinish://切割结束 ->  中控回复收到 OK

                        NowStep = "";
                        break;
                    case (int)EMachineAutoRunState.RequestUnload://请求下料 ->  中控回复收到 OK

                        NowStep = "";
                        break;
                    case (int)EMachineAutoRunState.UnloadFinish://下料完成 ->  中控回复下料完成

                        NowStep = "";
                        break;
                    case (int)EMachineAutoRunState.Idle://空闲 ->  中控回复收到 OK

                        NowStep = "";
                        break;
                    case (int)EMachineAutoRunState.Error://错误 ->  中控回复收到 OK

                        NowStep = "";
                        break;
                    case (int)EMachineAutoRunState.Running://切割机处于运行模式 ->  中控回复收到 OK

                        NowStep = "";
                        break;
                    case (int)EMachineAutoRunState.OnLine://切割机心跳 ->  中控回复收到 OK

                        NowStep = "";
                        break;
                    case (int)EMachineAutoRunState.NoPlate://中控回复料库无钢板

                        NowStep = "";
                        break;
                    case (int)EMachineAutoRunState.NoJob://中控回复无任务

                        NowStep = "";
                        break;
                    case (int)EMachineAutoRunState.PauseModel://中控回复中控处于暂停

                        NowStep = "";
                        break;
                    case (int)EMachineAutoRunState.AutoModel://切割机处于单机模式 ->  中控回复收到 OK

                        NowStep = "";
                        break;
                    case (int)EMachineAutoRunState.SingleModel://切割机处于全自动模式 ->  中控回复收到 OK

                        NowStep = "";
                        break;
                    case (int)EMachineAutoRunState.MachineDisabled://机器处于被禁用状态 
                        NowStep = "";
                        break;
                    case (int)EMachineAutoRunState.NCProcessRatio://当前程序运行百分比

                        NowStep = "";
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

            }
        }

        #region 退出按钮命令
        /// <summary>
        /// 退出按钮
        /// </summary>
        public BaseCommand CloseCommand
        {
            get
            {
                return new BaseCommand(CloseCommand_Executed);
            }
        }

        void CloseCommand_Executed(object obj)
        {
            //将连接的Socket 关闭
            if (SocketClient != null)
            {
                if (SocketClient.tcpClient != null)
                {
                    if (SocketClient.tcpClient.Connected)
                    {
                        SocketClient.CloseSocket();
                    }
                }
            }
            this.Close();
        }

        #endregion
    }
}
