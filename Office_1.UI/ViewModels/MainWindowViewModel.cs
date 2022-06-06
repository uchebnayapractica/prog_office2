using Office_1.UI.Commands;
using System.Windows.Input;
using Office_2.DataLayer.Models;
using Office_2.DataLayer.Services;


namespace Office_1.UI.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private string _inputPath;
        private string _outputPath;

        public MainWindowViewModel()
        {
            AllRequests = new AllRequestsViewModel();
            ReviewRequest = new ReviewRequestViewModel();
            ChangeVisibleGridCommand = new ChangeVisibleGridCommand(this);
            ReviewRequestCommand = new ReviewRequestCommand(ReviewRequest, this);
            PrintNewRequestsToFilesCommand = new PrintNewRequestsToFilesCommand(this);
            GetNewRequestsFromFilesCommand = new GetNewRequestsFromFilesCommand(this);

            settings = SettingsService.GetSettings();
            _inputPath = settings.ImportPath;
            _outputPath = settings.ExportPath;

            VisibleVM = AllRequests;
        }

        public ICommand ChangeVisibleGridCommand { get; set; }
        public ICommand ReviewRequestCommand { get; set; }

        //print copy rules
        public ICommand PrintNewRequestsToFilesCommand { get; set; }
        public ICommand GetNewRequestsFromFilesCommand { get; set; }

        //Откуда брать файлы
        public string InputPath
        {
            get => _inputPath;
            set
            {
                if (value != _inputPath)
                {
                    _inputPath = value;
                    OnPropertyChanged(nameof(InputPath));
                    //ChangeInputPath in db
                    if (value != null)
                    {
                        settings.ImportPath = _inputPath;
                        SettingsService.SaveSettings(settings);
                    }
                }
            }
        }

        //Куда сохранять файлы
        public string OutputPath
        {
            get => _outputPath;
            set
            {
                if (value != _outputPath)
                {
                    _outputPath = value;
                    OnPropertyChanged(nameof(OutputPath));
                    //ChangeOutputPath in db
                    if (value != null)
                    {
                        settings.ExportPath = _outputPath;
                        SettingsService.SaveSettings(settings);
                    }
                }
            }
        }

        public Settings settings { get; set; }
        public TabViewModel VisibleVM { get; set; }
        public ReviewRequestViewModel ReviewRequest { get; set; }
        public AllRequestsViewModel AllRequests { get; set; }
    }
}
