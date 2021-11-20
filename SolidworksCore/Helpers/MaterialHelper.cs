using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SolidworksCore.Helpers
{
    public static class MaterialHelper
    {
        public static List<string> GetSwMaterials(this SldWorks swApp)
        {
            if(swApp == null)
                throw new ArgumentNullException(nameof(swApp));

            if (swApp.GetMaterialDatabaseCount() == 0)
                return new List<string>();

            var materials = new List<string>();
            var matDBObjs = swApp.GetMaterialDatabases() as object[];
            foreach (var matDBObj in matDBObjs)
            {
                string dbPath = matDBObj as string;
                materials.AddRange(GetMaterialNamesSwMatDB(dbPath));
            }

            return materials;
        }

        public static List<string> GetMaterialNamesSwMatDB(string dbPathName)
        {
            if (string.IsNullOrWhiteSpace(dbPathName))
                throw new ArgumentException("PathName is null or empty space.");

            if (!File.Exists(dbPathName))
                return new List<string>();

            List<string> mats = new List<string>();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;
            XmlReader reader = XmlReader.Create(dbPathName, settings);
            while (reader.Read())
            {
                if(reader.Name.ToUpper() == "MATERIAL")
                {
                    string mat = reader.GetAttribute("name");
                    if (!string.IsNullOrEmpty(mat))
                        mats.Add(mat);
                }
            }

            return mats;
        }
    }
}
