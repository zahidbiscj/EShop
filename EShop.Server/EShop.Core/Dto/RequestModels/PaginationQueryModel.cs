using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Core.Dto.RequestModels
{
    public class PaginationQueryModel
    {
        public int PageSize { get; set; } = Int32.MaxValue;
        public int PageNo { get; set; } = 1;
        public int Skip() => (PageNo - 1) * PageSize;
        public string SearchText { get; set; } = "";

        public static PaginationQueryModel All = new PaginationQueryModel(){PageSize = int.MaxValue, PageNo = 1};
    }
}
