using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vigo.Service.DTO.Admin.Service;

namespace vigo.Service.DTO.Admin.Room
{
    public class RoomDetailDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Thumbnail { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Avaiable { get; set; }
        public int RoomTypeId { get; set; }
        public string RoomType { get; set; } = string.Empty;
        public int BusinessPartnerId { get; set; }
        public string BusinessPartnerName { get; set; } = string.Empty;
        public decimal Star { get; set; }
        public decimal DefaultDiscount { get; set; }
        public string ProvinceId { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string DistrictId { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public List<ServiceDTO> Services { get; set; } = new List<ServiceDTO>();
        public List<RoomImageDTO> Images { get; set; } = new List<RoomImageDTO>();
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime DeletedDate { get; set; }
    }
}
