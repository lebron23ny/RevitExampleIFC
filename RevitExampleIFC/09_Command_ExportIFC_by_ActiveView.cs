using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
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
    public class Command_ExportIFC_by_ActiveView : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            string exportDir = @"C:\Users\lebro\Desktop\";
            string nameFile = "model_Internal.ifc";

            string pathMapping = @"C:\Users\lebro\Desktop\Файлы\Revit c работы\IFC\МИ_Импорт_IfcClassMapping.txt";

            UIDocument uIDocument = commandData.Application.ActiveUIDocument;
            Document document = uIDocument.Document;
            View activeView = document.ActiveView;
            ElementId elementIdView = activeView.Id;

            
            IFCExportOptions ifcOptions = new IFCExportOptions
            {
                FileVersion = IFCVersion.IFC2x3, // Используем общепринятую версию
                ExportBaseQuantities = true, // Экспортировать количественные данные
                SpaceBoundaryLevel = 2, // Пространственные границы
                FamilyMappingFile = pathMapping,
                
            };
            //ifcOptions.FilterViewId = ElementId.InvalidElementId;
            ifcOptions.FilterViewId = elementIdView;

            ifcOptions.AddOption("SitePlacement", "3");
            ifcOptions.AddOption("VisibleElementsOfCurrentView", "1");
            //ifcOptions.AddOption("UseActiveViewGeometry", "1");
            //ifcOptions.AddOption("ExportBoundingBox", "0");
            
            using (Transaction tr = new Transaction(document, "Modify"))
            {
                tr.Start();
                ////Pass the setting of the myIFCExportConfiguration to the IFCExportOptions

                document.Export(exportDir, nameFile, ifcOptions);
                tr.Commit();
            }

            return Result.Succeeded;
        }
    }
}
