using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using vigo.Service.DTO.Admin.Account;
using vigo.Service.DTO.Application.Search;
using vigo.Service.DTO.Application.UI;

namespace vigo.Service.Application.IServiceApp
{
    public interface IUIService
    {
        Task<List<VisitProvinceDTO>> GetVisitProvince();
        Task<List<RecommendPlaceDTO>> GetPopularVisit();
        Task<List<BusinessPartnerShortDTO>> GetAllBusinessPartnerShort();
    }
}
