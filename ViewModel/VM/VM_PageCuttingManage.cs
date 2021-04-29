using DTO;
using Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ViewModel
{
    public class VM_PageCuttingManage : VM_BaseViewModel
    {
        
        public DataGrid<TaskRbTaskViewModel> DataGrid { get; set; }
        public ComboBox<DeviceTypeViewModel> ComboBox_DeviceType { get; set; }

        CommonHandle CommonHandle;

        TaskInfoHandle TaskHandle;
        public VM_PageCuttingManage()
        {
            DataGrid = new DataGrid<TaskRbTaskViewModel>(LoadData);
            ComboBox_DeviceType = new ComboBox<DeviceTypeViewModel>();
            CommonHandle = new CommonHandle();
            TaskHandle = new TaskInfoHandle();
            LoadComboBox();
            GetTaksInfoData();
            DataGrid.PageName = this.UIElementName;
        }

        public void GetTaksInfoData()
        {
            LoadData(DataGrid.CurrentPage, DataGrid.Area, DataGrid.InOutType, DataGrid.PageName);
        }

        /// <summary>
        /// 任务数据查询展示
        /// 
        /// </summary>
        /// <param name="page"></param>
        public void LoadData(int page, string Area, string TaskType, string PageName)
        {
            List<TaskRbTaskViewModel> lis_TaskViewModel = new List<TaskRbTaskViewModel>();

            //TaskHandle.GetTaskInfo(null, Area, TaskType, page, DataGrid.SkipNumber, (list, count, msg) =>
            //{
            //    if (list == null)
            //        return;
            //    list.ForEach(s =>
            //    {
            //        TaskRbTaskViewModel TaskViewModel = new TaskRbTaskViewModel();
            //        TaskViewModel.Id = s.Id;
            //        TaskViewModel.TaskId = (int)s.TaskId;
            //        TaskViewModel.Upi = s.Upi;
            //        TaskViewModel.Place = s.Place;
            //        TaskViewModel.Movelength = (int)s.Movelength;
            //        TaskViewModel.Batch = s.Batch;
            //        TaskViewModel.OrderId = s.OrderId;
            //        TaskViewModel.PackNum = s.PackNum;
            //        TaskViewModel.PlateNo = (int)s.PlateNo;
            //        TaskViewModel.Length = (float)s.Length;
            //        TaskViewModel.Width = (float)s.Width;
            //        TaskViewModel.Thk = (float)s.Thk;
            //        TaskViewModel.CreateTime = (DateTime)s.CreateTime;

            //        if (s.Type == 1)
            //            TaskViewModel.SType = "入库";
            //        else
            //            TaskViewModel.SType = "出库";

            //        if (s.Status == 1)
            //            TaskViewModel.SStatus = "正在执行";
            //        else if (s.Status == 2)
            //            TaskViewModel.SStatus = "执行完毕";
            //        else
            //            TaskViewModel.SStatus = "等待执行";

            //        lis_TaskViewModel.Add(TaskViewModel);
            //    });
            //    DataGrid.SetItemsSource(lis_TaskViewModel);
            //    DataGrid.RecordCount = count;

            //    DataGrid.Id = lis_TaskViewModel[0].Id;
            //});

            DataGrid.SelectCallBack = (task) =>
            {
                if (task == null)
                    return;
                DataGrid.Id = task.Id;
            };
        }


        /// <summary>
        /// 加载ComboBox内容
        /// 
        /// </summary>
        public void LoadComboBox()
        {
            //var TypeSource = CommonHandle.FillInComboBox("");
            //ComboBox_DeviceType.SetItemsSource(TypeSource);

            //DataGrid.InOutType = TypeSource[0].InOutType;

            //ComboBox_DeviceType.SelectCallBack = (InOutType) =>
            //{
            //    if (InOutType == null)
            //        return;
            //    DataGrid.InOutType = InOutType.InOutType;
            //};

            //var AreaSource = TaskHandle.GetArea();
            //ComboBox_TaskArea.SetItemsSource(AreaSource);

            //DataGrid.Area = AreaSource[0].Area;

            //ComboBox_TaskArea.SelectCallBack = (Area) =>
            //{
            //    if (Area == null)
            //        return;
            //    DataGrid.Area = Area.Area;
            //};
        }
    }
}
