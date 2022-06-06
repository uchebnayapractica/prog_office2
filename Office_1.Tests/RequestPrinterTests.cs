using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Office_1.DataLayer;
using Office_1.DataLayer.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using ZXing;
using ZXing.Common;

namespace Office_1.Tests;

public class RequestPrinterTests
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
    public void TestCreation()
    {
        const string myPath = "/Users/noliktop/Desktop/office_qr/";
        
        var request = RequestTests.MakeSomeRequest(Status.Created, "Иван", "улица Пушкина");
        RequestPrinter.PrintIntoFile(myPath + "qr_1.jpeg", request);
        
        // вариант с длинным контентом
        var clientName = string.Concat(Enumerable.Repeat("а", 500));
        request = RequestTests.MakeSomeRequest(Status.Created, "Иван" + clientName, "улица Пушкина");
        RequestPrinter.PrintIntoFile(myPath + "qr_2.jpeg", request);   
    }
    
}