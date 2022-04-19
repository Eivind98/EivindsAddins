using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.IFC;
using Autodesk.Revit.UI;

namespace ExampleAddIn
{

    [TransactionAttribute(TransactionMode.Manual)]
    public class IFCSettings : Autodesk.Revit.UI.IExternalCommand
    {


        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            ElementId ViewId = doc.ActiveView.Id;

            
            


            return Result.Succeeded;
        }






    }
}
