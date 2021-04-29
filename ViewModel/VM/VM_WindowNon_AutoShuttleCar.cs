using DTO;
using Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Utility;
namespace ViewModel
{
    public class VM_WindowNon_AutoShuttleCar : VM_BaseViewModel
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

        /// <summary>
        /// 机架号
        /// </summary>
        public string _Rack = "0";
        public string Rack
        {
            get { return _Rack; }
            set { _Rack = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 0:1200/1500  2:300/400
        /// </summary>
        public string _Slot = "0";
        public string Slot
        {
            get { return _Slot; }
            set { _Slot = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 连接状态
        /// </summary>
        public bool _ConnectState = false;
        public bool ConnectState
        {
            get { return _ConnectState; }
            set { _ConnectState = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string _ConnectText = "连接未开启";
        public string ConnectText
        {
            get { return _ConnectText; }
            set { _ConnectText = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Plc类型
        /// </summary>
        public string _PlcType = "S71200";
        public string PlcType
        {
            get { return _PlcType; }
            set { _PlcType = value; OnPropertyChanged(); }
        }

        private string _ActionType;

        public string ActionType
        {
            get { return _ActionType; }
            set { _ActionType = value; }
        }
        /// <summary>
        /// 起始位置
        /// </summary>
        private int _StartPosition;

        public int StartPosition
        {
            get { return _StartPosition; }
            set { _StartPosition = value; }
        }
        /// <summary>
        /// 目标位置
        /// </summary>
        private int _TargetPosition;

        public int TargetPosition
        {
            get { return _TargetPosition; }
            set { _TargetPosition = value; }
        }
        public ComboBox<ComBoBoxViewModel> ComboBox_Action { get; set; }
        public ComboBox<ComBoBoxViewModel> ComboBox_StartPosition { get; set; }
        public ComboBox<ComBoBoxViewModel> ComboBox_TargetPosition { get; set; }
        public ComboBox<ComBoBoxViewModel> ComboBox_PlcType { get; set; }

        S7Helper S7;
        #endregion

        public VM_WindowNon_AutoShuttleCar()
        {
            switch (Utility.Globle.ShuttleCarOneOrTwo)
            {
                case 1:
                    TitleName = "一号RGV手动模式";
                    break;
                case 2:
                    TitleName = "二号RGV手动模式";
                    break;
                default:
                    break;
            }

            ComboBox_Action = new ComboBox<ComBoBoxViewModel>();
            ComboBox_StartPosition = new ComboBox<ComBoBoxViewModel>();
            ComboBox_TargetPosition = new ComboBox<ComBoBoxViewModel>();
            ComboBox_PlcType = new ComboBox<ComBoBoxViewModel>();
            LoadComboBox();
        }

        #region 加载Combobox内容
        /// <summary>
        /// 加载ComboBox内容
        /// 
        /// </summary>
        public void LoadComboBox()
        {
            CommonHandle handle = new CommonHandle();

            List<ComBoBoxViewModel> TypeSource = new List<ComBoBoxViewModel>();

            TypeSource = handle.FillInComboBox("ActionType");

            ComboBox_Action.SetItemsSource(TypeSource);

            ActionType = TypeSource[0].ActionType;

            ComboBox_Action.SelectCallBack = (Type) =>
            {
                if (Type == null)
                    return;
                ActionType = Type.ActionType;
            };


            TypeSource = handle.FillInComboBox("Position");

            ComboBox_StartPosition.SetItemsSource(TypeSource);

            ComboBox_StartPosition.SelectCallBack = (Position) =>
            {
                if (Position == null)
                    return;
                StartPosition = Position.StartPosition;
            };

            ComboBox_TargetPosition.SetItemsSource(TypeSource);

            ComboBox_TargetPosition.SelectCallBack = (Position) =>
            {
                if (Position == null)
                    return;
                TargetPosition = Position.TargetPosition;
            };

            TypeSource = handle.FillInComboBox("PlcType");

            ComboBox_PlcType.SetItemsSource(TypeSource);

            ComboBox_PlcType.SelectCallBack = (Plc_Type) =>
            {
                if (Plc_Type == null)
                    return;
                PlcType = Plc_Type.PlcType;
            };

            StartPosition = TypeSource[0].StartPosition;
            TargetPosition = TypeSource[0].TargetPosition;
            PlcType = TypeSource[0].PlcType;
        }
        #endregion

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
            this.Close();
        }
        #endregion

        #region Opc-Connect命令
        /// <summary>
        /// 
        /// </summary>
        public BaseCommand OpcCommand
        {
            get
            {
                return new BaseCommand(OpcCommand_Executed);
            }
        }

        void OpcCommand_Executed(object obj)
        {
            if (ConnectState)
            {
                MessageBox.Show("服务已成功开始,请勿重复连接");
                return;
            }

            //创建连接对象
            S7 = new S7Helper();

            var code = S7.Connect(Ip, Rack, Slot, PlcType);

            if (code.ToString().ToUpper().Contains("NOERROR"))
            {
                ConnectState = true;
                ConnectText = "连接已开启";
            }
            else
            {
                ConnectState = false;
                ConnectText = "连接开启失败,请确定网络正常后再次连接";
            }
        }
        #endregion

        #region 执行按钮命令
        /// <summary>
        /// 执行按钮
        /// </summary>
        public BaseCommand ActionCommand
        {
            get
            {
                return new BaseCommand(ActionCommand_Executed);
            }
        }

        void ActionCommand_Executed(object obj)
        {
            if (!ConnectState)
            {
                MessageBox.Show("连接未开启,请开启服务后再进行操作!");
                return;
            }

            //发送起始位置
            S7.WriteValue("DBAddress", "Int", StartPosition.ToString(), 0);
            //发送目标位置
            S7.WriteValue("DBAddress", "Int", TargetPosition.ToString(), 0);
            //发送动作类型  1:移动 2:搬运
            S7.WriteValue("DBAddress", "Int", ActionType, 0);
        }
        #endregion
    }
}
