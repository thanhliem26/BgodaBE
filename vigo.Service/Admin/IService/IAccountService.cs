using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.AccountFolder;
using vigo.Domain.Helper;
using vigo.Service.DTO.Admin.Account;
using vigo.Service.DTO.Admin.Role;
using vigo.Service.DTO.Application.Account;
using vigo.Service.DTO.Shared;

namespace vigo.Service.Admin.IService
{
    public interface IAccountService
    {
        Task<UserAuthen> Login(LoginViaFormDTO dto);
        Task<PermissionDTO> GetPermission(ClaimsPrincipal user);
        Task<PagedResultCustom<BusinessPartnerDTO>> GetBusinessPartnerPaging(int page, int perPage, string? sortType, string? sortField, string? searchName, ClaimsPrincipal user);
        Task<List<BusinessPartnerShortDTO>> GetAllBusinessPartner(ClaimsPrincipal user);
        Task<BusinessPartnerDetailDTO> GetBusinessPartnerDetail(int id, ClaimsPrincipal user);
        Task<PagedResultCustom<EmployeeDTO>> GetEmployeePaging(int page, int perPage, string? sortType, string? sortField, string? searchName, ClaimsPrincipal user);
        Task<EmployeeDetailDTO> GetEmployeeDetail(int id, ClaimsPrincipal user);
        Task CreateBusinessPartner(CreateBusinessAccountDTO dto, ClaimsPrincipal user);
        Task CreateEmployee(CreateEmployeeAccount dto, ClaimsPrincipal user);
        Task UpdateEmployee(UpdateEmployeeDTO dto, ClaimsPrincipal user);
        Task UpdateBusiness(UpdateBusinessPartnerDTO dto, ClaimsPrincipal user);
        Task DeleteEmployee(Guid accountId, ClaimsPrincipal user);
        Task DeleteBusinessPartner(Guid accountId, ClaimsPrincipal user);
        Task<PagedResultCustom<TouristDTO>> GetTouristPaging(int page, int perPage, string? searchName, ClaimsPrincipal user);
        Task<TouristDetailDTO> GetTouristDetail(int id, ClaimsPrincipal user);
        Task UpdateTourist(TouristUpdateDTO dto, ClaimsPrincipal user);
        Task DeleteTourist(Guid accountId, ClaimsPrincipal user);

    }
}
