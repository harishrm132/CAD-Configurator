using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyTreeCAD.Models
{
    public class SwCompTree 
    {
        public int No { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string AssemblyItemName { get; set; }
        public string ItemPath { get; set; }
        public string ItemType { get; set; }
        public string ItemConfig { get; set; }
        public long ItemLevel { get; set; }
        public int Quantity { get; set; }

    }
}
