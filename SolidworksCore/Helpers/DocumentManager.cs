using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace SolidworksCore.Helpers
{
    public static class DocumentManager
    {
        public static void CreatePartDoc(this SldWorks swApp)
        {
            string defaultPartTemplate = swApp.GetUserPreferenceStringValue((int)swUserPreferenceStringValue_e.swDefaultTemplatePart);
            swApp.NewDocument(defaultPartTemplate, 0, 0, 0);
        }

        public static SldWorks CreateAssemblyDoc(string targetFolder, string fileName)
        {
            SldWorks swApp = SolidWorksSingleton.GetApplication();
            string defaultAssemblyTemplate = swApp.GetUserPreferenceStringValue((int)swUserPreferenceStringValue_e.swDefaultTemplateAssembly);
            swApp.NewDocument(defaultAssemblyTemplate, 0, 0, 0);
            ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;
            swModel.SaveAs3($"{targetFolder}\\{fileName}.SLDASM",
                (int)swSaveAsVersion_e.swSaveAsCurrentVersion,
                (int)swSaveAsOptions_e.swSaveAsOptions_CopyAndOpen);
            return swApp;
        }

        public static SldWorks CreateDrawingDoc()
        {
            SldWorks swApp = SolidWorksSingleton.GetApplication();
            string defaultdrawingTemplate = swApp.GetUserPreferenceStringValue((int)swUserPreferenceStringValue_e.swDefaultTemplateDrawing);
            swApp.NewDocument(defaultdrawingTemplate, 0, 0, 0);
            swApp.Visible = true;
            return swApp;
        }

        public static void Save(this SldWorks swApp, string targetFolder, string fileName, sw_DocType docType)
        {
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            string extn = "";
            switch (docType)
            {
                case sw_DocType.Part:
                    extn = ".SLDPRT";
                    break;
                case sw_DocType.Assembly:
                    extn = ".SLDASM";
                    break;
                case sw_DocType.Drawing:
                    extn = ".SLDDRW";
                    break;
                default:
                    break;
            }

            ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;
            swModel.SaveAs3($"{targetFolder}\\{fileName}{extn}",
                (int)swSaveAsVersion_e.swSaveAsCurrentVersion,
                (int)swSaveAsOptions_e.swSaveAsOptions_CopyAndOpen);
            swApp.CloseDoc($"{fileName}{extn}");
        }

    }

    public enum sw_DocType
    {
        Part,
        Assembly,
        Drawing
    }
}
