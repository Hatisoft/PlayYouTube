﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Soft.Hati.YouPlayVS.Core.MVVM;
using Soft.Hati.YouPlayVS.Core.Youtube;

namespace Soft.Hati.PlayYouTube.App.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _idStringVideo;
        private string _idVideo;
        private IEnumerable<SearchResult> videos;
        private SearchResult selectedVideo;
        private bool searchInProgress;

        public MainWindowViewModel()
        {
            GoCommand = new RelayCommand(Search, arg => true);
        }

        private void Search(object obj)
        {
            try
            {
                SearchInProgress = true;
                var req = new VideoRequester(new YouMixServiceContainer());
                req.Search(IDStringVideo).ContinueWith(result =>
                {
                    if (result.Exception == null)
                        Videos = result.Result.Videos;
                    else
                        HandleSearchException();
                    SearchInProgress = false;
                });
            }
            catch (Exception)
            {
                HandleSearchException();
                SearchInProgress = false;
            }
            
        }

        private void HandleSearchException()
        {
            MessageBox.Show(
                "Error accessing web service, please check your conection. If the problems perseveres please contact us",
                "Network Error", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public IEnumerable<SearchResult> Videos
        {
            get { return videos; }
            set { SetValue(ref videos, value, () => Videos); }
        }

        public bool SearchInProgress
        {
            get { return searchInProgress; }
            set { SetValue(ref searchInProgress, value, () => SearchInProgress); }
        }

        public SearchResult SelectedVideo
        {
            get { return selectedVideo; }
            set
            {
                SetValue(ref selectedVideo, value, ()=> SelectedVideo);
                if(value != null)
                    IDVideo = SelectedVideo.Id;
            }
        }

        public ICommand GoCommand { get; private set; }

        public string IDStringVideo
        {
            get { return _idStringVideo; }
            set { SetValue(ref _idStringVideo, value, () => IDStringVideo); }
        }

        public string IDVideo
        {
            get { return _idVideo; }
            set { SetValue(ref _idVideo, value, () => IDVideo); }
        }
    }
}