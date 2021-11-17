using CommonLibrary.Supports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyTreeCAD
{
    public class Initializr
    {
        public static List<object> GetDGV(CADTools app)
        {
            switch (app)
            {
                case CADTools.Solidworks:
                    SwAssemblyTree swAssembly = new SwAssemblyTree();
                    return new List<object>(swAssembly.CompList);
                    break;
                case CADTools.CATIA:
                    CaAssemblyTree caAssembly = new CaAssemblyTree();
                    return new List<object>(caAssembly.CompList);
                    break;
                case CADTools.SiemensNX:
                case CADTools.AutodeskInventor:
                case CADTools.SolidEdge:
                case CADTools.CREO:
                default:
                    return null;
                    break;
            }
        }
    }
}
