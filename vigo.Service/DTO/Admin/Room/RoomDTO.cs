using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vigo.Service.DTO.Admin.Account;
using vigo.Service.DTO.Admin.Service;

namespace vigo.Service.DTO.Admin.Room
{
    public class RoomDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Thumbnail { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Avaiable { get; set; }
        public int BusinessPartnerId { get; set; }
        public decimal Star { get; set; }
        public decimal DefaultDiscount { get; set; }
        public string Address { get; set; } = string.Empty;
        public BusinessPartnerShortDTO BusinessPartner { get; set; } = new BusinessPartnerShortDTO();
        public List<ServiceDTO> Services { get; set; } = new List<ServiceDTO>();
    }
}
