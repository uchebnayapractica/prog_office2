﻿using Office_1.DataLayer.Models;
using Office_1.UI.Commands;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;


namespace Office_1.UI.ViewModels
{
    public class AllRequestsViewModel : TabViewModel
    {
        private bool _showNew;
        private bool _showInReview;
        private bool _showReviewed;
        private bool _showDeclined;

        private bool _isReviewButtonAvailable;

        private Request _selectedItem;

        public AllRequestsViewModel()
        {
            GridVisibility = Visibility.Visible;
            Requests = new ObservableCollection<Request>();

            GetRequestsCommand = new GetRequestsCommand(this);

            ShowNew = false;
            ShowInReview = true;
            ShowDeclined = true;
            ShowReviewed = true;
        }

        public ObservableCollection<Request> Requests { get; set; }

        public bool ShowNew
        {
            get => _showNew;
            set
            {
                if (value != _showNew)
                {
                    _showNew = value;
                    OnPropertyChanged(nameof(ShowNew));
                    GetRequestsCommand.Execute(null);
                }
            }
        }

        public bool ShowInReview
        {
            get => _showInReview;
            set
            {
                if (value != _showInReview)
                {
                    _showInReview = value;
                    OnPropertyChanged(nameof(ShowInReview));
                    GetRequestsCommand.Execute(null);
                }
            }
        }

        public bool ShowReviewed
        {
            get => _showReviewed;
            set
            {
                if (value != _showReviewed)
                {
                    _showReviewed = value;
                    OnPropertyChanged(nameof(GridVisibility));
                    GetRequestsCommand.Execute(null);
                }
            }
        }

        public bool ShowDeclined
        {
            get => _showDeclined;
            set
            {
                if (value != _showDeclined)
                {
                    _showDeclined = value;
                    OnPropertyChanged(nameof(ShowDeclined));
                    GetRequestsCommand.Execute(null);
                }
            }
        }

        public bool IsReviewButtonAvailable
        {
            get => _isReviewButtonAvailable;
            set
            {
                if (value != _isReviewButtonAvailable)
                {
                    _isReviewButtonAvailable = value;
                    OnPropertyChanged(nameof(IsReviewButtonAvailable));
                }
            }
        }

        public Request SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (value != _selectedItem)
                {
                    _selectedItem = value;

                    OnPropertyChanged(nameof(SelectedItem));
                    if (value != null)
                    {
                        IsReviewButtonAvailable = true;
                    }
                    else
                    {
                        IsReviewButtonAvailable = false;
                    }
                }
            }
        }

        public ICommand GetRequestsCommand { get; set; }

    }
}