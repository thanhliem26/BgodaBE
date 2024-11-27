using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.Helper;
using vigo.Service.DTO.Admin.Account;
using vigo.Service.DTO.Admin.Discount;

namespace vigo.Service.Admin.IService
{
    public interface IDiscountCouponService
    {
        Task<PagedResultCustom<DiscountCouponDTO>> GetPaging(int page, int perPage, string? sortType, string? sortField, ClaimsPrincipal user);
        Task<List<DiscountCouponDTO>> GetAll(ClaimsPrincipal user);
        Task<List<RoomShortDTO>> GetRoomApplyDiscount(ClaimsPrincipal user);
        Task Create(CreateDiscountCouponDTO dto, ClaimsPrincipal user);
        Task<DiscountCouponDetailDTO> GetDetail(int id, ClaimsPrincipal user);
        Task Update(DiscountCouponUpdateDTO dto, ClaimsPrincipal user);
        Task Delete(int id, ClaimsPrincipal user);
    }
}
