using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace IncrementalLoading
{
    public class PagedSourceLoader<T,K> : IPagedSource<T,K>
        where T:class 
    {
        private Func<T, IPagedResponse<K>> getPagedResponse;
        public PagedSourceLoader(Func<T, IPagedResponse<K>> GetPagedResponse)
        {
            getPagedResponse = GetPagedResponse;
        }

        #region IPagedSource

        public async Task<IPagedResponse<K>> GetPage(string query, int pageIndex, int pageSize)
        {
            query += "&page="+pageIndex;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(query);
            var data = await response.Content.ReadAsStreamAsync();
            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(T));
            T dat = json.ReadObject(data) as T;
            return getPagedResponse(dat);
        }

        #endregion
    }
}
