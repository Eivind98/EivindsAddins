using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;

namespace AfterInstall
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var filename = @"D:\OneDrive\Skrivebord\LNA_K01_F2_APF_N01.rvt";
            var revit = $@"C:\Program Files\Autodesk\Revit 2022\Revit.exe";
            try
            {
                Process.Start(new ProcessStartInfo(revit)
                {
                    Arguments = filename,
                    UseShellExecute = false,
                    LoadUserProfile = true,
                    //UserName =  "eev_9"// WindowsIdentity.GetCurrent().Name
                });
            } catch(Exception e) {
            MessageBox.Show(e.Message);
            }
        }
    }
}
