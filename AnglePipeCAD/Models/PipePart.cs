using AnglePipeCAD.Helpers;
using CatiaCore;
using CommonLibrary.Helpers;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidworksCore;
using SolidworksCore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnglePipeCAD.Models
{
    public class PipePart : Base
    {
        public PipePart(string targetFolder, double outsideDia, double insideDia, double length)
        {
            OutsideDia = outsideDia.ConvUnits();
            InsideDia = insideDia.ConvUnits();
            Length = length.ConvUnits();
            MateOutsideFace = MateFace.PipeOutsideFace;
            MateBase = MateFace.PipeBase;
            TargetFolder = targetFolder;
            FileName = FileName3D.Pipe;
        }

        public double OutsideDia { get; set; }
        public double InsideDia { get; set; }
        public double Length { get; set; }

        public string MateOutsideFace { get; set; }
        public string MateBase { get; set; }

        public override void CreateSldWorks()
        {
            SldWorks swApp = SolidWorksSingleton.GetApplication();
            swApp.CreatePartDoc();
            ModelDoc2 swModel = swApp.ActiveDoc;
            Configuration config;
            CustomPropertyManager cusPropMgr;
            bool status;

            //Set Plane Names
            Feature swFeature = swModel.FeatureByPositionReverse(3);
            swFeature.Name = Planes.Front;
            swFeature = swModel.FeatureByPositionReverse(2);
            swFeature.Name = Planes.Top;
            swFeature = swModel.FeatureByPositionReverse(1);
            swFeature.Name = Planes.Right;

            //Select Plane
            status = swModel.Extension.SelectByID2(Planes.Top, FeatureType.Plane, 0, 0, 0, false, 0, null, 0);

            //Insert Sketch
            //TODO - UNITS in MKS
            swModel.InsertSketch2(true);
            swModel.CreateCircleByRadius2(0, 0, 0, OutsideDia / 2); //500 mm
            swModel.CreateCircleByRadius2(0, 0, 0, InsideDia / 2); //200 mm
            swModel.InsertSketch2(true);

            //Update Sketch Name
            swFeature = swModel.FeatureByPositionReverse(0);
            swFeature.Name = "PipeSketch";

            //Select Sketch
            status = swModel.Extension.SelectByID2("PipeSketch", FeatureType.Sketch, 0, 0, 0, false, 0, null, 0);

            //Create Extrude
            swFeature = swModel.FeatureManager.FeatureExtrusion3(true, false, false,
                (int)swEndConditions_e.swEndCondBlind, 0, Length, 0,
                false, false, false, false, 0, 0, false, false, false, false, false, false, false, 0, 0, false);

            //Change Entity Name of OD & ID for Mating
            swApp.ChangeEntityName(FeatureType.Face, MateOutsideFace, OutsideDia / 2, Length / 2, 0);
            double dim = (OutsideDia - InsideDia) / 4;
            swApp.ChangeEntityName(FeatureType.Face, MateBase, (InsideDia / 2) + dim, 0, 0);

            //Update Extrude Name
            swFeature = swModel.FeatureByPositionReverse(0);
            swFeature.Name = "PipeModel";

            //Rebuild
            swModel.ForceRebuild3(true);
            swModel.ViewZoomtofit2();

            config = swModel.GetActiveConfiguration();
            cusPropMgr = config.CustomPropertyManager;

            cusPropMgr.Add3("Description", (int)swCustomInfoType_e.swCustomInfoText, "Pipe Bore",
                (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
            cusPropMgr.Add3("Dimensions", (int)swCustomInfoType_e.swCustomInfoText, "800mm.",
                (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

            //Save Part File
            swApp.Save(TargetFolder, FileName, sw_DocType.Part);

        }

        public override void CreateCatia()
        {
            Initializr_AnglePipe.CreatePipe(OutsideDia, InsideDia, Length, TargetFolder, FileName);
        }

    }
}
