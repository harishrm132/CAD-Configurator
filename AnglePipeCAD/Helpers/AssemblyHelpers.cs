using AnglePipeCAD.Models;
using SolidWorks.Interop.sldworks;
using SolidworksCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnglePipeCAD.Helpers
{
    public static class AssemblyHelpers
    {
        public static void AddComponent(this SldWorks swApp, PipePart pipe, AnglePart angle, string assyName)
        {
            AssemblyDoc swAssembly = (AssemblyDoc)swApp.ActiveDoc;

            string[] xCompNames = new string[2];
            xCompNames[0] = $"{pipe.TargetFolder}\\{pipe.FileName}.SLDPRT";
            xCompNames[1] = $"{angle.TargetFolder}\\{angle.FileName}.SLDPRT";

            string[] xCoorSysNames = new string[2];
            xCoorSysNames[0] = "Coordinate System1";
            xCoorSysNames[1] = "Coordinate System1";

            var tMatrix = new double[]
            {
                0,0,1,
                0,1,0,
                -1,0,0,

                pipe.Length, angle.YLength, 0,
                0,0,0,0,

                1,0,0,
                0,1,0,
                0,0,1,

                0,0,0,
                0,0,0,0,
            };
            object tranformationMatrix = tMatrix;

            object components = swAssembly.AddComponents3(xCompNames, tranformationMatrix, xCoorSysNames);


            ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;
            string compName = $"{angle.FileName }-1@{assyName}";

            swModel.Extension.SelectByID2(compName, FeatureType.Component, 0, 0, 0, false, 0, null, 0);
            swAssembly.FixComponent();

        }

    }
}
