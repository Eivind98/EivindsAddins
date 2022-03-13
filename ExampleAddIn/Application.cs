using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ExampleAddIn
{

    [TransactionAttribute(TransactionMode.Manual)]
    public class Application : Autodesk.Revit.UI.IExternalApplication
    {

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            // Create a custom ribbon tab
            String tabName = "Eivind";
            application.CreateRibbonTab(tabName);

            // Add a new ribbon panel
            RibbonPanel ribbonPanel = application.CreateRibbonPanel(tabName, "Tools");

            // Get dll assembly path
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            // create push button for Export to IFC
            PushButtonData b1Data = new PushButtonData(
                "cmdExportIFC",
                "Export IFC",
                thisAssemblyPath,
                "ExampleAddIn.ExportIFC");

            PushButton pb1 = ribbonPanel.AddItem(b1Data) as PushButton;
            pb1.ToolTip = "Go to 3D view You want to export";
            //BitmapImage pb1Image = new BitmapImage(new Uri("pack://application:,,,/GrimshawRibbon;component/Resources/totalLength.png"));
            //pb1.LargeImage = pb1Image;
            return Result.Succeeded;
        }
    }

}
