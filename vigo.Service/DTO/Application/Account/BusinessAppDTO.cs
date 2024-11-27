using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vigo.Service.DTO.Application.Room;

namespace vigo.Service.DTO.Application.Account
{
    public class BusinessAppDTO
    {
        public string CompanyName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public List<RoomAppDTO> RoomAppDTOs { get; set; } = new List<RoomAppDTO>();
    }
}
