using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.Helper;
using vigo.Service.DTO.Admin.Rating;

namespace vigo.Service.Admin.IService
{
    public interface IRatingService
    {
        Task<PagedResultCustom<RatingDTO>> GetPaging(int page, int perPage, RatingType type, ClaimsPrincipal user);
        Task Approve(List<int> ids, ClaimsPrincipal user);
        Task UnApprove(List<int> ids, ClaimsPrincipal user);
        Task Delete(List<int> ids, ClaimsPrincipal user);
    }
}
