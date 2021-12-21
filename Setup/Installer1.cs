using Autodesk.RevitAddIns;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace Setup
{
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer
    {
        public Installer1()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
            //create a new addin manifest
            RevitAddInManifest Manifest = new RevitAddInManifest();

            //create an external command
            RevitAddInCommand command1 = new RevitAddInCommand("full path\\assemblyName.dll",
                 Guid.NewGuid(), "namespace.className", "Eivind");
            command1.Description = "description";
            command1.Text = "display text";

            // this command only visible in Revit MEP, Structure, and only visible 
            // in Project document or when no document at all
            command1.Discipline = Discipline.Mechanical | Discipline.Electrical |
                                    Discipline.Piping | Discipline.Structure;
            command1.VisibilityMode = VisibilityMode.NotVisibleInFamily;

            //create an external application
            RevitAddInApplication application1 = new RevitAddInApplication("appName",
                "full path\\assemblyName.dll", Guid.NewGuid(), "namespace.className", "Eivind");

            //add both command(s) and application(s) into manifest
            Manifest.AddInCommands.Add(command1);
            Manifest.AddInApplications.Add(application1);

            //save manifest to a file
            RevitProduct revitProduct1 = RevitProductUtility.GetAllInstalledRevitProducts()[0];
            Manifest.SaveAs(revitProduct1.AllUsersAddInFolder + "\\RevitAddInUtilitySample.addin");
        }
    }
}
