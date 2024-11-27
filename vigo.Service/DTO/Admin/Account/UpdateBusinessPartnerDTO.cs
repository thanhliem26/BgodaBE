using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Service.DTO.Admin.Account
{
    public class UpdateBusinessPartnerDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ProvinceId { get; set; } = string.Empty;
        public string DistrictId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int RoleId { get; set; }
    }
}
