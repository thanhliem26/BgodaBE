using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using vigo.Admin.Controllers.Base;
using vigo.Domain.AccountFolder;
using vigo.Domain.Helper;
using vigo.Service.Admin.IService;
using vigo.Service.Admin.Service;
using vigo.Service.DTO.Admin.Account;
using vigo.Service.DTO.Application.Account;
using vigo.Service.DTO.Shared;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace vigo.Admin.Controllers
{
    [Route("api/admin/accounts")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;
        public AccountController(IAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }

        [HttpPost("login-via-form")]
        public async Task<IActionResult> Login(LoginViaFormDTO dto)
        {
            try
            {
                var userAuthen = await _accountService.Login(dto);
                string token = CreateToken(userAuthen);
                return CreateResponse(new TokenRes() { AccessToken = token, UserType = userAuthen.UserType}, "login success", 200, null);
            }
            catch (CustomException e)
            {
                return CreateResponse(null, e.Message, 500, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return CreateResponse(null, "login fail", 500, null);
            }
        }

        [HttpGet("business-partners")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetBusinessPartnerPaging(int page, int perPage, string? sortType, string? sortField, string? searchName)
        {
            try
            {
                var data = await _accountService.GetBusinessPartnerPaging(page, perPage, sortType, sortField, searchName, User);
                Option options = new Option()
                {
                    Name = "",
                    PageSize = data.PageSize,
                    Page = data.PageIndex,
                    TotalRecords = data.TotalRecords
                };
                return CreateResponse(data.Items, "get success", 200, options);
            }
            catch (CustomException e)
            {
                return CreateResponse(null, e.Message, 500, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return CreateResponse(null, "get fail", 500, null);
            }
        }

        [HttpGet("business-partners/get-all")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllBusinessPartner()
        {
            try
            {
                var data = await _accountService.GetAllBusinessPartner(User);
                Option options = new Option()
                {
                    Name = "",
                    PageSize = data.Count(),
                    Page = 1,
                    TotalRecords = data.Count()
                };
                return CreateResponse(data, "get success", 200, options);
            }
            catch (CustomException e)
            {
                return CreateResponse(null, e.Message, 500, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return CreateResponse(null, "get fail", 500, null);
            }
        }

        [HttpGet("business-partners/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetBusinessPartnerDetail(int id)
        {
            try
            {
                var data = await _accountService.GetBusinessPartnerDetail(id, User);
                return CreateResponse(data, "get success", 200, null);
            }
            catch (CustomException e)
            {
                return CreateResponse(null, e.Message, 500, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return CreateResponse(null, "get fail", 500, null);
            }
        }

        [HttpGet("system-employees")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetEmployeePaging(int page, int perPage, string? sortType, string? sortField, string? searchName)
        {
            try
            {
                var data = await _accountService.GetEmployeePaging(page, perPage, sortType, sortField, searchName, User);
                Option options = new Option()
                {
                    Name = "",
                    PageSize = data.PageSize,
                    Page = data.PageIndex,
                    TotalRecords = data.TotalRecords
                };
                return CreateResponse(data.Items, "get success", 200, options);
            }
            catch (CustomException e)
            {
                return CreateResponse(null, e.Message, 500, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return CreateResponse(null, "get fail", 500, null);
            }
        }

        [HttpGet("get-info")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetPermission()
        {
            try
            {
                var data = await _accountService.GetPermission(User);
                return CreateResponse(data, "get success", 200, null);
            }
            catch (CustomException e)
            {
                return CreateResponse(null, e.Message, 500, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return CreateResponse(null, "get fail", 500, null);
            }
        }

        [HttpGet("tourists")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetTouristPaging(int page, int perPage, string? searchName)
        {
            try
            {
                var data = await _accountService.GetTouristPaging(page, perPage, searchName, User);
                Option options = new Option()
                {
                    Name = "",
                    PageSize = data.PageSize,
                    Page = data.PageIndex,
                    TotalRecords = data.TotalRecords
                };
                return CreateResponse(data.Items, "get success", 200, options);
            }
            catch (CustomException e)
            {
                return CreateResponse(null, e.Message, 500, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return CreateResponse(null, "get fail", 500, null);
            }
        }

        [HttpGet("tourists/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetTouristDetail(int id)
        {
            try
            {
                var data = await _accountService.GetTouristDetail(id, User);
                return CreateResponse(data, "get success", 200, null);
            }
            catch (CustomException e)
            {
                return CreateResponse(null, e.Message, 500, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return CreateResponse(null, "get fail", 500, null);
            }
        }


        [HttpPut("tourists")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateTourist(TouristUpdateDTO dto)
        {
            try
            {
                await _accountService.UpdateTourist(dto, User);
                return CreateResponse(null, "update success", 200, null);
            }
            catch (CustomException e)
            {
                return CreateResponse(null, e.Message, 500, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return CreateResponse(null, "update fail", 500, null);
            }
        }

        [HttpDelete("tourists/{accountId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteTourist(Guid accountId)
        {
            try
            {
                await _accountService.DeleteTourist(accountId, User);
                return CreateResponse(null, "delete success", 200, null);
            }
            catch (CustomException e)
            {
                return CreateResponse(null, e.Message, 500, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return CreateResponse(null, "delete fail", 500, null);
            }
        }


        [HttpGet("system-employees/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetEmployeeDetail(int id)
        {
            try
            {
                var data = await _accountService.GetEmployeeDetail(id, User);
                return CreateResponse(data, "get success", 200, null);
            }
            catch (CustomException e)
            {
                return CreateResponse(null, e.Message, 500, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return CreateResponse(null, "get fail", 500, null);
            }
        }

        [HttpPost("business-partners")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateBusinessPartner(CreateBusinessAccountDTO dto)
        {
            try
            {
                await _accountService.CreateBusinessPartner(dto, User);
                return CreateResponse(null, "create success", 200, null);
            }
            catch (CustomException e)
            {
                return CreateResponse(null, e.Message, 500, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return CreateResponse(null, "create fail", 500, null);
            }
        }

        [HttpPost("employees")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeAccount dto)
        {
            try
            {
                await _accountService.CreateEmployee(dto, User);
                return CreateResponse(null, "create success", 200, null);
            }
            catch (CustomException e)
            {
                return CreateResponse(null, e.Message, 500, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return CreateResponse(null, "create fail", 500, null);
            }
        }

        [HttpPut("employees")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateEmployee(UpdateEmployeeDTO dto)
        {
            try
            {
                await _accountService.UpdateEmployee(dto, User);
                return CreateResponse(null, "update success", 200, null);
            }
            catch (CustomException e)
            {
                return CreateResponse(null, e.Message, 500, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return CreateResponse(null, "update fail", 500, null);
            }
        }

        [HttpPut("business-partners")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateBusiness(UpdateBusinessPartnerDTO dto)
        {
            try
            {
                await _accountService.UpdateBusiness(dto, User);
                return CreateResponse(null, "update success", 200, null);
            }
            catch (CustomException e)
            {
                return CreateResponse(null, e.Message, 500, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return CreateResponse(null, "update fail", 500, null);
            }
        }

        [HttpDelete("employees/{accountId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteEmployee(Guid accountId)
        {
            try
            {
                await _accountService.DeleteEmployee(accountId, User);
                return CreateResponse(null, "delete success", 200, null);
            }
            catch (CustomException e)
            {
                return CreateResponse(null, e.Message, 500, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return CreateResponse(null, "delete fail", 500, null);
            }
        }

        [HttpDelete("business-partner/{accountId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteBusinessPartner(Guid accountId)
        {
            try
            {
                await _accountService.DeleteBusinessPartner(accountId, User);
                return CreateResponse(null, "delete success", 200, null);
            }
            catch (CustomException e)
            {
                return CreateResponse(null, e.Message, 500, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return CreateResponse(null, "delete fail", 500, null);
            }
        }

        private string CreateToken(UserAuthen userAuthen)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSettings:Key").Value!);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("InfoId", userAuthen.InfoId != null ? userAuthen.InfoId.ToString()! : string.Empty),
                    new Claim("RoleId", userAuthen.RoleId.ToString()),
                    new Claim("BusinessKey", userAuthen.BusinessKey),
                    new Claim("UserType", userAuthen.UserType)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
