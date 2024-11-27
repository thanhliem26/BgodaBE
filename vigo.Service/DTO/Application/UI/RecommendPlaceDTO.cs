using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vigo.Service.DTO.Application.Room;

namespace vigo.Service.DTO.Application.UI
{
    public class RecommendPlaceDTO
    {
        public string Name { get; set; } = string.Empty;
        public List<RoomAppDTO> Rooms { get; set; } = new List<RoomAppDTO>();
    }
}
