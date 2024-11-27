using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Service.DTO.Application.Room
{
    public class GetRoomSearchDTO
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
        public string? SearchInput { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int? RoomTypeId { get; set; }
        public List<int>? Stars { get; set; }
        public List<int>? Services { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }
    }
}
