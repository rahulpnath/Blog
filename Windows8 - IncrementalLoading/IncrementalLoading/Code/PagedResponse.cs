using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncrementalLoading
{
    [DebuggerDisplay("PageIndex = {PageIndex} - PageSize = {PageSize} - VirtualCount = {VirtualCount}")]
    public class PagedResponse<K> : IPagedResponse<K>
    {
        public PagedResponse(IEnumerable<K> items, int virtualCount,int itemsPerPage)
        {
            this.Items = items;
            this.VirtualCount = virtualCount;
            rpp = itemsPerPage;
        }

        public int VirtualCount { get; private set; }
        public int rpp { get;  set; }
        public IEnumerable<K> Items { get; private set; }
    }

}