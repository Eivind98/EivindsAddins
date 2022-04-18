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
    public class Helper
    {
        public static void CreateButton(
            string name, 
            string text, 
            string assemblyName, 
            string className, 
            string toolTip, 
            string pathForImage, 
            RibbonPanel ribbonPanelName
            )
        {

            PushButtonData b1Data = new PushButtonData(
                name,
                text,
                assemblyName,
                className);

            PushButton pb1 = ribbonPanelName.AddItem(b1Data) as PushButton;
            pb1.ToolTip = toolTip;
            BitmapImage pb1Image = new BitmapImage(new Uri(pathForImage));
            pb1.LargeImage = pb1Image;

        }


        public static string LookupParam(Element ele, string ParameterName)
        {
            try
            {
                IList<Parameter> ps = ele.GetParameters(ParameterName);

                List<string> param_values = new List<string>(
                  ps.Count);

                foreach (Parameter p in ps)
                {
                    // AsValueString displays the value as the 
                    // user sees it. In some cases, the underlying
                    // database value returned by AsInteger, AsDouble,
                    // etc., may be more relevant.

                    param_values.Add(p.AsValueString());
                }


                return param_values[0].ToString();
            }
            catch
            {
                return "";
            }
            

        }

        public static void HideElem(View v, List<Element> element)
        {
            
            ICollection<ElementId> xo = new List<ElementId>();
            foreach (Element e in element)
            {

                if (!e.IsHidden(v))
                {
                    xo.Add(e.Id);
                }
                

            }
            if (xo.Count > 0)
            {
                v.HideElements(xo);
            }
            



        }

        public static void UnHideElem(View v, List<Element> element)
        {
            ICollection<ElementId> yo = new List<ElementId>();
            foreach (Element e in element)
            {
                yo.Add(e.Id);


            }
            ResetOverrideElements(yo, v);


            ICollection<ElementId> xo = new List<ElementId>();
            foreach (Element e in element)
            {
                if (e.IsHidden(v))
                {
                    xo.Add(e.Id);
                }
                

            }

            



            if (xo.Count > 0)
            {
                v.UnhideElements(xo);
            }

            



        }

        public static void OverrideElem(View v, List<Element> element, OverrideGraphicSettings overrideGraphicSettings)
        {
            
            
            UnHideElem(v, element);
            ICollection<ElementId> xo = new List<ElementId>();
            foreach (Element e in element)
            {
                
                xo.Add(e.Id);

            }

            



            if (xo.Count > 0)
            {
                foreach(ElementId e in xo)
                {

                    v.SetElementOverrides(e, overrideGraphicSettings);

                }
                
                
            }





        }



        public static ElementId GetSolidSolidFillPattern(Document doc)
        {
            List<FillPatternElement> fillPatternList = new FilteredElementCollector(doc).WherePasses(new ElementClassFilter(typeof(FillPatternElement))).
                ToElements().Cast<FillPatternElement>().ToList();
            ElementId solidFillPatternId = null;

            foreach (FillPatternElement fp in fillPatternList)
            {
                if (fp.GetFillPattern().IsSolidFill)
                {
                    solidFillPatternId = fp.Id;
                    break;
                }
            }

            return solidFillPatternId;
        }


        public static void ResetOverrideElements(ICollection<ElementId> xo, View v)
        {

            OverrideGraphicSettings overrideGraphicSettings = new OverrideGraphicSettings();
            if (xo.Count > 0)
            {
                foreach (ElementId e in xo)
                {

                    v.SetElementOverrides(e, overrideGraphicSettings);

                }


            }

        }


    }
}
