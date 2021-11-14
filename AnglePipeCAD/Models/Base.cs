using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnglePipeCAD.Models
{
    public abstract class Base
    {
        public string FileName { get; set; }
        public string TargetFolder { get; set; }

        public abstract void CreateSldWorks();

        public abstract void CreateCatia();
    }
}
