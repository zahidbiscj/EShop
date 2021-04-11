using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Core.Dto.ResponseModels
{
    public class PagedResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<T> Data { get; set; }
        public int Total { get; set; }

        public PagedResponse()
        {
            Data = new List<T>();
        }

        public PagedResponse(IEnumerable<T> data, int total, int pageNumber, int pageSize)
        {
            Data = data;
            Total = total;
            PageSize = pageSize;
            PageNumber = pageNumber;
        }
    }
}
