using Office_1.UI.ViewModels;
using System.Windows;
using Office_2.DataLayer.Models;
using Office_2.DataLayer.Services;

namespace Office_1.UI.Commands
{
    public class ReviewRequestCommand : CommandBase
    {
        private readonly ReviewRequestViewModel _requestsViewModel;
        private readonly MainWindowViewModel _mainWindowViewModel;

        public ReviewRequestCommand(ReviewRequestViewModel rvm, MainWindowViewModel mvm)
        {
            _requestsViewModel = rvm;
            _mainWindowViewModel = mvm;
        }

        public override void Execute(object viewModel)
        {
            TabViewModel vm = (TabViewModel)viewModel;

            if (_requestsViewModel.Resolution != string.Empty &&
                _requestsViewModel.Resolution != null &&
                _requestsViewModel.Subject != string.Empty &&
                _requestsViewModel.Subject != null)
            {   
                //remark could be ""
                if (_requestsViewModel.Remark == null)
                {
                    _requestsViewModel.Remark = string.Empty;
                }

                Request request = _requestsViewModel.ReviewingRequest;
                request.Resolution = _requestsViewModel.Resolution;
                request.Remark = _requestsViewModel.Remark;
                request.Status = _requestsViewModel.Status;

                RequestService.UpdateRequest(request);

                //Обновляем список
                _mainWindowViewModel.AllRequests.GetRequestsCommand.Execute(null);
                MessageBox.Show("Успешно обновлено!");
                _mainWindowViewModel.ChangeVisibleGridCommand.Execute(vm);
            }
            else
            {
                MessageBox.Show("Заполните резолюцию!");
            }
        }
    }
}
