using AssemblyTreeCAD;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAD_Configurator.ViewModels
{
    public class AssemblyTreeViewModel : Screen
    {

        public List<object> CompList { get; set; }

        public AssemblyTreeViewModel()
        {
            CompList = Initializr.GetDGV(CommonLibrary.Supports.CADTools.Solidworks);
        }
    }
}
