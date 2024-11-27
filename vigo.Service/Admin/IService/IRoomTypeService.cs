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
    public interface IRoomTypeService
    {
        Task<PagedResultCustom<RoomTypeDTO>> GetPaging(int page, int perPage, string? sortType, string? sortField, string? searchName, ClaimsPrincipal user);
        Task<List<RoomTypeDTO>> GetAll(ClaimsPrincipal user);
        Task<RoomTypeDTO> GetDetail(int id, ClaimsPrincipal user);
        Task Create(RoomTypeCreateDTO dto, ClaimsPrincipal user);
        Task Update(RoomTypeUpdateDTO dto, ClaimsPrincipal user);
        Task Delete(int id, ClaimsPrincipal user);
    }
}
