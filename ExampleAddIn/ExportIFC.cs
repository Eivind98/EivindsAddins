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
    public class ExportIFC : Autodesk.Revit.UI.IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            // Get the active view of the current document.
            var view = commandData.View.Document.ActiveView;

            // Get the class type of the active view, and format the prompt string
            if (view is View3D)
            {

                UIApplication uiapp = commandData.Application;
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document;

                //Create an Instance of the IFC Export Class
                IFCExportOptions IFCExportOpt = new IFCExportOptions();

                //assigning IFC export option to use IFC4 
                IFCExportOpt.FileVersion = IFCVersion.IFC4;

                //Create an instance of the IFC Export Configuration Class
                BIM.IFC.Export.UI.IFCExportConfiguration myIFCExportConfiguration = BIM.IFC.Export.UI.IFCExportConfiguration.CreateDefaultConfiguration();

                //Apply the IFC Export Setting (Those are equivalent to the Export Setting in the IFC Export User Interface)
                myIFCExportConfiguration.VisibleElementsOfCurrentView = true;

                //Define the of a 3d view to export
                ElementId ExportViewId = doc.ActiveView.Id;

                //Pass the setting of the myIFCExportConfiguration to the IFCExportOptions
                myIFCExportConfiguration.UpdateOptions(IFCExportOpt, ExportViewId);

                //Define the output Directory for the IFC Export
                string directory = Path.GetDirectoryName(doc.PathName) + "/Export/IFC";

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (var tx = new Transaction(doc))
                {

                    tx.Start("Exporting to IFC");

                    
                    doc.Export(directory, $"{view.Name}", IFCExportOpt);

                    tx.RollBack();
                }

            }
            else
            {
                
                TaskDialog.Show("Friendly Reminder", "Needs a 3D view to work");
            }

            return Result.Succeeded;
        }
    }
}
