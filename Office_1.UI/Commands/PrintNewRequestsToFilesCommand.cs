using Office_1.UI.ViewModels;
using System.Windows;
using Office_2.DataLayer;

namespace Office_1.UI.Commands
{
    public class PrintNewRequestsToFilesCommand : CommandBase
    {
        private MainWindowViewModel _mainWindowViewModel;

        public PrintNewRequestsToFilesCommand(MainWindowViewModel vm)
        {
            _mainWindowViewModel = vm;
        }

        public override void Execute(object parameter)
        {
            RequestImportExportManager.ExportReviewedAndDeclinedRequests();

            MessageBox.Show("Успешно напечатаны рассмотренные и отклонённые заявки!");
        }
    }
}
