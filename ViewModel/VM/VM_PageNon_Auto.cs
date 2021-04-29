using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utility;


namespace ViewModel
{
    public class VM_PageNon_Auto : VM_BaseViewModel
    {

        #region 变量声明 - 双向绑定
        /// <summary>
        /// 行车任务号
        /// </summary>
        public string _CraneTaskId = "行车任务号ID";

        public string CraneTaskId
        {
            get { return _CraneTaskId; }
            set { _CraneTaskId = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 穿梭车任务号
        /// </summary>
        public string _ShuttleTaskId = "穿梭车任务号ID";

        public string ShuttleTaskId
        {
            get { return _ShuttleTaskId; }
            set { _ShuttleTaskId = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 切割机任务号
        /// </summary>
        public string _CuttingTaskId = "切割机任务号ID";

        public string CuttingTaskId
        {
            get { return _CuttingTaskId; }
            set { _CuttingTaskId = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 桁车钢板编号
        /// </summary>
        private string _CraneBarCode = "桁车钢板编号";

        public string CraneBarCode
        {
            get { return _CraneBarCode; }
            set { _CraneBarCode = value; }
        }

        /// <summary>
        /// 穿梭车钢板编号
        /// </summary>
        private string _ShuttleBarCode = "穿梭车钢板编号";

        public string ShuttleBarCode
        {
            get { return _ShuttleBarCode; }
            set { _ShuttleBarCode = value; }
        }
        /// <summary>
        /// 切割机钢板编号
        /// </summary>
        private string _CuttingBarCode = "切割机钢板编号";

        public string CuttingBarCode
        {
            get { return _CuttingBarCode; }
            set { _CuttingBarCode = value; }
        }
        /// <summary>
        /// 行车设备状态
        /// </summary>
        private string _CraneDeviceState = "行车设备状态";

        public string CraneDeviceState
        {
            get { return _CraneDeviceState; }
            set { _CraneDeviceState = value; }
        }


        /// <summary>
        /// 穿梭车设备状态
        /// </summary>
        private string _ShuttleDeviceState = "行车设备状态";

        public string ShuttleDeviceState
        {
            get { return _ShuttleDeviceState; }
            set { _ShuttleDeviceState = value; }
        }

        /// <summary>
        /// 切割机设备状态
        /// </summary>
        private string _CuttingDeviceState = "切割机设备状态";

        public string CuttingDeviceState
        {
            get { return _CuttingDeviceState; }
            set { _CuttingDeviceState = value; }
        }
        /// <summary>
        /// 钢板长度
        /// </summary>
        private double _MaterialLength = 100.00;

        public double MaterialLength
        {
            get { return _MaterialLength; }
            set { _MaterialLength = value; }
        }

        /// <summary>
        /// 钢板宽度
        /// </summary>
        private double _MaterialWidth = 100.00;

        public double MaterialWidth
        {
            get { return _MaterialWidth; }
            set { _MaterialWidth = value; }
        }

        /// <summary>
        /// 钢板厚度
        /// </summary>
        private double _MaterialThk = 100.00;

        public double MaterialThk
        {
            get { return _MaterialThk; }
            set { _MaterialThk = value; }
        }

        /// <summary>
        /// 钢板重量
        /// </summary>
        private double _MaterialWeigth = 100.00;

        public double MaterialWeigth
        {
            get { return _MaterialWeigth; }
            set { _MaterialWeigth = value; }
        }

        /// <summary>
        /// 任务进度
        /// </summary>
        private double _TaskProgress = 100.00;

        public double TaskProgress
        {
            get { return _TaskProgress; }
            set { _TaskProgress = value; }
        }

        #endregion
        /// <summary>
        /// 西门子通讯
        /// </summary>
        S7Helper S7;
        public Button<TaskRbTaskViewModel> Button { get; set; }


        public VM_PageNon_Auto()
        {
            Button = new Button<TaskRbTaskViewModel>();
            //LoadPageInfo();
        }

        #region 加载页面数据项
        public void LoadPageInfo()
        {
            try
            {
                S7 = new S7Helper();
                while (true)
                {
                    Thread.Sleep(200);
                    //读取桁车当前任务的条码信息
                    this.CraneBarCode = S7.ReadValue("CraneBarCode", "String", 50).ToString();
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}
