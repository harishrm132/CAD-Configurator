using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidworksCore.Helpers
{
    public static class AssemblyHelpers
    {
        public static void AddConcentricMate(this AssemblyDoc swAssy, string comp1, string comp2, string mateFace1, string mateFace2)
        {
            Component2 swComp = swAssy.GetComponentByName($"{comp1}-1");
            ModelDoc2 swModel = swComp.GetModelDoc2();
            PartDoc swPart = (PartDoc)swModel;
            Entity swEntity = swPart.GetEntityByName(mateFace1, (int)swSelectType_e.swSelFACES);
            Entity swFace1 = swComp.GetCorrespondingEntity(swEntity);

            swComp = swAssy.GetComponentByName($"{comp2}-1");
            swModel = swComp.GetModelDoc2();
            swPart = (PartDoc)swModel;
            swEntity = swPart.GetEntityByName(mateFace2, (int)swSelectType_e.swSelFACES);
            Entity swFace2 = swComp.GetCorrespondingEntity(swEntity);

            bool bRet = swFace1.Select4(false, null);
            bRet = swFace2.Select4(true, null);

            Mate2 mate = swAssy.AddMate3((int)swMateType_e.swMateCONCENTRIC,
                (int)swMateAlign_e.swMateAlignALIGNED,
                false, 0, 0, 0, 0, 0, 0, 0, 0, false, out int errorCode);

            swModel.ForceRebuild3(false);
        }

        public static void AddDistanceMate(this AssemblyDoc swAssy, string comp1, string comp2, string mateFace1, string mateFace2, double distance)
        {
            Component2 swComp = swAssy.GetComponentByName($"{comp1}-1");
            ModelDoc2 swModel = swComp.GetModelDoc2();
            PartDoc swPart = (PartDoc)swModel;
            Entity swEntity = swPart.GetEntityByName(mateFace1, (int)swSelectType_e.swSelFACES);
            Entity swFace1 = swComp.GetCorrespondingEntity(swEntity);

            swComp = swAssy.GetComponentByName($"{comp2}-1");
            swModel = swComp.GetModelDoc2();
            swPart = (PartDoc)swModel;
            swEntity = swPart.GetEntityByName(mateFace2, (int)swSelectType_e.swSelFACES);
            Entity swFace2 = swComp.GetCorrespondingEntity(swEntity);

            bool bRet = swFace1.Select4(false, null);
            bRet = swFace2.Select4(true, null);

            Mate2 mate = swAssy.AddMate3((int)swMateType_e.swMateDISTANCE,
                (int)swMateAlign_e.swMateAlignALIGNED,
                false, distance, distance, distance, 0, 0, 0, 0, 0, false, out int errorCode);

            swModel.ForceRebuild3(false);
        }
    }
}
