using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidworksCore.Helpers
{
    public static class SelectionHelper
    {
        public static List<T> GetSelectedObjects<T>(this ModelDoc2 swModel, int selectionMark = -1)
        {

            if (swModel == null)
                throw new ArgumentNullException(nameof(swModel));

            var list = new List<T>();
            var selectionMgr = swModel.SelectionManager as SelectionMgr;
            int selectedobjectsCount = selectionMgr.GetSelectedObjectCount2(selectionMark);
            if (selectedobjectsCount == 0)
                return new List<T>();
            else
            {
                for (int i = 1; i <= selectedobjectsCount; i++)
                {

                    var obj = default(T);
                    try
                    {
                        obj = (T)selectionMgr.GetSelectedObject6(i, selectionMark);

                    }
                    catch (InvalidCastException)
                    {
                        obj = default(T);
                    }

                    if (EqualityComparer<T>.Default.Equals(obj) == false)
                        list.Add(obj);

                }
            }
            return list;

        }
    }
}
}
