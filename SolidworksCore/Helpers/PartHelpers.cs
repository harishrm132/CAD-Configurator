using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidworksCore.Helpers
{
    public static class PartHelpers
    {
        public static void ChangeEntityName(this SldWorks swApp, string entType, string entName, double CoorX, double CoorY, double CoorZ)
        {
            ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;
            swModel.Extension.SelectByID2("", entType, CoorX, CoorY, CoorZ, false, 0, null, 0);

            SelectionMgr swSelMgr = swModel.SelectionManager;
            Face2 swFace = swSelMgr.GetSelectedObject6(1, -1);

            PartDoc swPart = swApp.ActiveDoc;
            swPart.SetEntityName(swFace, entName);
        }

        public static void SimpleCut(this ModelDoc2 swModel)
        {
            Feature swFeature = swModel.FeatureManager.FeatureCut3(true, false, false,
                (int)swEndConditions_e.swEndCondThroughAll, (int)swEndConditions_e.swEndCondBlind,
                0, 0, false, false, false, false, 0, 0, false, false,
                false, false, false, true, true, false, false, false,
                (int)swStartConditions_e.swStartSketchPlane, 0, true);
        }

        public static void SimpleFillet(this ModelDoc2 swModel, double uniformRad)
        {
            Feature swFeature = swModel.FeatureManager.FeatureFillet3(
                (int)swFeatureFilletOptions_e.swFeatureFilletUniformRadius,
                uniformRad, 0, 0, 0, 0, 0, null, null, null, null, null, null, null);
        }

        public static bool IsConfigurationExist(this ModelDoc2 swModel, string configurationName)
        {
            if (configurationName == null) throw new ArgumentNullException("configurationName");

            if (swModel.GetConfigurationCount() > 0)
            {
                var configurations = (object[])swModel.GetConfigurationNames();
                for (int i = 0; i < configurations.Length; i++)
                {
                    if (configurations[i].ToString().ToLower().Trim() == configurationName.ToLower().Trim()) return true;
                }
            }

            return false;
        }

        public static T Get<T>(this ModelDoc2 swModel, string customPropName, out string value, string configuration = "Default")
        {
            if (swModel == null)
                throw new ArgumentNullException(nameof(swModel));

            string resolvedvalue = string.Empty;

            swModel.Extension.CustomPropertyManager[configuration].Get2(customPropName, out value, out resolvedvalue);

            T ret = default(T);

            try
            {
                if (typeof(T) == typeof(double))
                {
                    double v;
                    var parse = double.TryParse(resolvedvalue, out v);
                    return (T)Convert.ChangeType(v, typeof(T));
                }
                else if (typeof(T) == typeof(string))
                {
                    return (T)Convert.ChangeType(resolvedvalue, typeof(T));
                }
                else if (typeof(T) == typeof(bool))
                {
                    Boolean boo;
                    var parse = bool.TryParse(resolvedvalue, out boo);
                    return (T)Convert.ChangeType(boo, typeof(T));
                }
            }
            catch (Exception) { }

            return ret;
        }

    }
}
