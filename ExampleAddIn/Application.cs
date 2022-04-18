using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Media.Imaging;

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
            RibbonPanel ribbonPanelIFC = application.CreateRibbonPanel(tabName, "IFC Tools");

            // Get dll assembly path
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            /*
             * CreateButton Helper!
             * Name is the name of PushButtonData
             * Text is the text under the button not necessary
             * AssemblyName is something
             * ClassName is the path/ClassName the button runs
             * ToolTip is a helper for the user to use the button
             * PathForImage is the path to the logo used for the button
             * RibbonPanelName is the name of the Ribbon the button should be in
             */

            Helper.CreateButton(
                "cmdExportIFC", 
                "Export IFC", 
                thisAssemblyPath, 
                "ExampleAddIn.ExportIFC", 
                "Go to 3D view You want to export", 
                "D:/WorkspacesCsharp/EivindsAddins/ExampleAddIn/Logos/IFC Export Icon.png",
                ribbonPanelIFC
                );

            Helper.CreateButton(
                "cmdIFCSettings",
                "Settings",
                thisAssemblyPath,
                "ExampleAddIn.IFCSettings",
                "Settings for IFC Export",
                "D:/WorkspacesCsharp/EivindsAddins/ExampleAddIn/Logos/Settings for IFC Export.png",
                ribbonPanelIFC
                );


            // create push button for Export to IFC
            //PushButtonData b1Data = new PushButtonData(
            //    "cmdExportIFC",
            //    "Export IFC",
            //    thisAssemblyPath,
            //    "ExampleAddIn.ExportIFC");

            //PushButton pb1 = ribbonPanel.AddItem(b1Data) as PushButton;
            //pb1.ToolTip = "Go to 3D view You want to export";
            //BitmapImage pb1Image = new BitmapImage(new Uri("D:/WorkspacesCsharp/EivindsAddins/ExampleAddIn/Logos/IFC Export Icon.png"));
            //pb1.LargeImage = pb1Image;
            return Result.Succeeded;
        }
    }

}
