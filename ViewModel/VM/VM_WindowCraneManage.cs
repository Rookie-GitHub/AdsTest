using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class VM_CraneManageWindow : VM_BaseViewModel
    {

        public Button<TaskRbTaskViewModel> Button { get; set; }
        public VM_CraneManageWindow()
        {
            Button = new Button<TaskRbTaskViewModel>();
        }
    }
}
