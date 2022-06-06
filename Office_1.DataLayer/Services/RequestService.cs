using Microsoft.EntityFrameworkCore;
using Office_1.DataLayer.Models;

namespace Office_1.DataLayer.Services
{
    public static class RequestService
    {
        private static IList<Status> GetStatuses(bool showCreated, bool showInReview, bool showReviewed,
            bool showDeclined)
        {
            List<Status> statuses = new();

            if (showCreated)
            {
                statuses.Add(Status.Created);
            }

            if (showInReview)
            {
                statuses.Add(Status.InReview);
            }

            if (showReviewed)
            {
                statuses.Add(Status.Reviewed);
            }

            if (showDeclined)
            {
                statuses.Add(Status.Declined);
            }

            return statuses;
        }

        public static IList<Request> GetSpecialRequests(bool showCreated, bool showInReview, bool showReviewed,
            bool showDeclined)
        {
            using var context = new ApplicationContext();

            var statuses = GetStatuses(showCreated, showInReview, showReviewed, showDeclined);
            var query = context.Requests.Where(r => statuses.Contains(r.Status));

            return query.Include(r => r.Client).ToList();
        }

        public static IList<Request> GetAllRequests()
        {
            using var context = new ApplicationContext();

            return context.Requests.Include(r => r.Client).ToList();
        }

        public static void UpdateRequest(Request request)
        {
            using var context = new ApplicationContext();

            context.Requests.Update(request);
            context.SaveChanges();
        }

        public static void UpdateRequest(Request request, Client client)
        {
            using var context = new ApplicationContext();

            request.Client = client;
            context.Clients.Attach(client);

            context.Requests.Update(request);
            context.SaveChanges();
        }

        public static void InsertRequest(Request request, Client client)
        {
            using var context = new ApplicationContext();

            request.Client = client;

            context.Clients.Attach(client);

            context.Requests.Add(request);
            context.SaveChanges();
        }

        public static bool Exists(Request request)
        {
            using var context = new ApplicationContext();

            return context.Requests.Any(r => r.Id == request.Id);
        }
        
    }
}