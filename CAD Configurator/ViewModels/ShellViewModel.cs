using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CAD_Configurator.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        public ShellViewModel()
        {

        }

        public void ShowTools()
        {
            ActivateItem(new PipeAngleViewModel());
        }
    }
}
