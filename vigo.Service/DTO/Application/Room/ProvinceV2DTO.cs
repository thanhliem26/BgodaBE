using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Service.DTO.Application.Room
{
    public class ProvinceV2DTO
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public int RoomNumber { get; set; }
        public List<RoomAppDTO> Rooms { get; set; } = new List<RoomAppDTO>();
    }
}
