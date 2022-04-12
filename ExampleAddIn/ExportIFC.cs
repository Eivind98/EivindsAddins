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
                IFCExportOptions IFCExportOptions = new IFCExportOptions();

                //Create an instance of the IFC Export Configuration Class
                BIM.IFC.Export.UI.IFCExportConfiguration myIFCExportConfiguration = BIM.IFC.Export.UI.IFCExportConfiguration.CreateDefaultConfiguration();

                //Apply the IFC Export Setting (Those are equivalent to the Export Setting in the IFC Export User Interface)
                myIFCExportConfiguration.VisibleElementsOfCurrentView = true;



                //Define the of a 3d view to export
                ElementId ExportViewId = doc.ActiveView.Id;

                //Pass the setting of the myIFCExportConfiguration to the IFCExportOptions
                myIFCExportConfiguration.UpdateOptions(IFCExportOptions, ExportViewId);

                //Define the output Directory for the IFC Export
                string directory = Path.GetDirectoryName(doc.PathName);

                using (var tx = new Transaction(doc))
                {

                    tx.Start("Exporting to IFC");

                    //var els = GetLinkedElements(doc);

                    //ShowMessage("Linked: " + string.Join(", ", els));
                    
                    //Process the IFC Export
                    doc.Export(directory, $"{Guid.NewGuid()}-{doc.Title}", IFCExportOptions);

                    tx.RollBack();
                }



            }

            return Result.Succeeded;
        }

        private void ShowMessage(string message)
        {
            TaskDialog mainDialog = new TaskDialog("Hello, Revit!");
            mainDialog.MainInstruction = "Hello, Revit!";
            mainDialog.MainContent = message;

            // Add commmandLink options to task dialog
            mainDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink1,
                                      "View information about the Revit installation");
            mainDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink2,
                                      "View information about the active document");

            // Set common buttons and default button. If no CommonButton or CommandLink is added,
            // task dialog will show a Close button by default
            mainDialog.CommonButtons = TaskDialogCommonButtons.Close;
            mainDialog.DefaultButton = TaskDialogResult.Close;

            TaskDialogResult tResult = mainDialog.Show();

        }


        private IEnumerable<Element> GetLinkedElements(Document doc)
        {
            var collector = new FilteredElementCollector(doc);
            return collector.Where(x => x.GetType() == typeof(RevitLinkType));
        }

    }
        


        //public Result Execute(
        //    ExternalCommandData commandData,
        //    ref string message,
        //    ElementSet highlightElements)
        //{
        //    // retrieve all link elements:
            
        //    Document doc = app.ActiveUIDocument.Document;
        //    List<Element> links = GetElements(
        //      BuiltInCategory.OST_RvtLinks,
        //      typeof(Instance), app, doc);


            

        //}





    //    public List<Element> FindingLinkedElements(ExternalCommandData commandData)
    //    {

    //        var data = new List<ElementData>();
    //        var app = commandData.Application;
    //        var docs = app.Application.Documents;

    //        // retrieve all link elements:
    //        Document doc = app.ActiveUIDocument.Document;
    //        List<Element> links = GetElements(BuiltInCategory.OST_RvtLinks, typeof(Instance), app, doc);

    //        LinkElementId el = LinkElementId;
            
    //        return links;
    //    }
    //}


    //public class ElementData
    //{
    //    private readonly double _x;
    //    private readonly double _y;
    //    private readonly double _z;

    //    public ElementData(
    //        string path,
    //        string elementName,
    //        int id,
    //        double x,
    //        double y,
    //        double z,
    //        string uniqueId)
    //    {
    //        var i = path.LastIndexOf("\\");
    //        Document = path.Substring(i + 1);
    //        Element = elementName;
    //        Id = id;
    //        _x = x;
    //        _y = y;
    //        _z = z;
    //        UniqueId = uniqueId;
    //        Folder = path.Substring(0, i);
    //    }

    //    public string Document { get; }

    //    public string Element { get; }

    //    public int Id { get; }

    //    public string UniqueId { get; }

    //    public string Folder { get; }
    //}



}
