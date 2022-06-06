using Office_1.UI.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Office_2.DataLayer;
using Office_2.DataLayer.Models;

namespace Office_1.UI.ViewModels
{
    public class ReviewRequestViewModel : TabViewModel
    {
        private string _directorName;

        private string _subject;

        private string _content;

        private string _resolution;

        private string _remark;

        private Status _status;

        private Request _reviewingRequest;

        private string _statusDefinition;


        public ReviewRequestViewModel()
        {
            GridVisibility = Visibility.Hidden;
            Statuses = new ObservableCollection<string>();
            Statuses.Add("Рассматривается");
            Statuses.Add("Рассмотрено");
            Statuses.Add("Отклонено");
        }

        public ObservableCollection<string> Statuses { get; set; }

        public Request ReviewingRequest
        {
            get => _reviewingRequest;
            set
            {
                if (value != _reviewingRequest)
                {
                    _reviewingRequest = value;
                    OnPropertyChanged(nameof(ReviewingRequest));
                    DirectorName = _reviewingRequest.DirectorName;
                    Subject = _reviewingRequest.Subject;
                    Content = _reviewingRequest.Content;
                    ClientName = _reviewingRequest.Client.Name;
                    ClientAddress = _reviewingRequest.Client.Address;
                    Client = _reviewingRequest.Client;

                    //Optional
                    Resolution = _reviewingRequest.Resolution;
                    Remark = _reviewingRequest.Remark;
                    Status = _reviewingRequest.Status;
                }
            }
        }

        public string DirectorName
        {
            get => _directorName;
            set
            {
                if (value != _directorName)
                {
                    _directorName = value;
                    OnPropertyChanged(nameof(DirectorName));
                }
            }
        }

        public string Subject
        {
            get => _subject;
            set
            {
                if (value != _subject)
                {
                    _subject = value;
                    OnPropertyChanged(nameof(Subject));
                }
            }
        }

        public string Content
        {
            get => _content;
            set
            {
                if (value != _content)
                {
                    _content = value;
                    OnPropertyChanged(nameof(Content));
                }
            }
        }

        public string Resolution
        {
            get => _resolution;
            set
            {
                if (value != _resolution)
                {
                    _resolution = value;
                    OnPropertyChanged(nameof(Resolution));
                }
            }
        }

        public string Remark
        {
            get => _remark;
            set
            {
                if (value != _remark)
                {
                    _remark = value;
                    OnPropertyChanged(nameof(Remark));
                }
            }
        }

        public Status Status
        {
            get => _status;
            set
            {
                if (value != _status)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                    StatusDefinition = EnumExtension.GetDescription(_status);
                }
            }
        }

        public string StatusDefinition
        {
            get => _statusDefinition;
            set
            {
                if (value != _statusDefinition)
                {
                    _statusDefinition = value;
                    OnPropertyChanged(nameof(StatusDefinition));
                    Status = EnumExtension.GetValueFromDescription<Status>(_statusDefinition);
                }
            }
        }

    }
}
