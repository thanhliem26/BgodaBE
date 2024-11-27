using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.Helper;
using vigo.Service.DTO.Admin.Account;
using vigo.Service.DTO.Admin.Booking;

namespace vigo.Service.Admin.IService
{
    public interface IBookingService
    {
        Task<PagedResultCustom<BookingDTO>> GetPaging(int page, int perPage, bool? isReceived, string? sortType, string? sortField, ClaimsPrincipal user);
        Task<BookingDetailDTO> GetDetail(int id, ClaimsPrincipal user);
        Task ReceiveBooking(List<int> ids, ClaimsPrincipal user);
        Task Delete(List<int> ids, ClaimsPrincipal user);
    }
}
