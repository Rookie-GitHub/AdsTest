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
    public class VM_PageStoreManage : VM_BaseViewModel
    {
        public delegate void Delete(int Id);

        public DataGrid<StoreManageViewModel> DataGrid { get; set; }
        public ComboBox<RobAreaViewModel> ComboBox_Area { get; set; }

        public ComboBox<RobAreaViewModel> ComboBox_Type { get; set; }

        StoreManageHandle StoreHandle = new StoreManageHandle();

        TaskInfoHandle TaskHandle = new TaskInfoHandle();

        public VM_PageStoreManage()
        {
            DataGrid = new DataGrid<StoreManageViewModel>(LoadData);
            ComboBox_Area = new ComboBox<RobAreaViewModel>();
            ComboBox_Type = new ComboBox<RobAreaViewModel>();
            LoadComboBox();
            GetInfoData();
            DataGrid.PageName = this.UIElementName;
        }

        public void GetInfoData()
        {
            int skipNumber = DataGrid.SkipNumber;
            LoadData(DataGrid.CurrentPage, DataGrid.Area, DataGrid.PlaceType, DataGrid.PageName);
        }

        /// <summary>
        /// 任务数据查询展示
        /// 
        /// </summary>
        /// <param name="page"></param>
        public void LoadData(int page, string Area, string Type, string PageName)
        {
            List<StoreManageViewModel> lis_StoreManage = new List<StoreManageViewModel>();

            //StoreHandle.GetStoreInfo(null, Area, Type, page, DataGrid.SkipNumber, (list, count, msg) =>
            //{
            //    if (list == null)
            //        return;
            //    list.ForEach(s =>
            //    {
            //        StoreManageViewModel StoreManageModel = new StoreManageViewModel();

            //        StoreManageModel.Id = s.Id;
            //        StoreManageModel.PlaceNo = s.PlaceNo;

            //        switch ((int)s.PlaceType)
            //        {
            //            case 1:
            //                StoreManageModel.PlaceType = "一类型货位";
            //                break;
            //            case 2:
            //                StoreManageModel.PlaceType = "二类型货位";
            //                break;
            //            case 3:
            //                StoreManageModel.PlaceType = "三类型货位";
            //                break;
            //            case 4:
            //                StoreManageModel.PlaceType = "四类型货位";
            //                break;
            //            case 5:
            //                StoreManageModel.PlaceType = "水平货架";
            //                break;
            //            default:
            //                break;
            //        }
            //        StoreManageModel.Length = (int)s.Length;
            //        StoreManageModel.Width = (int)s.Width;

            //        StoreManageModel.S_Status = (int)s.Status == 0 ? "不可用" : "可用";
            //        StoreManageModel.S_IsLock = (int)s.IsLock == 0 ? "未锁定" : "锁定";
            //        StoreManageModel.S_IsFull = (int)s.IsFull == 0 ? "无货" : "有货";

            //        lis_StoreManage.Add(StoreManageModel);
            //    });
            //    DataGrid.SetItemsSource(lis_StoreManage);
            //    DataGrid.RecordCount = count;

            //    DataGrid.Id = lis_StoreManage[0].Id;
            //});

            DataGrid.SelectCallBack = (Store) =>
            {
                if (Store == null)
                    return;
                DataGrid.Id = Store.Id;
            };
        }


        /// <summary>
        /// 加载ComboBox内容
        /// 
        /// </summary>
        public void LoadComboBox()
        {
            var AreaSource = StoreHandle.GetRobArea();
            ComboBox_Area.SetItemsSource(AreaSource);

            DataGrid.Area = AreaSource[0].Area;

            ComboBox_Area.SelectCallBack = (Area) =>
            {
                if (Area == null)
                    return;
                DataGrid.Area = Area.Area;
            };

            var TypeSource = StoreHandle.GetPlaceType();

            ComboBox_Type.SetItemsSource(TypeSource);

            DataGrid.PlaceType = TypeSource[0].Id.ToString();

            ComboBox_Type.SelectCallBack = (Type) =>
            {
                if (Type == null)
                    return;
                DataGrid.PlaceType = Type.Id.ToString();
            };
        }

    }
}
