using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Office_1.DataLayer;

namespace Office_1.Tests;

public class RequestScannerTests
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
    public void TestScanning()
    {
        const string myPath = "/Users/noliktop/Desktop/office_qr/";

        var request = RequestScanner.LoadFromFile(myPath + "qr_1.jpeg");
        
        Assert.AreNotEqual(0, request.Id);
        
        Assert.IsNotNull(request.Client);
        Assert.AreEqual("Иван", request.Client.Name);
        
        Assert.AreEqual("какая-то информация", request.Content);
    }

    [Test]
    public void TestScanningBigVariant()
    {
        const string myPath = "/Users/noliktop/Desktop/office_qr/";

        var request = RequestScanner.LoadFromFile(myPath + "qr_2.jpeg");
        
        Assert.AreNotEqual(0, request.Id);
        
        Assert.IsNotNull(request.Client);
        Assert.IsTrue(request.Client.Name.StartsWith("Иванааааа"));
        
        Assert.AreEqual("какая-то информация", request.Content);
    }
    
}