using IncrementalLoading.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace IncrementalLoading
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //get your consumerkey from 500px site to test the applcation
            // http://500px.com/settings/applications?from=developers
            var consumerKey = "<YOUR CONSUMER KEY HERE>";
            var datasourceUrl = "https://api.500px.com/v1/photos?feature=popular&consumer_key=" + consumerKey + "&rpp=20";
            incrementalData.ItemsSource = new IncrementalSource<RootObject, Photo>(datasourceUrl, RootObjectResponse);
        }

        private PagedResponse<Photo> RootObjectResponse(RootObject rootObject)
        {
            return new PagedResponse<Photo>(rootObject.photos, rootObject.total_items, rootObject.photos != null ? rootObject.photos.Count : 0);
        }
    }
}
