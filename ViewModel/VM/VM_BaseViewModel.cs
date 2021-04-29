using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace ViewModel
{
    public class VM_BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public const string UINameSapce = "AdsTest";
        public string UIElementName = "";
        public FrameworkElement UIElement { get; set; }
        public Window WindowMain { get; set; } //主窗体 

        public EventHandler CloseCallBack = null; //窗体/页面/控件 关闭委托
        public VM_BaseViewModel()
        {
            WindowMain = Application.Current.MainWindow;
            SetUIElement();
            UIElement.DataContext = this;
        }


        #region 通过反射创建对应的UI元素
        public void SetUIElement()
        {
            Type childType = this.GetType();//获取子类的类型  
            string name = this.GetType().Name;
            UIElementName = name.Replace("VM_", "");
            UIElementName = UIElementName.Replace("`1", "");//应对泛型实体

            if (name.Contains("Window"))
            {
                UIElement = GetElement<Window>();
                (UIElement as Window).Closing += (s, e) =>
                {
                    if (CloseCallBack != null)
                    {
                        CloseCallBack(s, e);
                    }
                };
            }
            else if (name.Contains("Page"))
            {
                UIElement = GetElement<Page>();
                (UIElement as Page).Unloaded += (s, e) =>
                {
                    if (CloseCallBack != null)
                    {
                        CloseCallBack(s, e);
                    }
                };
            }
            else if (name.Contains("UC"))
            {
                UIElement = GetElement<UserControl>();
                (UIElement as UserControl).Unloaded += (s, e) =>
                {
                    if (CloseCallBack != null)
                    {
                        CloseCallBack(s, e);
                    }
                };
            }
            else
            {
                throw new Exception("元素名不规范");
            }
        }

        public E GetElement<E>()
        {
            Type type = GetFormType(UINameSapce + "." + UIElementName);
            E element = (E)Activator.CreateInstance(type);
            return element;
        }

        public static Type GetFormType(string fullName)
        {
            Assembly assembly = Assembly.Load(UINameSapce);
            Type type = assembly.GetType(fullName, true, false);
            return type;
        }
        #endregion

        #region 窗体操作
        public void Show()
        {
            if (UIElement is Window)
            {
                (UIElement as Window).Show();
            }
            else
            {
                throw new Exception("元素类型不正确");
            }
        }

        public void ShowDialog()
        {
            if (UIElement is Window)
            {
                (UIElement as Window).ShowDialog();
            }
            else
            {
                throw new Exception("元素类型不正确");
            }
        }

        public void Close()
        {
            if (UIElement is Window)
            {
                (UIElement as Window).Close();
            }
            else
            {
                throw new Exception("元素类型不正确");
            }
        }

        public void Hide()
        {
            if (UIElement is Window)
            {
                (UIElement as Window).Hide();
            }
            else
            {
                throw new Exception("元素类型不正确");
            }
        }
        #endregion

        /// <summary>
        /// data与View关联 
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}