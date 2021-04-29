using DTO;
using Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class VM_PageDeviceManage : VM_BaseViewModel
    {
        #region 变量声明
        public DataGrid<DeviceTypeViewModel> DataGrid { get; set; }
        public ComboBox<ComBoBoxViewModel> ComboBox_DeviceType { get; set; }
        CommonHandle CommonHandle;
        #endregion


        public VM_PageDeviceManage()
        {
            DataGrid = new DataGrid<DeviceTypeViewModel>(LoadData);
            ComboBox_DeviceType = new ComboBox<ComBoBoxViewModel>();
            CommonHandle = new CommonHandle();
            LoadComboBox();
            DataGrid.PageName = this.UIElementName;
        }

        public void LoadData(int page, string Area, string TaskType, string PageName)
        {

        }

        /// <summary>
        /// 加载ComboBox内容
        /// 
        /// </summary>
        public void LoadComboBox()
        {
            var TypeSource = CommonHandle.FillInComboBox("DeviceType");
            ComboBox_DeviceType.SetItemsSource(TypeSource);

            DataGrid.DeviceType = TypeSource[0].DeviceType;

            ComboBox_DeviceType.SelectCallBack = (Type) =>
            {
                if (Type == null)
                    return;
                DataGrid.DeviceType = Type.DeviceType;
            };
        }

    }
}
