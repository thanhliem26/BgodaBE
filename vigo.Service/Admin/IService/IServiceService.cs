using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.Helper;
using vigo.Service.DTO.Admin.Service;

namespace vigo.Service.Admin.IService
{
    public interface IServiceService
    {
        Task<PagedResultCustom<ServiceDTO>> GetPaging(int page, int perPage, string? sortType, string? sortField, string? searchName, ClaimsPrincipal user);
        Task<List<ServiceDTO>> GetAll(ClaimsPrincipal user);
        Task<ServiceDTO> GetDetail(int id, ClaimsPrincipal user);
        Task Create(ServiceCreateDTO dto, ClaimsPrincipal user);
        Task Update(ServiceUpdateDTO dto, ClaimsPrincipal user);
        Task Delete(int id, ClaimsPrincipal user);
    }
}
