using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitExampleIFC
{
    [Transaction(TransactionMode.Manual)]
    public class CommandMoveIFCbyPath : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uIDocument = commandData.Application.ActiveUIDocument;
            Document document = uIDocument.Document;

            string nameFileIfc = "building1.ifc";
            string pathFileIfc = @"C:\Users\lebro\Desktop\Модели Revit для программирования\building1.ifc.RVT";

            var listLinkInstance = new FilteredElementCollector(document)
                .OfClass(typeof(RevitLinkInstance)).Where(x => x.Name.Contains(nameFileIfc)).ToList();

            foreach (var linkInstance in listLinkInstance)
            {
                RevitLinkInstance linkInstanceCurrent = linkInstance as RevitLinkInstance;
                var linkDoc = linkInstanceCurrent.GetLinkDocument();
                string pathCurrent = linkDoc.PathName;
                if(new FileInfo(pathCurrent).FullName == new FileInfo(pathFileIfc).FullName)
                {
                    using(Transaction transaction = new Transaction(document))
                    {
                        transaction.Start("Move");

                        Location location = linkInstanceCurrent.Location;
                        location.Move(new XYZ(0, 0, 10));
                        document.Regenerate();

                        transaction.Commit();
                    }
                }

            }

            return Result.Succeeded;
        }

    }



    public class SeparatorSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            return elem is FamilyInstance instance && instance.Symbol.FamilyName == "ADSK_Балка_Двутавр_ГОСТ Р 57837-2017";
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return true;
        }
    }
}
