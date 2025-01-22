
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitExampleIFC
{
    [Transaction(TransactionMode.Manual)]
    public class CommandDeleteLinkByPath : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uIDocument = commandData.Application.ActiveUIDocument;
            Document document = uIDocument.Document;

            string nameFileIfc = "building1.ifc";
            string pathFileIfc = @"C:\Users\lebro\Desktop\Модели Revit для программирования\building1.ifc.RVT";

            List<RevitLinkType> listLinkTypeByName = new FilteredElementCollector(document)
                .OfClass(typeof(RevitLinkType))
                .Where(x => x.Name == nameFileIfc)
                .Cast<RevitLinkType>()
                .ToList();

            foreach(RevitLinkType linkType in listLinkTypeByName)
            {
                Document document1 = linkType.Document;
                string path = document1.PathName;

                Dictionary<ExternalResourceType, ExternalResourceReference> resourceLink = 
                    (Dictionary<ExternalResourceType, ExternalResourceReference>)linkType.GetExternalResourceReferences();
                ExternalResourceType key = resourceLink.Keys.FirstOrDefault();
                ExternalResourceReference resourceReference = resourceLink[key];
                string pathIfc =resourceReference.InSessionPath;
                if(new FileInfo(pathIfc).FullName == new FileInfo(pathFileIfc).FullName)
                {
                    using(Transaction transaction = new Transaction(document, "Delete"))
                    {
                        transaction.Start();
                        document.Delete(linkType.Id);
                        FileInfo fileInfo = new FileInfo(pathFileIfc);
                        fileInfo.Delete();
                        transaction.Commit();
                    }
                }

            }


            return Result.Succeeded;
        }
    }
}
