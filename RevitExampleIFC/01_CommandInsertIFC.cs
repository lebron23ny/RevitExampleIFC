using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using IFC= Autodesk.Revit.DB.IFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using System.IO;

namespace RevitExampleIFC
{
    [Transaction(TransactionMode.Manual)]
    public class CommandInsertIFC : IExternalCommand
    {
        UIApplication uIApplication;
        Application application;
        Document docIFC;
        Document thisDoc;
        UIDocument thisUIDoc;


        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            uIApplication = commandData.Application;
            application = uIApplication.Application;
            thisUIDoc = uIApplication.ActiveUIDocument;
            thisDoc = thisUIDoc.Document;

            List<string> listPathIFC = new List<string>()
            {
                @"C:\Users\lebro\Desktop\IFC\Новая модель.ifc",
                @"C:\Users\lebro\Desktop\IFC\Новая модель1_БезПривязки.ifc"
            };
            List<string> listPathIFCrvt = new List<string>()
            {
                 @"C:\Users\lebro\Desktop\IFC\Новая модель.ifc.rvt",
                  @"C:\Users\lebro\Desktop\IFC\Новая модель1_БезПривязки.ifc.rvt"
            };
            //string pathIFC = @"C:\Users\lebro\Desktop\IFC\Новая модель.ifc";
            //string pathIFCrvt = @"C:\Users\lebro\Desktop\IFC\Новая модель.ifc.rvt";

            using (Transaction transaction = new Transaction(thisDoc, "Link"))
            {
                transaction.Start();
                for (int i = 0; i < listPathIFC.Count; i++)
                {
                    GetOpenIFC(listPathIFC[i]);
                    SaveAsIFC(listPathIFCrvt[i]);
                    docIFC.Close();
                    docIFC.Dispose();

                    RevitLinkOptions revitLinkOptions = new RevitLinkOptions(false);
                    ExternalResourceReference ifc_resource = ExternalResourceReference.
                        CreateLocalResource
                        (
                            thisDoc,
                            ExternalResourceTypes.BuiltInExternalResourceTypes.IFCLink,
                            ModelPathUtils.ConvertUserVisiblePathToModelPath(listPathIFC[i]),
                            PathType.Absolute
                        );

                    LinkLoadResult result = RevitLinkType.CreateFromIFC(
                        thisDoc, ifc_resource, listPathIFCrvt[i], false, revitLinkOptions);

                    RevitLinkInstance revitLink = RevitLinkInstance.Create(thisDoc, result.ElementId);
                    //if (result.ElementId != ElementId.InvalidElementId)
                    //{
                    //    RevitLinkInstance revitLink = RevitLinkInstance.Create(thisDoc, result.ElementId);
                    //}

                    Location locationRevitLink = revitLink.Location;
                    locationRevitLink.Move(new XYZ(0, 0, 0));
                    thisDoc.Regenerate();
                }
                
                transaction.Commit();
            }
            
            return Result.Succeeded;
        }


        private void GetOpenIFC(string pathIFC)
        {
            
            IFC.IFCImportOptions ifc_option = new IFC.IFCImportOptions();
            ifc_option.Action = IFC.IFCImportAction.Open;
            ifc_option.AutoJoin = true;
            ifc_option.AutocorrectOffAxisLines = true;
            ifc_option.Intent = IFC.IFCImportIntent.Parametric;
            ifc_option.CreateLinkInstanceOnly = false;

            if (File.Exists(pathIFC))
            {
                docIFC = application.OpenIFCDocument(pathIFC, ifc_option);
            }
            
        }

        private void SaveAsIFC(string pathIFCrvt)
        {
            SaveAsOptions sop = new SaveAsOptions();
            sop.Compact = true;
            sop.MaximumBackups = 1;
            sop.OverwriteExistingFile = true;
            docIFC.SaveAs(pathIFCrvt, sop);
            string tittle = docIFC.Title;
        }
    }
}
