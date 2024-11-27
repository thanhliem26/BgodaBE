using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Service.DTO.Shared
{
    public class MetaData
    {
        public int Count { get; set; }
        public ICollection<object>? Rows { get; set; }
    }
}
