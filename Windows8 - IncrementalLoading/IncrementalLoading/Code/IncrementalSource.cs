using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace IncrementalLoading
{
    public class IncrementalSource<T, K> : ObservableCollection<K>, ISupportIncrementalLoading
        where T: class
    {
        private string Query { get; set; }
        private int VirtualCount { get; set; }
        private int CurrentPage { get; set; }
        private IPagedSource<T,K> Source { get; set; }
        private int rpp { get; set; }

        public IncrementalSource(string query, Func<T, IPagedResponse<K>> GetPagedResponse)
        {
            this.Source = new PagedSourceLoader<T, K>(GetPagedResponse);
            this.VirtualCount = int.MaxValue;
            this.CurrentPage = 0;
            this.Query = query;
        }

        #region ISupportIncrementalLoading

        public bool HasMoreItems
        {
            get { return this.VirtualCount > this.CurrentPage * (rpp == 0 ? 10 :rpp); }
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            CoreDispatcher dispatcher = Window.Current != null ? Window.Current.Dispatcher : Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
            if (count > 50 || count <= 0)
            {
                // default load count to be set to 50
                count = 50;
            }


            return Task.Run<LoadMoreItemsResult>(
                async () =>
                {

                    IPagedResponse<K> result = await this.Source.GetPage(string.Format(this.Query,count), ++this.CurrentPage, (int)count);

                    this.VirtualCount = result.VirtualCount;
                    if (rpp == 0)
                    {
                        rpp = result.rpp;
                    }

                    await dispatcher.RunAsync(
                        CoreDispatcherPriority.Normal,
                        () =>
                        {
                            foreach (K item in result.Items)
                                this.Add(item);
                        });

                    return new LoadMoreItemsResult() { Count = (uint)result.Items.Count() };

                }).AsAsyncOperation<LoadMoreItemsResult>();
        }

        #endregion
    }
}
