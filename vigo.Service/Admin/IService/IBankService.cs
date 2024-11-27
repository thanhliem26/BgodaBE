using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.Helper;
using vigo.Service.DTO.Admin.Bank;
using vigo.Service.DTO.Shared;

namespace vigo.Service.Admin.IService
{
    public interface IBankService
    {
        Task<List<BankDTO>> GetAll(ClaimsPrincipal user);

        Task<PagedResultCustom<BusinessPartnerBankDTO>> GetPagingBusinessBank(int page, int perPage, string? sortType, string? sortField, ClaimsPrincipal user);
        Task<BusinessPartnerBankDTO> GetBusinessBankDetail(int id, ClaimsPrincipal user);
        Task AddBankAccount(CreateBankAccountDTO dto, ClaimsPrincipal user);
        Task UpdateBankAccount(UpdateBankAccountDTO dto, ClaimsPrincipal user);
        Task DeleteBankAccount(int id, ClaimsPrincipal user);
    }
}
