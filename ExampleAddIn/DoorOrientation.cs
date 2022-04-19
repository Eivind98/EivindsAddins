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
    public class DoorOrientation : Autodesk.Revit.UI.IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            ElementId ViewId = doc.ActiveView.Id;

            IList<Element> elementsInActiveView = new FilteredElementCollector(doc, ViewId).OfCategory(BuiltInCategory.OST_Doors).ToElements();


            for (int i = 0; i < elementsInActiveView.Count; i++)
            {

                ElementId ele = elementsInActiveView[i].Id;
                FamilyInstance elementInstance = (FamilyInstance)doc.GetElement(ele);

                Transaction tx = new Transaction(doc, "Assign Value to Door Parameter H/V");
                tx.Start();

                try
                {
                    Parameter pa = elementInstance.LookupParameter("H/V");
                    
                    if (elementInstance.HandFlipped == elementInstance.FacingFlipped)
                    {
                        pa.Set("V");
                        
                    }
                    else
                    {
                        pa.Set("H");
                        
                    }
                    tx.Commit();
                }
                catch
                {
                    tx.RollBack();
                }
            }

            //Troubleshooting
            TaskDialog.Show("Yoyo", string.Join("\n", elementsInActiveView));


            return Result.Succeeded;
        }
    }
}
