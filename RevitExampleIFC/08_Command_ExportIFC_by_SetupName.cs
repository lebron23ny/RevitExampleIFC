using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.IFC;
using Autodesk.Revit.UI;
using BIM.IFC.Export.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitExampleIFC
{
    [Transaction(TransactionMode.Manual)]
    public class Command_ExportIFC_by_SetupName : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            string exportDir = @"C:\Users\lebro\Desktop\тест Revit";
            string nameFile = "model1.ifc";

            //Define Document
            Document doc = commandData.Application.ActiveUIDocument.Document;

            //Create an Instance of the IFC Export Class
            IFCExportOptions IFCExportOptions = new IFCExportOptions();

            //Create an instance of the IFC Export Configuration Class
            IFCExportConfiguration myIFCExportConfiguration = IFCExportConfiguration.CreateDefaultConfiguration();
            
            //Apply the IFC Export Setting (Those are equivalent to the Export Setting in the IFC Export User Interface)
            //myIFCExportConfiguration.VisibleElementsOfCurrentView = true;

            
            

            ////Define the of a 3d view to export
            ElementId ExportViewId = ElementId.InvalidElementId;
            using(Transaction tr = new Transaction(doc, "Modify"))
            {
                tr.Start();
                ////Pass the setting of the myIFCExportConfiguration to the IFCExportOptions
                myIFCExportConfiguration.UpdateOptions(IFCExportOptions, ExportViewId);
                doc.Export(exportDir, nameFile, IFCExportOptions);
                tr.Commit();
            }
            

            

            //Process the IFC Export
            
            return Result.Succeeded;
        }

        
    }



}
