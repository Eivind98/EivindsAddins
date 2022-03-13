using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.IFC;
using Autodesk.Revit.UI;

namespace ExampleAddIn
{

    [TransactionAttribute(TransactionMode.Manual)]
    public class ExportIFC : Autodesk.Revit.UI.IExternalCommand
    {

        
        


        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            // Get the active view of the current document.
            View view = commandData.View.Document.ActiveView;

            // Get the class type of the active view, and format the prompt string
            if (view is View3D)
            {
                
                UIApplication uiapp = commandData.Application;
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document;

                //Create an Instance of the IFC Export Class
                IFCExportOptions IFCExportOptions = new IFCExportOptions();

                //Create an instance of the IFC Export Configuration Class
                BIM.IFC.Export.UI.IFCExportConfiguration myIFCExportConfiguration = BIM.IFC.Export.UI.IFCExportConfiguration.CreateDefaultConfiguration();

                //Apply the IFC Export Setting (Those are equivalent to the Export Setting in the IFC Export User Interface)
                myIFCExportConfiguration.VisibleElementsOfCurrentView = true;



                //Define the of a 3d view to export
                ElementId ExportViewId = null;

                //Pass the setting of the myIFCExportConfiguration to the IFCExportOptions
                myIFCExportConfiguration.UpdateOptions(IFCExportOptions, ExportViewId);

                //Define the output Directory for the IFC Export
                string Directory = doc.PathName;

                //Process the IFC Export
                doc.Export(Directory, doc.Title, IFCExportOptions);
            }

            return Result.Succeeded;
        }

        public void Export(View view)
        {

        }
    }

}
