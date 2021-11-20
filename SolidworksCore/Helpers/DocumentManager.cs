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

            string extn = docType.GetSwExtension();

            ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;
            swModel.SaveAs3($"{targetFolder}\\{fileName}{extn}",
                (int)swSaveAsVersion_e.swSaveAsCurrentVersion,
                (int)swSaveAsOptions_e.swSaveAsOptions_CopyAndOpen);
            swApp.CloseDoc($"{fileName}{extn}");
        }

        public static swDocumentTypes_e GetSwDocType(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToUpper();
            if (extension.Contains(SwFileExtension.Part))
                return swDocumentTypes_e.swDocPART;
            else if (extension.Contains(SwFileExtension.Assembly))
                return swDocumentTypes_e.swDocASSEMBLY;
            else if (extension.Contains(SwFileExtension.Drawing))
                return swDocumentTypes_e.swDocDRAWING;

            return swDocumentTypes_e.swDocNONE;
        }

        public static string GetSwExtension(this sw_DocType docType)
        {
            switch (docType)
            {
                case sw_DocType.Part:
                    return SwFileExtension.Part;
                case sw_DocType.Assembly:
                    return SwFileExtension.Assembly;
                case sw_DocType.Drawing:
                    return SwFileExtension.Drawing;
                default:
                    return string.Empty;
            }
        }
    }

    public enum sw_DocType
    {
        Part,
        Assembly,
        Drawing
    }
}
