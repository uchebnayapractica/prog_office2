using Office_2.DataLayer.Models;

namespace Office_2.DataLayer.Services;

public class SettingsService
{

    public static Settings GetSettings()
    {
        using var context = new ApplicationContext();

        if (context.Settings.Any()) // если настройки уже есть, то просто их возвращаем
        {
            return context.Settings.First();
        }

        // в ином случае создаем их
        
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

        AddExtraSlashes(settings);

        context.Settings.Update(settings);
        context.SaveChanges();
    }

    private static void AddExtraSlashes(Settings settings)
    {
        // это нужно, так как потом просто конкатенируется в полный путь файла без добавления лишних слешей
        
        // если путь не заканчивается на / или \ (в зависимости от операционной системы)
        if (!Path.EndsInDirectorySeparator(settings.ExportPath)) 
        {
            settings.ExportPath += Path.DirectorySeparatorChar; // то мы докидываем этот слеш
        }
        
        if (!Path.EndsInDirectorySeparator(settings.ImportPath)) // аналогично
        {
            settings.ImportPath += Path.DirectorySeparatorChar;
        }
    }

}