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
    public class AnglePart : Base
    {
        public AnglePart(string targetFolder, double xLength, double yLength, double width, double thickness, double boltHoleRad, double pipeHoleRad, double rad1, double rad2, double x1, double x2)
        {
            XLength = xLength.ConvUnits();
            YLength = yLength.ConvUnits();
            Width = width.ConvUnits();
            Thickness = thickness.ConvUnits();
            BoltHoleRad = boltHoleRad.ConvUnits();
            PipeHoleRad = pipeHoleRad.ConvUnits();
            Rad1 = rad1.ConvUnits();
            Rad2 = rad2.ConvUnits();
            X1 = x1.ConvUnits();
            X2 = x2.ConvUnits();
            MateRef1 = MateFace.AngleFace;
            MateRefHole = MateFace.AngleHole;
            TargetFolder = targetFolder;
            FileName = FileName3D.Angle;
        }

        public double XLength { get; set; }
        public double YLength { get; set; }
        public double Width { get; set; }
        public double Thickness { get; set; }
        public double BoltHoleRad { get; set; }
        public double PipeHoleRad { get; set; }
        public double Rad1 { get; set; }
        public double Rad2 { get; set; }
        public double X1 { get; set; }
        public double X2 { get; set; }
        public string MateRef1 { get; set; }
        public string MateRefHole { get; set; }

        public override void CreateSldWorks()
        {
            SldWorks swApp = SolidWorksSingleton.GetApplication();
            swApp.CreatePartDoc();
            ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;

            //Select Plane
            bool status = swModel.Extension.SelectByID2(Planes.Front, FeatureType.Plane, 0, 0, 0, false, 0, null, 0);

            //Insert Sketch
            swModel.InsertSketch2(true);
            swModel.CreateLine2(0, 0, 0, XLength, 0, 0);
            swModel.CreateLine2(0, 0, 0, 0, YLength, 0);

            //swModel.AddDimension2(0, 0, 0);
            //swModel.AddDimension2(0, 0, 0);
            //int markHorizontal = 2;
            //int markVertical = 4;
            //swModel.Extension.SelectByID2("Point1@Origin", FeatureType.SketchSegment, 0, 0, 0, false, markHorizontal | markVertical, null, 0);

            //Add Dimesnion To Sketch
            object datumDisp = "Point1@Origin";
            swModel.SketchManager.FullyDefineSketch(true, true,
                (int)(swSketchFullyDefineRelationType_e.swSketchFullyDefineRelationType_Vertical | swSketchFullyDefineRelationType_e.swSketchFullyDefineRelationType_Horizontal),
                true, (int)swAutodimScheme_e.swAutodimSchemeBaseline, datumDisp,
                (int)swAutodimScheme_e.swAutodimSchemeBaseline, datumDisp,
                (int)swAutodimHorizontalPlacement_e.swAutodimHorizontalPlacementBelow,
                (int)swAutodimVerticalPlacement_e.swAutodimVerticalPlacementLeft);
            swModel.InsertSketch2(true);

            //Update Sketch Name & Select
            Feature swFeature = swModel.FeatureByPositionReverse(0);
            swFeature.Name = "Sketch1";
            swModel.Extension.SelectByID2(swFeature.Name, FeatureType.Sketch, 0, 0, 0, false, 0, null, 0);

            //Create SheetMetal Flange
            swModel.FeatureManager.InsertSheetMetalBaseFlange2(Thickness, false, Thickness,
                Width, 0, true, 0, 0, 0, null, false, 0, 0, 0, 0, false, false, false, false);

            //Change Face Name
            swModel.ShowNamedView2("", (int)swStandardViews_e.swIsometricView);
            swApp.ChangeEntityName(FeatureType.Face, MateRef1, Thickness, YLength / 2, -Width / 2);

            //Select Face For Bolt Holes & Create Sketch
            swModel.Extension.SelectByID2("", FeatureType.Face, XLength / 2, Thickness, -Width / 2, false, 0, null, 0);
            swModel.InsertSketch2(true);
            swModel.CreateCircleByRadius2(XLength - X1, X2, 0, BoltHoleRad);
            swModel.CreateCircleByRadius2(XLength - X1, Width - X2, 0, BoltHoleRad);
            swModel.InsertSketch2(true);

            //Update Sketch Name & Select
            swFeature = swModel.FeatureByPositionReverse(0);
            swFeature.Name = "BoltHoles";
            swModel.Extension.SelectByID2(swFeature.Name, FeatureType.Sketch, 0, 0, 0, false, 0, null, 0);
            swModel.SimpleCut();

            //Select Pipe Hole Face
            PartDoc swPart = (PartDoc)swApp.ActiveDoc;
            Entity swEntity = swPart.GetEntityByName(MateRef1, (int)swSelectType_e.swSelFACES);
            swEntity.Select4(false, null);

            //Create Pipe Hole Sketch
            swModel.InsertSketch2(true);
            swModel.CreateCircleByRadius2(Width / 2, YLength - (PipeHoleRad * 2), 0, PipeHoleRad);
            //swModel.AddDiameterDimension(0, 0, 0);
            swModel.InsertSketch2(true);
            swModel.SimpleCut();
            //Change Hole Face Name
            swModel.ShowNamedView2("", (int)swStandardViews_e.swIsometricView);
            swApp.ChangeEntityName(FeatureType.Face, MateRefHole, Thickness / 2, YLength - PipeHoleRad, -Width / 2);

            //Select Edges for Fillet & Create (Horz Bottom Plate)
            swModel.Extension.SelectByID2("", FeatureType.Edge, XLength, Thickness / 2, 0, false, 0, null, 0);
            swModel.Extension.SelectByID2("", FeatureType.Edge, XLength, Thickness / 2, -Width, true, 0, null, 0);
            swModel.SimpleFillet(Rad2);

            //Select Edges for Fillet & Create (Vert Bottom Plate)
            swModel.ShowNamedView2("", (int)swStandardViews_e.swIsometricView);
            swModel.Extension.SelectByID2("", FeatureType.Edge, Thickness / 2, YLength, 0, false, 0, null, 0);
            swModel.Extension.SelectByID2("", FeatureType.Edge, Thickness / 2, YLength, -Width, true, 0, null, 0);
            swModel.SimpleFillet(Rad1);

            //Clear Selection & Zoom to fit
            swModel.ClearSelection2(true);
            //swModel.ShowNamedView2("*NormalTo", -1);
            swModel.ShowNamedView2("", (int)swStandardViews_e.swIsometricView);
            swModel.ViewZoomtofit2();

            //Save Document
            swApp.Save(TargetFolder, FileName, sw_DocType.Part);

        }

        public override void CreateCatia()
        {
            Initializr_AnglePipe.CreateAngle(XLength, YLength, Width, Thickness, PipeHoleRad, X1, X2, BoltHoleRad, Rad1, Rad2, TargetFolder, FileName);
        }
    }
}
