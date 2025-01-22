using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitExampleIFC
{
    [Transaction(TransactionMode.Manual)]
    public class CommandMoveAndRotateIFCbySelect : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uIDocument = commandData.Application.ActiveUIDocument;
            Document document = uIDocument.Document;

            Selection selection = uIDocument.Selection;

            Reference elementRef = null;
            try
            {
                elementRef = selection.PickObject(ObjectType.Element, "Выберите ifc");
            }
            catch (OperationCanceledException e)
            {
                return Result.Cancelled;
            }

            Element selectedElement = document.GetElement(elementRef);
            if (selectedElement != null && selectedElement is RevitLinkInstance)
            {
                using (Transaction transaction = new Transaction(document))
                {
                    transaction.Start("Move");

                    Location location = selectedElement.Location;
                    location.Move(new XYZ(0, 0, 10));
                    Line line = Line.CreateBound(
                        new XYZ(0,0,0),
                        new XYZ(0,0,10));
                    location.Rotate(line, Math.PI*45/180);
                    document.Regenerate();

                    transaction.Commit();
                }
            }

            return Result.Succeeded;
        }
    }
}
