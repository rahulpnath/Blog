using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IncrementalLoading.PCL.ViewModels
{
    using IncrementalLoading.PCL.Helpers;
using IncrementalLoading.PCL.Models;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    public class PhotoCollectionViewModel
    {
        // Get a consumer key from http://500px.com/settings/applications by registering for an application
        private static string ConsumerKey = "";

        private string datasourceUrl = "https://api.500px.com/v1/photos?feature={0}";

        // rpp indicates the number of items per page.(request per page)
        private string additionalParameters = "&rpp=10&page={0}&consumer_key=" + ConsumerKey;

        private IncrementalLoader<PhotoCollection> incrementalLoader;

        private PhotoCollection currentCollection;

        public string Title { get; set; }

        public ObservableCollection<Photo> Photos { get; set; }

        public PhotoCollectionViewModel(string collectionName)
        {
            this.Title = collectionName;
            this.Photos = new ObservableCollection<Photo>();
            this.datasourceUrl = string.Format(this.datasourceUrl, collectionName) + this.additionalParameters;
            this.incrementalLoader = new IncrementalLoader<PhotoCollection>(this.datasourceUrl);
        }

        public async void Initialize()
        {
            await this.LoadMorePhotos(null);
        }

        public async Task LoadMorePhotos(Photo currentPhoto)
        {
            if (currentPhoto != null)
            {
                var index = this.Photos.IndexOf(currentPhoto);
                if (this.Photos.Count - 3 > index)
                {
                    return ;
                }
            }
            this.currentCollection = await this.incrementalLoader.LoadNextPage();

            foreach (var photo in this.currentCollection.photos)
            {
                this.Photos.Add(photo);
            }
        }

    }
}
