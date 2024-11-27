using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.Helper;
using vigo.Service.DTO.Admin.Role;

namespace vigo.Service.Admin.IService
{
    public interface IRoleService
    {
        Task<PagedResultCustom<RoleDTO>> GetPaging(int page, int perPage, string? sortType, string? sortField, string? searchName, ClaimsPrincipal user);
        Task<List<RoleDTO>> GetAll(ClaimsPrincipal user);
        Task<RoleDetailDTO> GetDetail(int id, ClaimsPrincipal user);
        Task<List<RolePermissionDTO>> GetPermission(ClaimsPrincipal user);
        Task Create(RoleCreateDTO dto, ClaimsPrincipal user);
        Task Update(RoleUpdateDTO dto, ClaimsPrincipal user);
        Task Delete(int id, ClaimsPrincipal user);
    }
}
