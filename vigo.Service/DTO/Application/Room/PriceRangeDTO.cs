using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Service.DTO.Application.Room
{
    public class PriceRangeDTO
    {
        public decimal Max { get; set; }
        public decimal Min { get; set; }
    }
}
