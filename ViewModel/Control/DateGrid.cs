using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace ViewModel
{
    public class DataGrid<T> : Control<T>
    {
        private Action<T> LoadAction = null;
        private Action<int, string, string, string> _loadData = null;
        private Action<int, string, string, string, string> _loadDataForResume = null;
        public Action<T> SelectCallBack = null;
        private Func<object, bool> DataFilter = null;
        public Window _FrameSource;
        public Window FrameSource
        {
            get { return _FrameSource; }
            set { _FrameSource = value; OnPropertyChanged(); }
        }

        private ObservableCollection<T> _ItemsSource = new ObservableCollection<T>();
        public ObservableCollection<T> ItemsSource
        {
            get { return _ItemsSource; }
            set
            {
                _ItemsSource = value;
                if (_ItemsSource != null && _ItemsSource.Count > 0 && SelectedItem == null)
                {
                    SelectedItem = _ItemsSource.First();
                }
                OnPropertyChanged();
            }
        }

        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _area;

        public string Area
        {
            get { return _area; }
            set { _area = value; }
        }

        private string _inOutType;

        public string InOutType
        {
            get { return _inOutType; }
            set { _inOutType = value; }
        }

        private string _combo_Type;

        public string Combo_Type
        {
            get { return _combo_Type; }
            set { _combo_Type = value; }
        }

        private string _combo_Area;

        public string Combo_Area
        {
            get { return _combo_Area; }
            set { _combo_Area = value; }
        }

        private string _station;

        public string Station
        {
            get { return _station; }
            set { _station = value; }
        }

        private string _scan_Type;

        public string Scan_Type
        {
            get { return _scan_Type; }
            set { _scan_Type = value; }
        }

        private string _PageName;

        public string PageName
        {
            get { return _PageName; }
            set { _PageName = value; }
        }

        private string _placeType;

        public string PlaceType
        {
            get { return _placeType; }
            set { _placeType = value; }
        }

        /// <summary>
        /// 设备类型
        /// </summary>
        private string _DeviceType;

        public string DeviceType
        {
            get { return _DeviceType; }
            set { _DeviceType = value; }
        }

        public void SetItemsSource(List<T> itemSource)
        {
            ItemsSource = new ObservableCollection<T>(itemSource);
        }
        public T _SelectedItem;
        public T SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
                if (SelectCallBack != null)
                {
                    SelectCallBack(_SelectedItem);
                }
                OnPropertyChanged();
            }
        }
        private ICollectionView _ItemsSourceView;
        public ICollectionView ItemsSourceView
        {
            get
            {
                _ItemsSourceView = CollectionViewSource.GetDefaultView(_ItemsSource);
                return _ItemsSourceView;
            }
            set
            {
                _ItemsSourceView = value;
                OnPropertyChanged();
            }
        }
        private T _Condition = (T)Activator.CreateInstance(typeof(T));
        public T Condition { get { return _Condition; } set { _Condition = value; OnPropertyChanged(); } }

        #region 查询方法
        public DataGrid(Action<int, string, string, string> action)
        {
            _loadData = action;
            BindSource(LoadAction);
        }

        public DataGrid(Action<int, string, string, string, string> action)
        {
            _loadDataForResume = action;
            BindSource(LoadAction);
        }
        public void BindSource(Action<T> loadAction, T conditionRow = default(T))
        {
            LoadAction = loadAction;
            if (LoadAction != null)
            {
                CurrentPage = 1;
                LoadAction(conditionRow);
            }
        }
        public void BindSource(Action loadAction)
        {
            LoadAction = new Action<T>((obj) =>
            {
                loadAction();
            }); ;
            if (LoadAction != null)
            {
                CurrentPage = 1;
                LoadAction(default(T));
            }
        }
        public void ItemsSourceReBind()
        {
            BindSource(LoadAction);
        }
        public void SelectedItemReBind()
        {
            T newitem = (T)Activator.CreateInstance(typeof(T));
            List<System.Reflection.PropertyInfo> plist = typeof(T).GetProperties().ToList();

            foreach (var propertyInfo in plist)
            {
                propertyInfo.SetValue(newitem, propertyInfo.GetValue(SelectedItem));
            }
            SelectedItem = newitem;
        }
        public void SetFilter(Func<object, bool> dataFilter)
        {
            try
            {
                DataFilter = dataFilter;
                _ItemsSourceView = CollectionViewSource.GetDefaultView(_ItemsSource);
                _ItemsSourceView.Filter = new Predicate<object>(DataFilter);
            }
            catch (Exception ex)
            {

            }
        }
        public void Refresh()
        {
            if (_ItemsSourceView == null)
            {
                _ItemsSourceView = CollectionViewSource.GetDefaultView(this.ItemsSource);
            }
            _ItemsSourceView.Refresh();
        }
        #endregion

        #region 分页


        private int _SkipNumber = 30;
        public int SkipNumber { get { return _SkipNumber; } set { _SkipNumber = value; OnPropertyChanged(); } }

        private volatile int _CurrentPage = 1;
        public int CurrentPage
        {
            get { return _CurrentPage; }
            set
            {
                _CurrentPage = value;
                if (_CurrentPage > PageCount)
                {
                    _CurrentPage = PageCount;
                }
                if (_CurrentPage < 1)
                {
                    _CurrentPage = 1;
                }
                OnPropertyChanged();
            }
        }
        private int _PageCount = 1;
        public int PageCount { get { return _PageCount; } set { _PageCount = value; OnPropertyChanged(); } }
        private int _RecordCount = 0;
        public int RecordCount
        {
            get { return _RecordCount; }
            set
            {
                _RecordCount = value;
                if (_RecordCount <= SkipNumber)
                {
                    PageCount = 1;
                }
                else
                {
                    PageCount = int.Parse(Math.Ceiling((double)RecordCount / (double)SkipNumber).ToString());
                }
                if (_CurrentPage > PageCount)
                {
                    _CurrentPage = PageCount;
                }
                OnPropertyChanged();
            }
        }

        private TextBox<string> _JumpTextBox = new TextBox<string>();
        public TextBox<string> JumpTextBox
        {
            get { return _JumpTextBox; }
            set { _JumpTextBox = value; OnPropertyChanged(); }
        }

        #region 跳页
        public BaseCommand JumpCommand
        {
            get
            {
                return new BaseCommand(JumpCommand_Executed);
            }
        }
        void JumpCommand_Executed(object send)
        {
            int pagenum = 0;

            if (int.TryParse(JumpTextBox.Text, out pagenum))
            {
                if (pagenum <= PageCount && pagenum > 0)
                {
                    CurrentPage = pagenum;

                    if (LoadAction != null)
                    {
                        LoadAction(Condition);
                    }
                    switch (PageName)
                    {
                        case "PageScanRecords":
                            //查询
                            _loadData(CurrentPage, Station, Scan_Type, PageName);
                            break;
                        case "PageStoreManage":
                            //查询
                            _loadData(CurrentPage, Area, Scan_Type, PageName);
                            break;
                        case "PageDataGrid":
                            //查询
                            _loadData(CurrentPage, Area, InOutType, PageName);
                            break;
                        case "PageScanResume":
                            //查询 
                            _loadDataForResume(CurrentPage, Station, Scan_Type, "", PageName);
                            break;
                        default:
                            break;
                    }

                }
                else
                {
                    MessageBox.Show("请正确填写跳转页数。", "提示信息");
                }
            }
            else
            {
                MessageBox.Show("请正确填写跳转页数。", "提示信息");
            }
        }
        #endregion

        #region 上一页
        public BaseCommand PreviousCommand
        {
            get
            {
                return new BaseCommand(PreviousCommand_Executed);
            }
        }
        void PreviousCommand_Executed(object send)
        {
            if (CurrentPage > 1)
            {
                CurrentPage -= 1;
                if (LoadAction != null)
                {
                    LoadAction(Condition);
                }
                switch (PageName)
                {
                    case "PageScanRecords":
                        //查询
                        _loadData(CurrentPage, Station, Scan_Type, PageName);
                        break;
                    case "PageStoreManage":
                        //查询
                        _loadData(CurrentPage, Area, Scan_Type, PageName);
                        break;
                    case "PageDataGrid":
                        //查询
                        _loadData(CurrentPage, Area, InOutType, PageName);
                        break;
                    case "PageScanResume":
                        //查询 
                        _loadDataForResume(CurrentPage, Station, Scan_Type, "", PageName);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                MessageBox.Show("已至首页。", "提示信息");
            }
        }
        #endregion

        #region 下一页
        public BaseCommand NextCommand
        {
            get
            {
                return new BaseCommand(NextCommand_Executed);
            }
        }
        void NextCommand_Executed(object send)
        {
            if (CurrentPage < PageCount)
            {
                CurrentPage += 1;

                if (LoadAction != null)
                {
                    LoadAction(Condition);
                }
                switch (PageName)
                {
                    case "PageScanRecords":
                        //查询
                        _loadData(CurrentPage, Station, Scan_Type, PageName);
                        break;
                    case "PageStoreManage":
                        //查询
                        _loadData(CurrentPage, Area, Scan_Type, PageName);
                        break;
                    case "PageDataGrid":
                        //查询
                        _loadData(CurrentPage, Area, InOutType, PageName);
                        break;
                    case "PageScanResume":
                        //查询 
                        _loadDataForResume(CurrentPage, Station, Scan_Type, "", PageName);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                MessageBox.Show("已至末页。", "提示信息");
            }
        }
        #endregion

        public BaseCommand RefreshData
        {
            get
            {
                return new BaseCommand(RefreshData_Executed);
            }
        }
        void RefreshData_Executed(object send)
        {
            Refresh();
        }
        #endregion

        #region 修改
        public BaseCommand UpdateCommand
        {
            get
            {
                return new BaseCommand(UpdateCommand_Executed);
            }
        }
        void UpdateCommand_Executed(object send)
        {
            //Globle.AddOrUp = 1;
            //Globle.UpdateId = Id;
            bool Exist = false;
            switch (PageName)
            {
                case "PageCuttingManage":

                    if (FrameSource == null)
                    {
                        //FrameSource = new VM_WindowCuttingManage().UIElement as Window;
                        //FrameSource.Show();
                    }
                    else
                    {
                        foreach (Window item in Application.Current.Windows)
                        {
                            if (item == FrameSource)
                            {
                                Exist = true;
                            }
                            else
                            {

                            }
                        }

                        if (!Exist)
                        {
                            //FrameSource = new VM_WindowCuttingManage().UIElement as Window;
                            //FrameSource.Show();
                        }
                    }
                    break;
                default:
                    break;
            }

        }
        #endregion

        #region 删除
        public BaseCommand DeleteCommand
        {
            get
            {
                return new BaseCommand(DeleteCommand_Executed);
            }
        }
        void DeleteCommand_Executed(object send)
        {
            int IResult = -1;
            switch (PageName)
            {                
                default:
                    break;
            }

        }
        #endregion

        #region 查询
        public BaseCommand GetInfoCommand
        {
            get
            {
                return new BaseCommand(GetInfoCommand_Executed);
            }
        }
        void GetInfoCommand_Executed(object send)
        {

        }
        #endregion

        #region 刷新
        public BaseCommand RefreshDataInfoCommand
        {
            get
            {
                return new BaseCommand(RefreshDataInfoCommand_Executed);
            }
        }
        void RefreshDataInfoCommand_Executed(object send)
        {
            switch (PageName)
            {
                default:
                    break;
            }
        }
        #endregion

        #region 新增窗体
        /// <summary>
        /// 
        /// </summary>
        public BaseCommand AddCommand
        {
            get
            {
                return new BaseCommand(AddCommand_Executed);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        void AddCommand_Executed(object obj)
        {
            bool Exist = false;

            foreach (Window item in Application.Current.Windows)
            {
                if (!item.Title.Contains("客户端"))
                {
                    item.Close();
                }
            }
            switch (PageName)
            {
                case "PageCuttingManage":
                    if (FrameSource == null)
                    {
                        //FrameSource = new VM_WindowCuttingManage().UIElement as Window;
                        //FrameSource.Show();
                    }
                    else
                    {
                        foreach (Window item in Application.Current.Windows)
                        {
                            if (item == FrameSource)
                            {
                                Exist = true;
                            }
                            else
                            {

                            }
                        }

                        if (!Exist)
                        {
                            //FrameSource = new VM_WindowCuttingManage().UIElement as Window;
                            //FrameSource.Show();
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion

    }
}