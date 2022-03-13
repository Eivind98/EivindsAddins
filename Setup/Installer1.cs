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
            CreateExampleAddInManifest();
            CreateBuildingCoderManifest();
            
        }

        public void CreateExampleAddInManifest() 
        {
            //create a new addin manifest
            RevitAddInManifest Manifest = new RevitAddInManifest();


            string path = this.Context.Parameters["targetdir"];


            //create an external application
            RevitAddInApplication application1 = new RevitAddInApplication(
                "Eivind test",
                $@"{path}ExampleAddin.dll",
                Guid.NewGuid(),
                "ExampleAddIn.Application",
                "Eivind");

            //add both command(s) and application(s) into manifest
            Manifest.AddInApplications.Add(application1);

            //save manifest to a file
            RevitProduct revitProduct1 = RevitProductUtility.GetAllInstalledRevitProducts()[0];
            Manifest.SaveAs(revitProduct1.AllUsersAddInFolder + "\\RevitAddInUtilitySample.addin");
        }

        public void CreateBuildingCoderManifest()
        {
            //create a new addin manifest
            RevitAddInManifest Manifest = new RevitAddInManifest();


            string path = this.Context.Parameters["targetdir"];


            //create an external application
            RevitAddInApplication application1 = new RevitAddInApplication(
                "Eivind test",
                $@"{path}BuildingCoder.dll",
                Guid.NewGuid(),
                "BuildingCoder.CmdDemoCheck",
                "Eivind");

            RevitAddInApplication application2 = new RevitAddInApplication(
                "Eivind test",
                $@"{path}BuildingCoder.dll",
                Guid.NewGuid(),
                "BuildingCoder.CmdNewArea",
                "Eivind");

            //add both command(s) and application(s) into manifest
            Manifest.AddInApplications.Add(application1);

            Manifest.AddInApplications.Add(application2);

            //save manifest to a file
            RevitProduct revitProduct1 = RevitProductUtility.GetAllInstalledRevitProducts()[0];
            Manifest.SaveAs(revitProduct1.AllUsersAddInFolder + "\\BuildingCoder.addin");
        }

    }
}
