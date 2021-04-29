using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace ViewModel
{
    public class Button<T> : Control<T>
    {
        private Action<T> LoadAction = null;
        public Action<T> SelectCallBack = null;
        private Func<object, bool> DataFilter = null;
        public List<T> ListT = new List<T>();

        private string _PageName;

        public string PageName
        {
            get { return _PageName; }
            set { _PageName = value; }
        }
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
        public void SetItemsSource(List<T> itemSource)
        {
            ItemsSource = new ObservableCollection<T>(itemSource);
        }

        public void SetItemsSourceNew(List<T> itemSource)
        {
            ListT = itemSource;
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


        #region 方法

        public Button()
        {

        }
        public void BindSource(Action<T> loadAction, T conditionRow = default(T))
        {
            LoadAction = loadAction;
            if (LoadAction != null)
            {
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
    }
}
