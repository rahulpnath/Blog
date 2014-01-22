using IncrementalLoading.PCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IncrementalLoading.PCL.Helpers
{
    using System.IO;
    using System.Runtime.Serialization.Json;

    public class IncrementalLoader<T> where T : class
    {
        private string BaseUrl;

        private int CurrentPageNumber;

        private bool isCurrentlyLoading;

        private string CurrentUrl
        {
            get
            {
                return string.Format(this.BaseUrl, ++this.CurrentPageNumber);
            }
        }

        /// <summary>
        /// Initialize a new instance of Incremental loader.
        /// </summary>
        /// <param name="baseUrl">Base url</param>
        public IncrementalLoader(string baseUrl)
        {
            this.BaseUrl = baseUrl;
        }

        public async Task<T> LoadNextPage()
        {
            if (this.isCurrentlyLoading)
            {
                // call in progress
                return null;
            }

            this.isCurrentlyLoading = true;
            HttpClient client = new HttpClient();

            // Add Microsoft.Bcl.Async nuget for await to work on PCL.

            var response = await client.GetStringAsync(this.CurrentUrl);
            var serializer = new DataContractJsonSerializer(typeof(T));
            var returnObject = serializer.ReadObject(new MemoryStream(Encoding.Unicode.GetBytes(response))) as T;
            this.isCurrentlyLoading = false;

            return returnObject;
        }

    }
}
