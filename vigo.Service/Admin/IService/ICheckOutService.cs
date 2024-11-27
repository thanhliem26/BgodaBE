using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Service.Admin.IService
{
    public interface ICheckOutService
    {
        public Task CheckOutEveryday(DateTime time);
    }
}
