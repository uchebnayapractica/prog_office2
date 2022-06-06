using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Office_1.DataLayer;
using Office_1.DataLayer.Models;
using Office_1.DataLayer.Services;

namespace Office_1.Tests;

public class SettingsTests
{

    [SetUp]
    public void SetUp()
    {
        using var db = new ApplicationContext();

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        db.Database.Migrate();
    }

    [Test]
    public void CheckSettingsAmount()
    {
        using var context = new ApplicationContext();

        Assert.AreEqual(0, context.Settings.Count());

        var s = SettingsService.GetSettings();

        Assert.AreEqual(1, context.Settings.Count());

        var s2 = SettingsService.GetSettings();
     
        Assert.AreEqual(s.Id, s2.Id);
        Assert.AreEqual(1, context.Settings.Count()); // никакие новые настройки не докинулись
    }

    [Test]
    public void TestUpdateSettings()
    {
        using var context = new ApplicationContext();
        
        var s = SettingsService.GetSettings();

        s.ExportPath = "/home/noliktop/export";
        s.ImportPath = "/home/noliktop/import";
        
        SettingsService.SaveSettings(s);
        
        var s2 = SettingsService.GetSettings();
        
        Assert.AreEqual(s.ExportPath, s2.ExportPath);
        Assert.AreEqual(s.ImportPath, s2.ImportPath);
        
        Assert.AreEqual(1, context.Settings.Count());
    }
    
}