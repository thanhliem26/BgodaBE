using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.Entity;

namespace vigo.Service.DTO.Shared
{
    public class DataTemp<T> where T : class
    {
        public List<T> Data { get; set; } = new List<T>();
    }
}
