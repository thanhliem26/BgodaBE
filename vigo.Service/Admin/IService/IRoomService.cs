using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.Helper;
using vigo.Service.DTO.Admin.Room;

namespace vigo.Service.Admin.IService
{
    public interface IRoomService
    {
        Task<PagedResultCustom<RoomDTO>> GetPaging(int page, int perPage, int? roomTypeId, int? businessPartnerId, string? sortType, string? sortField, string? searchName, ClaimsPrincipal user);
        Task<RoomDetailDTO> GetDetail(int id, ClaimsPrincipal user);
        Task Create(CreateRoomDTO dto, ClaimsPrincipal user);
        Task Update(UpdateRoomDTO dto, ClaimsPrincipal user);
        Task Delete(int id, ClaimsPrincipal user);
    }
}
