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

            var view = commandData.Application.ActiveUIDocument.ActiveView as View3D;

            var instance = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance));

            IList<Element> elementsInActiveView = new FilteredElementCollector(doc, ViewId).OfCategory(BuiltInCategory.OST_Doors).ToElements();






            //IList<ElementId> elementsInActive = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Doors).ToElementIds();





            for (int i = 0; i < elementsInActiveView.Count; i++)
            {



                

                    ElementId ele = elementsInActiveView[i].Id;
                    FamilyInstance elementInstance = (FamilyInstance)doc.GetElement(ele);

                    Transaction tx = new Transaction(doc, "Do Door Stuff");
                    tx.Start();

                    try
                    {

                        Parameter pa = elementInstance.LookupParameter("H/V");

                        if (elementInstance.HandFlipped)
                        {
                            pa.Set("H");
                        }
                        else
                        {
                            pa.Set("V");
                        }
                        tx.Commit();
                    }
                    catch
                    {
                        tx.RollBack();
                    }


                
                



            }

            TaskDialog.Show("Yoyo", string.Join("\n", elementsInActiveView));










            return Result.Succeeded;
        }






    }
}
