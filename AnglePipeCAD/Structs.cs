using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnglePipeCAD
{
    public struct FileName3D
    {
        public const string Pipe = "PipePart";
        public const string Angle = "AnglePart";
        public const string PipeAngle = "PipeAngleAssembly";
    }
    
    public struct MateFace
    {
        public const string PipeOutsideFace = "PipeOutsideFace";
        public const string AngleHole = "RefHole";
        public const string PipeBase = "PipeFace";
        public const string AngleFace = "Ref1";
    }
}
