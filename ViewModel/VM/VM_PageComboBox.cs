using DTO;
using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxy;

namespace ViewModel
{
    public class VM_PageComboBox : VM_BaseViewModel
    {
        public ComboBox<User> UserComboBox { get; set; }
        DataProxy proxy = new DataProxy();
        public VM_PageComboBox()
        {
            UserComboBox = new ComboBox<User>();
            UserComboBox.SetItemsSource(proxy.GetComboBoxData());

            UserComboBox.SelectCallBack = (user) =>
            {
                MessageBox.Show(user.Name);
            };
        }

    }
}
