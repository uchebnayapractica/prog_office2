using Office_1.DataLayer.Models;
using Office_1.DataLayer.Services;
using Office_1.UI.ViewModels;
using System.Collections.Generic;

namespace Office_1.UI.Commands
{
    public class GetRequestsCommand : CommandBase
    {
        private readonly AllRequestsViewModel _requestsViewModel;
        public GetRequestsCommand(AllRequestsViewModel ovm)
        {
            _requestsViewModel = ovm;
        }

        public override void Execute(object viewModel)
        {
            _requestsViewModel.Requests.Clear();

            var requests = new List<Request>();

            requests.AddRange(RequestService.GetSpecialRequests(_requestsViewModel.ShowNew, _requestsViewModel.ShowInReview, _requestsViewModel.ShowReviewed, _requestsViewModel.ShowDeclined));

            foreach (Request request in requests)
            {
                _requestsViewModel.Requests.Add(request);
            }
        }
    }
}
