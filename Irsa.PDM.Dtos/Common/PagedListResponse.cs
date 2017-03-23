using System.Collections.Generic;

namespace Irsa.PDM.Dtos.Common
{
    public class PagedListResponse<T>: Response<IList<T>>
    {
        public int Count { get; set; }
    }
}
