using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Service.DTO.Application.Search
{
    public class SearchResultDTO
    {
        public List<ProvinceShortDTO> ProvinceShortDTOs { get; set; } = new List<ProvinceShortDTO>();
        public List<BPShortDTO> BPShortDTOs { get; set; } = new List<BPShortDTO>();
    }
}
