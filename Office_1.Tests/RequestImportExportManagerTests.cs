using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Office_1.DataLayer;
using Office_1.DataLayer.Models;
using Office_1.DataLayer.Services;

namespace Office_1.Tests;

public class RequestImportExportManagerTests
{

    [SetUp]
    public void SetUp()
    {
        using (var db = new ApplicationContext())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.Database.Migrate();
        }

        SetSettings();
    }

    private void SetSettings()
    {
        var s = SettingsService.GetSettings();
        s.ExportPath = "/Users/noliktop/Desktop/requests/export";
        s.ImportPath = "/Users/noliktop/Desktop/requests/import";
        
        SettingsService.SaveSettings(s);
    }

    [Test]
    public void TestExport()
    {
        RequestTests.MakeSomeRequest(Status.Created, "Иван", "улица Пушкина");
        RequestTests.MakeSomeRequest(Status.Created, "Иван 2", "улица Колотушкина");
        RequestTests.MakeSomeRequest(Status.Created, "Иван 3", "улица Колотушкина 2");
        RequestTests.MakeSomeRequest(Status.Created, "Кто-то", "улица Колотушкина 3");
        RequestTests.MakeSomeRequest(Status.Created, "Владислав", "улица Пушкина 4");

        var files = RequestImportExportManager.ExportCreatedRequests();
        Console.WriteLine("Exported files: " + string.Join(", ", files));
        
        Assert.AreEqual(2, files.Count);

        var updRequests = RequestService.GetAllRequests();
        Assert.AreEqual(2, updRequests.Count);

        var first = updRequests[0];
        var second = updRequests[1];
        
        Assert.AreEqual(Status.InReview, first.Status);
        Assert.AreEqual(Status.InReview, second.Status);
    }

    [Test]
    public void TestImport()
    {
        var requests = RequestImportExportManager.ImportRequests();
        
        Assert.AreEqual(2, requests.Count, "Залейте 2 файлика из export папки");

        var first = requests[0];
        var second= requests[1];
        
        Assert.AreNotEqual(0, first.Id);
        Assert.AreNotEqual(0, second.Id);
        
        Assert.IsNotNull(first.Client);
        Assert.IsNotNull(second.Client);
        
        Assert.AreNotEqual(0, first.Client.Id);
        Assert.AreNotEqual(0, second.Client.Id);

        if (!first.Client.Name.Equals("Иван"))
        {
            (first, second) = (second, first);
        }
        
        Assert.AreEqual("Иван", first.Client.Name);
        Assert.AreEqual("Иван 2", second.Client.Name);
        
        Assert.AreEqual(Status.Created, first.Status);
        Assert.AreEqual(Status.Created, second.Status);
        
        Assert.AreEqual("резолюция", first.Resolution);
        Assert.AreEqual("резолюция", second.Resolution);
    }
    
}