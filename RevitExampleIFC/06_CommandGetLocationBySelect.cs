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
    public class CommandGetLocationBySelect : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uIDocument = commandData.Application.ActiveUIDocument;
            Document document = uIDocument.Document;

            Selection selection = uIDocument.Selection;

            Reference elementRef = null;


            try
            {
                while(true)
                {
                    elementRef = selection.PickObject(ObjectType.Element, "Выберите ifc");
                    Element selectedElement = document.GetElement(elementRef);

                    if (selectedElement != null && selectedElement is RevitLinkInstance)
                    {
                        Transform transform = (selectedElement as RevitLinkInstance).GetTotalTransform();
                        string name = (selectedElement as RevitLinkInstance).Name;
                        XYZ origin = transform.Origin;
                        TaskDialog.Show($"Положение: {name}", $"Позиция:" +
                            $"\n X={UnitUtils.ConvertFromInternalUnits(origin.X, UnitTypeId.Millimeters)}" +
                            $"\n Y={UnitUtils.ConvertFromInternalUnits(origin.Y, UnitTypeId.Millimeters)}" +
                            $"\n Z={UnitUtils.ConvertFromInternalUnits(origin.Z, UnitTypeId.Millimeters)}");
                    }
                }
                
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException e)
            {
                TaskDialog.Show("Исключение", e.Message);
                return Result.Cancelled;
            }

            

            return Result.Succeeded;
        }
    }
}
