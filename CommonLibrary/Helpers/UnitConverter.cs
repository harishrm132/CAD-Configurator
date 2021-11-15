using CommonLibrary.Supports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Helpers
{
    public static class UnitConverter
    {
        public static Units SketchUnits { get; set; } = Units.MM_to_M;

        public static double ConvUnits(this double val)
        {
            switch (SketchUnits)
            {
                case Units.MM_to_M:
                    return val / 1000;
                case Units.MM_to_MM:
                    return val;
                default:
                    return val;
            }

        }
    }
}
