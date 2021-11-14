using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnglePipeCAD
{
    public enum CADTools
    {
        Solidworks = 1,
        CATIA,
        SiemensNX,
        AutodeskInventor,
        SolidEdge,
        CREO
    }

    public enum Units
    {
        MM_to_M,
        MM_to_MM,
    }
}
