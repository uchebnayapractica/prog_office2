using Office_1.DataLayer.Models;

namespace Office_1.DataLayer.Services;

public class SettingsService
{

    public static Settings GetSettings()
    {
        using var context = new ApplicationContext();

        if (context.Settings.Any())
        {
            return context.Settings.First();
        }

        var settings = new Settings
        {
            ExportPath = string.Empty,
            ImportPath = string.Empty
        };
        
        context.Settings.Add(settings);
        context.SaveChanges();

        return settings;
    }

    public static void SaveSettings(Settings settings)
    {
        using var context = new ApplicationContext();

        if (!Path.EndsInDirectorySeparator(settings.ExportPath))
        {
            settings.ExportPath += Path.DirectorySeparatorChar;
        }
        
        if (!Path.EndsInDirectorySeparator(settings.ImportPath))
        {
            settings.ImportPath += Path.DirectorySeparatorChar;
        }

        context.Settings.Update(settings);
        context.SaveChanges();
    }

}