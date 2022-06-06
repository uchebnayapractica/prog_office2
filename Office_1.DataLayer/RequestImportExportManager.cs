using Office_1.DataLayer.Models;
using Office_1.DataLayer.Services;

namespace Office_1.DataLayer;

public static class RequestImportExportManager
{

    public static IList<Request> ImportRequests(bool deleteFiles = true)
    {
        var s = SettingsService.GetSettings();

        var path = s.ImportPath;
        if (path == "")
        {
            throw new Exception("Не задан import path");
        }

        if (!Directory.Exists(path))
        {
            throw new Exception("Указанный import path не существует");
        }

        return ImportRequestsFrom(path);
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
        var requests = RequestService.GetSpecialRequests(false, false, true, true);
        
        return ExportRequests(requests);
    }
    
    private static IList<string> ExportRequests(IList<Request> requests)
    {
        var s = SettingsService.GetSettings();

        var path = s.ExportPath;
        if (path == "")
        {
            throw new Exception("Не задан export path");
        }

        if (!Directory.Exists(path))
        {
            throw new Exception("Указанный export path не существует");
        }
        
        return ExportRequestsTo(path, requests);
    }
    
    private static IList<string> ExportRequestsTo(string path, IList<Request> requests)
    {
        var filesPaths = new List<string>();
        
        foreach (var request in requests)
        {
            var filePath = path + GenerateName(request);
            RequestPrinter.PrintIntoFile(filePath, request, true);
            
            filesPaths.Add(filePath);
        }

        return filesPaths;
    }

    private static string GenerateName(Request request)
    {
        return "обращение_" + request.Client.Name.Replace(' ', '_') + "_N" + request.Id + ".jpg";
    }
    
}