using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidworksCore.Helpers
{
    public static class UnitConverter
    {
        public static SwUnits SketchUnits { get; set; } = SwUnits.MM_to_M;

        public static double ConvUnits(this double val)
        {
            switch (SketchUnits)
            {
                case SwUnits.MM_to_M:
                    return val / 1000;
                case SwUnits.MM_to_MM:
                    return val;
                default:
                    return val;
            }

        }
    }


}
