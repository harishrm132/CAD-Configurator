using AssemblyTreeCAD.Models;
using CommonLibrary.DataAccess;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidworksCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyTreeCAD
{
    public class SwAssemblyTree
    {
        public SwAssemblyTree()
        {
            SldWorks swApp = SolidWorksSingleton.GetApplication();
            ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;
            //AssemblyDoc swAssemblyDoc = (AssemblyDoc)swModel;
            Configuration swConfig = swModel.ConfigurationManager.ActiveConfiguration;
            Component2 swRootComp = swConfig.GetRootComponent3(true);

            CompList = new List<SwCompTree>();
            TraverseComponent(swRootComp, 1);
            ExportFileName = swModel.GetTitle().Split('.')[0];
        }

        public List<SwCompTree> CompList { get; set; }

        public string ExportFileName { get; set; }

        public void Export()
        {
            CompList.ExportCsv(ExportFileName);
        }

        private void TraverseComponent(Component2 swComp, long nLevel)
        {
            string sPadStr = " ";
            long i = 0;
            for (i = 0; i <= nLevel - 1; i++)
            {
                sPadStr = sPadStr + " ";
            }


            object[] vChildComps = swComp.GetChildren();
            int no = 1;
            foreach (var vComp in vChildComps)
            {
                Component2 swChildComp = (Component2)vComp;
                string itemType = "";
                ModelDoc2 model = swChildComp.GetModelDoc2();
                if (model != null)
                {
                    if (model.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY) itemType = "Assembly";
                    else itemType = "Part";
                }
                SwCompTree comp = new SwCompTree()
                {
                    ItemId = swChildComp.GetID(),
                    ItemName = model != null ? model.GetTitle().Split('.')[0] : swChildComp.Name.Split('-')[0].Trim(),
                    AssemblyItemName = swChildComp.Name,
                    ItemLevel = nLevel,
                    ItemPath = swChildComp.GetPathName(),
                    ItemType = itemType,
                    ItemConfig = swChildComp.ReferencedConfiguration,
                    Quantity = 1,
                };

                if (CompList.Any(x => x.ItemName == comp.ItemName))
                    CompList.First(x => x.ItemName == comp.ItemName).Quantity += 1;
                else
                {
                    comp.No = no++;
                    CompList.Add(comp);
                }


                TraverseComponent(swChildComp, nLevel + 1);
            }
        }

    }
}
