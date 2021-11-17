using AssemblyTreeCAD.Models;
using CatiaCore;
using CommonLibrary.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyTreeCAD
{
    public class CaAssemblyTree
    {
        public CaAssemblyTree()
        {
            Initializr_AssemblyTree assemblyTree = new Initializr_AssemblyTree();
            foreach (CompTree vbComp in assemblyTree.CompList)
            {
                CompList.Add(new CaCompTree()
                {
                    ItemName = vbComp.ItemName,
                    ItemType = vbComp.ItemType,
                    ItemLevel = vbComp.ItemLevel,
                    Quantity = vbComp.Quantity
                });
            }
            ExportFileName = assemblyTree.ExportFileName;
        }

        public List<CaCompTree> CompList { get; set; }

        public string ExportFileName { get; set; }

        public void Export()
        {
            CompList.ExportCsv(ExportFileName);
        }
    }
}
