using DTO;
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
    public class VM_PageScanResume : VM_BaseViewModel
    {
        public DataGrid<ScanLogViewModel> DataGrid { get; set; }
        public ComboBox<ScanStationViewModel> ComboBox_Station { get; set; }
        public ComboBox<ScanTypeViewModel> ComboBox_Type { get; set; }

        DataProxy proxy = new DataProxy();

        ScanRecordsHandle ScanLogInfo = new ScanRecordsHandle();
        TaskInfoHandle TaskHandle = new TaskInfoHandle();
        public VM_PageScanResume()
        {
            DataGrid = new DataGrid<ScanLogViewModel>(LoadData);
            ComboBox_Station = new ComboBox<ScanStationViewModel>();
            ComboBox_Type = new ComboBox<ScanTypeViewModel>();

            GetInfoData(DataGrid.CurrentPage);

            LoadComboBox();

            DataGrid.PageName = this.UIElementName;


        }

        public void GetInfoData(int Page)
        {
            int currentPage = Page;
            int skipNumber = DataGrid.SkipNumber;

            //LoadData(currentPage, DataGrid.Station, DataGrid.Scan_Type, "", this.UIElementName);

        }

        /// <summary>
        /// 任务数据查询展示
        /// 
        /// </summary>
        /// <param name="page"></param>
        public void LoadData(int page, string Station, string Type, string Upi, string PageName)
        {

            var StartTime = Globle.Start_Time;
            var EndTime = Globle.End_Time;
            Upi = Globle.Upi;

            List<ScanLogViewModel> lis_ScanLog = new List<ScanLogViewModel>();

            //ScanLogInfo.GetScanLogInfo(null, Station, Type, Upi, StartTime, EndTime, false, page, DataGrid.SkipNumber, (list, count, msg) =>
            //{
            //    if (list == null)
            //        return;
            //    list.ForEach(s =>
            //    {
            //        ScanLogViewModel ScanViewModel = new ScanLogViewModel();

            //        ScanViewModel.Id = s.Id;
            //        ScanViewModel.UPI = s.UPI;
            //        ScanViewModel.PlanNo = s.PlanNo;
            //        ScanViewModel.OrderId = s.OrderId;
            //        ScanViewModel.PackgeCode = s.PackgeCode;
            //        ScanViewModel.BackUp1 = s.BackUp1;
            //        ScanViewModel.CreateTime = s.CreateTime;

            //        switch (s.Station)
            //        {
            //            case 1:
            //                ScanViewModel.S_Station = "封边前";
            //                break;
            //            case 2:
            //                ScanViewModel.S_Station = "封边后";
            //                break;
            //            case 3:
            //                ScanViewModel.S_Station = "钻孔前";
            //                break;
            //            case 4:
            //                ScanViewModel.S_Station = "钻孔后";
            //                break;
            //            case 5:
            //                ScanViewModel.S_Station = "分拣主线";
            //                break;
            //            case 6:
            //                ScanViewModel.S_Station = "入库前";
            //                break;
            //            case 7:
            //                ScanViewModel.S_Station = "包装前";
            //                break;
            //            default:
            //                break;
            //        }

            //        switch (s.IsDel)
            //        {
            //            case 0:
            //                ScanViewModel.S_IsDel = "否";
            //                break;
            //            case 1:
            //                ScanViewModel.S_IsDel = "是";
            //                break;
            //            default:
            //                break;
            //        }

            //        if (s.Station <= 4)
            //        {
            //            ScanViewModel.S_Target = "后道工序";
            //        }
            //        else if (s.Station == 5)
            //        {
            //            switch (s.TargetArea)
            //            {
            //                case "1":
            //                    ScanViewModel.S_Target = "ABC岛区";
            //                    break;
            //                case "2":
            //                    ScanViewModel.S_Target = "DEF岛区";
            //                    break;
            //                default:
            //                    break;
            //            }
            //        }
            //        else if (s.Station == 6)
            //        {
            //            switch (s.TargetArea)
            //            {
            //                case "1":
            //                    ScanViewModel.S_Target = "A#";
            //                    break;
            //                case "2":
            //                    ScanViewModel.S_Target = "B#";
            //                    break;
            //                case "3":
            //                    ScanViewModel.S_Target = "C#";
            //                    break;
            //                case "4":
            //                    ScanViewModel.S_Target = "D#";
            //                    break;
            //                case "5":
            //                    ScanViewModel.S_Target = "E#";
            //                    break;
            //                case "6":
            //                    ScanViewModel.S_Target = "F#";
            //                    break;
            //                default:
            //                    break;
            //            }
            //        }
            //        else
            //        {
            //            ScanViewModel.S_Target = "包装";
            //        }

            //        lis_ScanLog.Add(ScanViewModel);
            //    });

            //    DataGrid.SetItemsSource(lis_ScanLog);
            //    DataGrid.RecordCount = count;
            //});

            DataGrid.SelectCallBack = (ScanLog) =>
            {
                if (ScanLog == null)
                    return;
                DataGrid.Id = ScanLog.Id;
            };
        }


        /// <summary>
        /// 加载ComboBox内容
        /// 
        /// </summary>
        public void LoadComboBox()
        {
            var Station = ScanLogInfo.GetScanStation();
            ComboBox_Station.SetItemsSource(Station);

            DataGrid.Station = Station[0].Station;

            ComboBox_Station.SelectCallBack = (s) =>
            {
                if (s == null)
                    return;

                DataGrid.Station = s.Station;
            };

            var ScanType = ScanLogInfo.GetScanType();
            ComboBox_Type.SetItemsSource(ScanType);

            DataGrid.Scan_Type = ScanType[0].Type;

            ComboBox_Type.SelectCallBack = (s) =>
            {
                if (s == null)
                    return;
                DataGrid.Scan_Type = s.Type;
            };
        }
    }
}

