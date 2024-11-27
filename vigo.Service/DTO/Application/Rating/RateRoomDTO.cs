using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Service.DTO.Application.Rating
{
    public class RateRoomDTO
    {
        public int RoomId { get; set; }
        public int Rate { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
