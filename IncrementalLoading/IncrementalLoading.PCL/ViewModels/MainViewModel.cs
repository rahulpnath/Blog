using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IncrementalLoading.PCL.ViewModels
{
    public class MainViewModel: ViewModelBase
    {
        private static string[] photoCollections =
            {
                "popular",
                "upcoming",
                "editors",
                "fresh_today"
            };

        public List<PhotoCollectionViewModel> PhotoCollectionViewModels { get; set; }

        public MainViewModel()
        {
            PhotoCollectionViewModels = new List<PhotoCollectionViewModel>();
            foreach (var photoCollection in photoCollections)
            {
                this.PhotoCollectionViewModels.Add(new PhotoCollectionViewModel(photoCollection));
            }
        }

        private bool isInitialized;

        public void OnPageNavigated()
        {
        if (!this.isInitialized)
        {
            // initialize all View Models
            foreach (var photoCollectionViewModel in this.PhotoCollectionViewModels)
            {
                photoCollectionViewModel.Initialize();
            }

            this.isInitialized = true;
        }
        }
    }
}
