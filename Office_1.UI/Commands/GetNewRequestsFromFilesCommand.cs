using Office_1.UI.ViewModels;
using System.Windows;
using Office_2.DataLayer;

namespace Office_1.UI.Commands
{
    public class GetNewRequestsFromFilesCommand : CommandBase
    {
        private MainWindowViewModel _mainWindowViewModel;

        public GetNewRequestsFromFilesCommand(MainWindowViewModel vm)
        {
            _mainWindowViewModel = vm;
        }

        public override void Execute(object parameter)
        {
             RequestImportExportManager.ImportRequests();
            _mainWindowViewModel.AllRequests.GetRequestsCommand.Execute(null);
            MessageBox.Show("Успешно добавлены все поступившие заявки!");
        }
    }
}
