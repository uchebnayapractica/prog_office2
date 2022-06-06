using Office_2.DataLayer.Models;
using Office_2.DataLayer.Services;

namespace Office_2.DataLayer;

public static class RequestImportExportManager
{

    public static IList<Request> ImportRequests(bool deleteFiles = true)
    {
        var s = SettingsService.GetSettings();

        var path = s.ImportPath;
        CheckPath(path, "import path");

        return ImportRequestsFrom(path, deleteFiles);
    }

    private static IList<Request> ImportRequestsFrom(string path, bool deleteFiles = true)
    {
        var requests = new List<Request>();
        
        foreach (var file in Directory.GetFiles(path))
        {
            var request = RequestScanner.LoadFromFile(file); 
            
            requests.Add(request);

            if (deleteFiles)
            {
                File.Delete(file);                
            }
        }
        
        return requests;
    }

    public static IList<string> ExportCreatedRequests()
    {
        var requests = RequestService.GetSpecialRequests(true, false, false, false);
        
        return ExportRequests(requests, Status.InReview);
    }

    public static IList<string> ExportReviewedAndDeclinedRequests()
    {
        var requests = RequestService.GetSpecialRequests(false, false, true, true);
        
        return ExportRequests(requests, Status.Completed);
    }
    
    private static IList<string> ExportRequests(IList<Request> requests, Status? newStatus = null)
    {
        var s = SettingsService.GetSettings();

        var path = s.ExportPath;
        CheckPath(path, "export path");
        
        return ExportRequestsTo(path, requests, newStatus);
    }
    
    private static IList<string> ExportRequestsTo(string path, IList<Request> requests, Status? newStatus = null)
    {
        var filesPaths = new List<string>();
        
        foreach (var request in requests)
        {
            var filePath = path + GenerateName(request);
            RequestPrinter.PrintIntoFile(filePath, request, newStatus);
            
            filesPaths.Add(filePath);
        }

        return filesPaths;
    }

    private static string GenerateName(Request request)
    {
        return "обращение_" + request.Client.Name.Replace(' ', '_') + "_N" + request.Id + ".jpg";
    }

    private static void CheckPath(string path, string namePath)
    {
        if (path == "")
        {
            throw new Exception($"Не задан {namePath}");
        }

        if (!Directory.Exists(path))
        {
            throw new Exception($"Указанный {namePath} не существует");
        }
    }
    
}