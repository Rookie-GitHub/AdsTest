using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ViewModel
{
    public class VM_PageMain : VM_BaseViewModel
    {

        public Page _FrameSource;
        public Page FrameSource
        {
            get { return _FrameSource; }
            set
            {
                _FrameSource = value;
                OnPropertyChanged();
            }
        }
        public BaseCommand ChangeFrameSourceCommand
        {
            get
            {
                return new BaseCommand(ChangeFrameSourceCommand_Executed);
            }
        }
        public void ChangeFrameSourceCommand_Executed(object obj)
        {
            string pageName = obj.ToString();
            switch (pageName)
            {
                case "PageMain":
                    FrameSource = new VM_PageMain().UIElement as Page;
                    break;
                case "PageUser":
                    FrameSource = new VM_PageUser().UIElement as Page;
                    break;
            }
        }
    }
}
