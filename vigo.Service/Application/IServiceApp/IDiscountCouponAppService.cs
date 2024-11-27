using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.Helper;
using vigo.Service.DTO.Admin.Room;
using vigo.Service.DTO.Application.Discount;

namespace vigo.Service.Application.IServiceApp
{
    public interface IDiscountCouponAppService
    {
        Task<PagedResultCustom<DiscountCouponAppDTO>> GetPaging(int page, int perPage, string? searchName, ClaimsPrincipal user);
        Task<List<DiscountCouponAppDTO>> GetAllUseAble(int roomId, ClaimsPrincipal user);
        Task<List<RoomDTO>> GetAllUseAbleRoom(int couponId, ClaimsPrincipal user);
        Task<DiscountCouponAppDTO> GetDetail(int id, ClaimsPrincipal user);
    }
}
