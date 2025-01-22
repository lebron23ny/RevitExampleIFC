using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitExampleIFC
{
    [Transaction(TransactionMode.Manual)]
    public class CommandGetRVTLinks : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uIDocument = commandData.Application.ActiveUIDocument;
            Document document = uIDocument.Document;

            string nameFileIfc = "building1.ifc";
            string pathFileIfc = @"C:\Users\lebro\Desktop\Модели Revit для программирования\building1.ifc.RVT";

            List<RevitLinkType> listLinkType = new FilteredElementCollector(document)
                .OfClass(typeof(RevitLinkType))
                .Cast<RevitLinkType>()
                .ToList();
            
            List<RevitLinkType> listLinkTypeByName = new FilteredElementCollector(document)
                .OfClass(typeof(RevitLinkType))
                .Where(x=>x.Name == nameFileIfc)
                .Cast<RevitLinkType>()
                .ToList();
         
            var listLinkInstance = new FilteredElementCollector(document)
                .OfClass(typeof(RevitLinkInstance)).Where(x=>x.Name.Contains("ifc")).ToList(); 


            
            foreach(var link in listLinkType)
            {

                RevitLinkType rvtLinkCurrent = link as RevitLinkType;
                Document rvtDocument = rvtLinkCurrent.Document;
                string path = rvtDocument.PathName;

                string name_rvt_link = rvtLinkCurrent.Name;
              
                var resourcesRefs = rvtLinkCurrent.GetExternalResourceReferences();
                ExternalResourceType key = resourcesRefs.Keys.FirstOrDefault();

                ExternalResourceReference value = resourcesRefs[key];
                string pathIfc = value.InSessionPath;               
            }

            foreach(var linkInstance in listLinkInstance)
            {
                RevitLinkInstance linkInstanceCurrent = linkInstance as RevitLinkInstance;
                string name_rvt_ = linkInstanceCurrent.Name;
                
                Location location = linkInstanceCurrent.Location;
                
                
                var linkDoc = linkInstanceCurrent.GetLinkDocument();
                string name_ = linkDoc.PathName;
                var name = linkDoc.Title;
                
            }
            ;
            return Result.Succeeded;
        }
    }
}
