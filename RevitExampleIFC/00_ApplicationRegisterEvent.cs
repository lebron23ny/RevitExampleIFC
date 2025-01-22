using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitExampleIFC
{
    public class ApplicationRegisterEvent : IExternalApplication
    {
        ControlledApplication controlledApplication { get; set; }
        Application Application { get; set; }
        UIControlledApplication UIControlledApplication { get; set; }
        
        public Result OnShutdown(UIControlledApplication application)
        {
            throw new NotImplementedException();
        }

        public Result OnStartup(UIControlledApplication application)
        {
            UIControlledApplication = application;
            controlledApplication = application.ControlledApplication;
            //controlledApplication.LinkedResourceOpening += ControlledApplication_LinkedResourceOpening;
            //controlledApplication.LinkedResourceOpened += ControlledApplication_LinkedResourceOpened;
            //controlledApplication.DocumentSaved += ControlledApplication_DocumentSaved;
            //controlledApplication.FileImported += ControlledApplication_FileImported;
            //controlledApplication.FileExporting += ControlledApplication_FileExporting;
            controlledApplication.DocumentOpening += ControlledApplication_DocumentOpening;
            return Result.Succeeded;
        }

        private void ControlledApplication_DocumentOpening(object sender, Autodesk.Revit.DB.Events.DocumentOpeningEventArgs e)
        {
            // Получаем документ, который был открыт
            

            // Проверяем, если файл имеет расширение .ifc, это может быть результатом импорта
            string fileName = e.PathName.ToLower();
            if (fileName.EndsWith(".ifc"))
            {
                TaskDialog.Show("IFC Import", "IFC файл был импортирован: ");
            };
        }

        private void ControlledApplication_FileExporting(object sender, Autodesk.Revit.DB.Events.FileExportingEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ControlledApplication_FileImported(object sender, Autodesk.Revit.DB.Events.FileImportedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ControlledApplication_DocumentSaved(object sender, Autodesk.Revit.DB.Events.DocumentSavedEventArgs e)
        {
            
            var d = sender.GetType().Assembly;
            TaskDialog.Show("Сообщение", $"Произошло событие {e.Document.PathName}!");
        }

        private void ControlledApplication_LinkedResourceOpened(object sender, Autodesk.Revit.DB.Events.LinkedResourceOpenedEventArgs e)
        {
            var d = sender.GetType().Assembly;
            TaskDialog.Show("Сообщение", $"Произошло событие {e.LinkedResourcePathName}!"); ;
        }

        private void ControlledApplication_LinkedResourceOpening(object sender, Autodesk.Revit.DB.Events.LinkedResourceOpeningEventArgs e)
        {
            var d = sender.GetType().Name;
            TaskDialog.Show("Сообщение", $"Произошло событие {e.LinkedResourcePathName}!"); ;
        }
    }
}
