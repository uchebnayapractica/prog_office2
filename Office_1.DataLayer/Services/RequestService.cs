using Microsoft.EntityFrameworkCore;
using Office_2.DataLayer.Models;

namespace Office_2.DataLayer.Services
{
    public static class RequestService
    {
        public static IList<Request> GetSpecialRequests(bool showCreated, bool showInReview, bool showReviewed,
            bool showDeclined, bool showCompleted = false)
        {
            using var context = new ApplicationContext();
            
            var statuses = GetStatuses(showCreated, showInReview, showReviewed, showDeclined, showCompleted);
            var query = context.Requests.Where(r => 
                statuses.Contains(r.Status) // обращение имеет искомый статус 
                );

            return query.Include(r => r.Client).ToList();
        }

        private static IList<Status> GetStatuses(bool showCreated, bool showInReview, bool showReviewed,
            bool showDeclined, bool showCompleted = false)
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

            if (showCompleted)
            {
                statuses.Add(Status.Completed);
            }

            return statuses;
        }

        public static IList<Request> GetAllRequests()
        {
            using var context = new ApplicationContext();

            return context.Requests
                .Include(r => r.Client) // чтобы клиентов тоже вернуло
                .ToList();
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
            context.Clients.Attach(client); // чтобы клиента тоже загрузило, а иначе ошибка лезет

            context.Requests.Update(request);
            context.SaveChanges();
        }

        public static void InsertRequest(Request request, Client client)
        {
            using var context = new ApplicationContext();

            request.Client = client;
            
            context.Clients.Attach(client); // чтобы клиента тоже загрузило, а иначе ошибка лезет
            
            context.Requests.Add(request);
            context.SaveChanges();
        }

        public static bool Exists(Request request)
        {
            using var context = new ApplicationContext();

            return context.Requests.Any(r => 
                r.Id == request.Id // проверяем что это искомый id 
                );
        }
        
    }
}