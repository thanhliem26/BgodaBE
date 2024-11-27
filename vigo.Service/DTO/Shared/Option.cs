using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Service.DTO.Shared
{
    public class Option
    {
        public string Name { get; set; } = string.Empty;
        public int TotalRecords { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }
}
