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

            var view = commandData.Application.ActiveUIDocument.ActiveView as View3D;

            Transaction tx = new Transaction(doc, "Getting Elements in View");
            tx.Start();
            view.EnableRevealHiddenMode();
            IList<Element> elementsInActiveView = new FilteredElementCollector(doc, ViewId).OfCategory(BuiltInCategory.OST_Walls).ToElements();
            view.EnableRevealHiddenMode();

            tx.RollBack();
            

            DateTime nowDate = DateTime.Parse("18-04-2022");

            

            List<Element> hideElements = new List<Element>();
            List<Element> showElements = new List<Element>();
            List<Element> transElements = new List<Element>();

            


            TaskDialog.Show("YoyO", Helper.LookupParam(elementsInActiveView[0], "Start date") + "\n" 
                + elementsInActiveView.Count.ToString() + "\n" 
                + (DateTime.Parse(Helper.LookupParam(elementsInActiveView[0], "Start date")) < nowDate).ToString() + "\n"
                + (DateTime.Parse(Helper.LookupParam(elementsInActiveView[0], "Start date")) > nowDate).ToString());
            


            for (int i = 0; i < elementsInActiveView.Count; i++)
            {
                DateTime startDate;
                DateTime endDate;

                bool startBool = DateTime.TryParse(Helper.LookupParam(elementsInActiveView[i], "Start date"), out startDate);
                bool endBool = DateTime.TryParse(Helper.LookupParam(elementsInActiveView[i], "End date"), out endDate);

                if (!startBool)
                {
                    startDate = DateTime.MinValue;
                }
                if (!endBool)
                {
                    endDate = DateTime.MinValue;
                }

                if (startDate > nowDate)
                {
                    


                    hideElements.Add(elementsInActiveView[i]);

                }
                else if (endDate < nowDate)
                {
                    showElements.Add(elementsInActiveView[i]);
                }
                else
                {
                    transElements.Add(elementsInActiveView[i]);
                }

            }


            OverrideGraphicSettings overrideGraphicSettingsActive = new OverrideGraphicSettings();
            Color color = new Color(255, 0, 0);
            ElementId pat = Helper.GetSolidSolidFillPattern(doc);

            


            overrideGraphicSettingsActive.SetSurfaceForegroundPatternColor(color);
            overrideGraphicSettingsActive.SetSurfaceForegroundPatternId(pat);

            TaskDialog.Show("Something1", string.Join("\n", hideElements));
            TaskDialog.Show("Something1", string.Join("\n", transElements));
            TaskDialog.Show("Something1", string.Join("\n", showElements));




            Transaction trans = new Transaction(doc, "Make Note UPPER");
            trans.Start();


            Helper.OverrideElem(view, transElements, overrideGraphicSettingsActive);
            
            Helper.UnHideElem(view, showElements);

            Helper.HideElem(view, hideElements);


            //var hideIds = new List<ElementId>();
            //foreach (var id in ids)
            //{
            //    if (!visibleElementIds.Contains(id))
            //    {
            //        hideIds.Add(id);
            //    }
            //}

            //using (var tran = new Transaction(doc, "Test"))
            //{
            //    tran.Start();
            //    var view = commandData.Application.ActiveUIDocument.ActiveView as View3D;
            //    if (view != null)
            //    {
            //        view.HideElements(hideIds);
            //    }
            //    tran.Commit();
            //}






            trans.Commit();
            trans.Dispose();


            return Result.Succeeded;
        }






    }
}
