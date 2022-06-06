using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Office_1.DataLayer;
using Office_1.DataLayer.Models;
using Office_1.DataLayer.Services;

namespace Office_1.Tests;

public class RequestTests
{
    [SetUp]
    public void Setup()
    {
        using var db = new ApplicationContext();

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        db.Database.Migrate();
    }

    [Test]
    public void TestMakeRequest()
    {
        var request = MakeSomeRequest(Status.Created, "Иван", "улица");
        
        Assert.AreNotEqual(0, request.Id); // обращение загрузилось в бд 
        
        Assert.AreEqual("какая-то информация", request.Content);
        Assert.AreEqual("Николаев Николай Николаевич", request.DirectorName);
        Assert.AreEqual("что-то", request.Remark);
        Assert.AreEqual("резолюция", request.Resolution);
        Assert.AreEqual(Status.Created, request.Status);
        Assert.AreEqual("тема обращения", request.Subject);
        
        var client = request.Client;
        Assert.IsNotNull(client);
        Assert.AreNotEqual(0, client.Id);
        Assert.AreEqual("Иван", client.Name);
    }

    [Test]
    public void TestGetAllRequests()
    {
        var request = MakeSomeRequest(Status.Created, "Иван", "улица");

        var allRequests = RequestService.GetAllRequests();
        Assert.IsNotEmpty(allRequests);

        var gotRequest = allRequests.First();
        Assert.AreEqual(request.Id, gotRequest.Id);

        var client = gotRequest.Client;
        Assert.IsNotNull(client);
        Assert.AreNotEqual(0, client.Id);
        Assert.AreEqual("Иван", client.Name);
    }

    [Test]
    public void TestUpdateRequest()
    {
        var request = MakeSomeRequest(Status.Created);

        request.Status = Status.InReview;
        RequestService.UpdateRequest(request);

        var gotRequest = RequestService.GetAllRequests().First();
        Assert.AreEqual(Status.InReview, gotRequest.Status);

        gotRequest.Status = Status.Reviewed;
        RequestService.UpdateRequest(gotRequest);
        
        gotRequest = RequestService.GetAllRequests().First();
        Assert.AreEqual(Status.Reviewed, gotRequest.Status);
    }

    [Test]
    public void TestGetSpecialRequests()
    {
        var requestStatusCreated = MakeSomeRequest(Status.Created, "Иван", "улица Пушкина");
        var requestStatusInReview = MakeSomeRequest(Status.InReview, "Иван 2", "улица Пушкина");
        var requestStatusReviewed = MakeSomeRequest(Status.Reviewed, "Иван 3", "улица Пушкина");
        var requestStatusDeclined = MakeSomeRequest(Status.Declined, "Иван 4", "улица Пушкина");

        #region test created
        var requests = RequestService.GetSpecialRequests(true, false, false, false);
        Assert.IsNotEmpty(requests);
        
        var request = requests.First();
        Assert.AreEqual(requestStatusCreated.Id, request.Id);
        Assert.AreEqual(Status.Created, request.Status);
        #endregion

        #region test inreview
        requests = RequestService.GetSpecialRequests(false, true, false, false);
        Assert.IsNotEmpty(requests);
        
        request = requests.First();
        Assert.AreEqual(requestStatusInReview.Id, request.Id);
        Assert.AreEqual(Status.InReview, request.Status);
        #endregion
        
        #region test reviewed
        requests = RequestService.GetSpecialRequests(false, false, true, false);
        Assert.IsNotEmpty(requests);
        
        request = requests.First();
        Assert.AreEqual(requestStatusReviewed.Id, request.Id);
        Assert.AreEqual(Status.Reviewed, request.Status);
        #endregion

        #region test declined
        requests = RequestService.GetSpecialRequests(false, false, false, true);
        Assert.IsNotEmpty(requests);
        
        request = requests.First();
        Assert.AreEqual(requestStatusDeclined.Id, request.Id);
        Assert.AreEqual(Status.Declined, request.Status);
        #endregion
    }

    [Test]
    public void TestGetSpecialRequestsMultipleChoice()
    {
        // проверяем что при выборе нескольких вариантов даст нужные
        
        var requestStatusCreated = MakeSomeRequest(Status.Created, "Иван", "улица Пушкина");
        var requestStatusInReview = MakeSomeRequest(Status.InReview, "Иван 2", "улица Пушкина"); // вариант который не будет выбран
        var requestStatusReviewed = MakeSomeRequest(Status.Reviewed, "Иван 3", "улица Пушкина");

        #region выбор по 2 статусам
        var requests = RequestService.GetSpecialRequests(true, false, true, true);
        
        // по идее должно выдать created и reviewed только...
        Assert.AreEqual(2, requests.Count);

        var gotRequestCreated = requests[0];
        var gotRequestReviewed = requests[1];
        if (gotRequestCreated.Status == Status.Reviewed) // на случай если они пришли в обратном порядке
        {
            (gotRequestCreated, gotRequestReviewed) = (gotRequestReviewed, gotRequestCreated);
        }
        
        Assert.AreEqual(Status.Created, gotRequestCreated.Status);
        Assert.AreEqual(Status.Reviewed, gotRequestReviewed.Status);
        
        Assert.AreEqual(requestStatusCreated.Id, gotRequestCreated.Id);
        Assert.AreEqual(requestStatusReviewed.Id, gotRequestReviewed.Id);
        #endregion
        
        #region выбор 0 вариантов
        requests = RequestService.GetSpecialRequests(false, false, false, false);
        Assert.IsEmpty(requests);
        #endregion
    }

    public static Request MakeSomeRequest(Status status)
    {
        var client = ClientTests.MakeSomeClient();

        return MakeSomeRequest(status, client);
    }

    public static Request MakeSomeRequest(Status status, string clientName, string clientAddress)
    {
        var client = ClientTests.MakeSomeClient(clientName, clientAddress);

        return MakeSomeRequest(status, client);
    }

    public static Request MakeSomeRequest(Status status, Client client)
    {
        var request = new Request
        {
            Content = "какая-то информация",
            DirectorName = "Николаев Николай Николаевич",
            Remark = "что-то",
            Resolution = "резолюция",
            Status = status,
            Subject = "тема обращения"
        };
        
        RequestService.InsertRequest(request, client);

        return request;
    }
}