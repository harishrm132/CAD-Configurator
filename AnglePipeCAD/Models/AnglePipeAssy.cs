using AnglePipeCAD.Helpers;
using SolidWorks.Interop.sldworks;
using SolidworksCore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnglePipeCAD.Models
{
    public class AnglePipeAssy : Base
    {

        public AnglePart Angle { get; set; }
        public PipePart Pipe { get; set; }
        public double MateDistance { get; set; }

        public AnglePipeAssy(AnglePart angle, PipePart pipe, double mateDistance)
        {
            Angle = angle;
            Pipe = pipe;
            MateDistance = mateDistance;
            TargetFolder = angle.TargetFolder;
            FileName = FileName3D.PipeAngle;
        }
        public override void CreateSldWorks()
        {
            //Create Assembly
            SldWorks swApp = DocumentManager.CreateAssemblyDoc(TargetFolder, FileName);
            swApp.AddComponent(Pipe, Angle, FileName);
            AssemblyDoc swAssy = (AssemblyDoc)swApp.ActiveDoc;
            swAssy.AddConcentricMate(Angle.FileName, Pipe.FileName, Angle.MateRefHole, Pipe.MateOutsideFace);
            swAssy.AddDistanceMate(Angle.FileName, Pipe.FileName, Angle.MateRef1, Pipe.MateBase, MateDistance);

            //Save & close assembly
            swApp.Save(TargetFolder, FileName, sw_DocType.Assembly);
            Tools.OpenFolder(TargetFolder);
        }

        public override void CreateCatia()
        {
            throw new NotImplementedException();
        }

    }
}
