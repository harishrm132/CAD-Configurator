using AnglePipeCAD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnglePipeCAD
{
    public class Initializr
    {
        public static void CreateAnglePipe(AnglePart angle, PipePart pipe, AnglePipeAssy anglePipeAssy, CADTools app)
        {
            switch (app)
            {
                case CADTools.Solidworks:
                    angle.CreateSldWorks();
                    pipe.CreateSldWorks();
                    anglePipeAssy.CreateSldWorks();
                    break;
                case CADTools.CATIA:
                    break;
                case CADTools.SiemensNX:
                    break;
                case CADTools.AutodeskInventor:
                    break;
                case CADTools.SolidEdge:
                    break;
                case CADTools.CREO:
                    break;
                default:
                    break;
            }
        }

    }
}
