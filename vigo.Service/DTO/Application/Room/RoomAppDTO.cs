using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Service.DTO.Application.Room
{
    public class RoomAppDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ProvinceId { get; set; } = string.Empty;
        public string Province {  get; set; } = string.Empty;
        public string DistrictId { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Thumbnail { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal Star { get; set; }
        public int Avaiable { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public decimal DefaultDiscount { get; set; }
        public string Address { get; set; } = string.Empty;
    }
}
