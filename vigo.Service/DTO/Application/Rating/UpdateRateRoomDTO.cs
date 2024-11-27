using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Service.DTO.Application.Rating
{
    public class UpdateRateRoomDTO
    {
        public int Id { get; set; }
        public int Rate { get; set; }
        public string UpdateComment { get; set; } = string.Empty;
    }
}
