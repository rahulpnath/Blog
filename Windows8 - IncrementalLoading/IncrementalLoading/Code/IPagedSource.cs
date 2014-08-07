using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncrementalLoading
{

    public interface IPagedSource<R,K>
    {
        Task<IPagedResponse<K>> GetPage(string query, int pageIndex, int pageSize);
    }

}
