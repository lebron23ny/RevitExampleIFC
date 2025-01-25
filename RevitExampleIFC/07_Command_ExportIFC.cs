using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.IFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RevitExampleIFC
{
    [Transaction(TransactionMode.Manual)]
    public class Command_ExportIFC : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Получаем активный документ
            Document doc = commandData.Application.ActiveUIDocument.Document;

            // Путь для сохранения IFC файла
            string exportDir = @"C:\Users\lebro\Desktop\тест Revit";
            string nameFile = "model.ifc";

            // Создаем объект IFCExportOptions
            IFCExportOptions ifcOptions = new IFCExportOptions
            {
                // Укажите нужные параметры, например:
                FileVersion = IFCVersion.IFC2x3,
                WallAndColumnSplitting = true
            };

            try
            {
                using (Transaction tr = new Transaction(doc, "Export IFC"))
                {
                    tr.Start();
                    var d = doc.Export(exportDir, nameFile, ifcOptions);
                    tr.Commit();
                }
                // Выполняем экспорт


                TaskDialog.Show("Успех", "Модель успешно экспортирована в IFC!");
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }
}
