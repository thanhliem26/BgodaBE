using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vigo.Service.DTO.Admin.Account;
using vigo.Service.DTO.Application.Account;

namespace vigo.Service.DTO.Application.Search
{
    public class SearchResultReturnDTO
    {
        public List<BusinessAppDTO> BusinessPartnerDTOs { get; set; } = new List<BusinessAppDTO>();
    }
}
