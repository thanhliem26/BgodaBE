using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Domain.Helper
{
    public class PagedResultCustom<T>
    {
        public List<T> Items { get; set; }
        public int TotalRecords { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public PagedResultCustom(List<T> items, int totalRecords, int pageIndex, int pageSize)
        {
            Items = items;
            TotalRecords = totalRecords;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }
}
