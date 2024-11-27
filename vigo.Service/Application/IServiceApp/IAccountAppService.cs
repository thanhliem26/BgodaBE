using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using vigo.Service.DTO;
using vigo.Service.DTO.Admin.Account;
using vigo.Service.DTO.Application.Account;

namespace vigo.Service.Application.IServiceApp
{
    public interface IAccountAppService
    {
        Task<TouristDTO> GetTouristInfo(ClaimsPrincipal user);
        Task UpdateTouristInfo(TouristUpdateDTO dto, ClaimsPrincipal user);
        Task Register(TouristRegisterDTO dto);
        Task UpdatePassword(UpdatePasswordAppDTO dto, ClaimsPrincipal user);
        Task ActiveEmail(string token);
        Task ResendActiveEmail(string email);
    }
}
