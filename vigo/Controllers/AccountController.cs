using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vigo.Controllers.Base;
using vigo.Domain.Helper;
using vigo.Service.Application.IServiceApp;
using vigo.Service.DTO;
using vigo.Service.DTO.Admin.Account;
using vigo.Service.DTO.Application.Account;

namespace vigo.Controllers
{
    [Route("api/application/accounts")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IAccountAppService _accountService;
        private readonly IConfiguration _configuration;
        public AccountController(IAccountAppService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(TouristRegisterDTO dto)
        {
            try
            {
                await _accountService.Register(dto);
                return CreateResponse(null, "register success", 200, null);
            }
            catch (CustomException e)
            {
                return CreateResponse(null, e.Message, 500, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return CreateResponse(null, "register fail", 500, null);
            }
        }

        [HttpPost("update-password")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordAppDTO dto)
        {
            try
            {
                await _accountService.UpdatePassword(dto, User);
                return CreateResponse(null, "register success", 200, null);
            }
            catch (CustomException e)
            {
                return CreateResponse(null, e.Message, 500, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return CreateResponse(null, "register fail", 500, null);
            }
        }

        [HttpPost("resend-active-email")]
        public async Task<IActionResult> ResendActiveEmail(string email)
        {
            try
            {
                await _accountService.ResendActiveEmail(email);
                return CreateResponse(null, "resend success", 200, null);
            }
            catch (CustomException e)
            {
                return CreateResponse(null, e.Message, 500, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return CreateResponse(null, "resend fail", 500, null);
            }
        }

        [HttpGet("info")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetTouristInfo()
        {
            try
            {
                var data = await _accountService.GetTouristInfo(User);
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

        [HttpPost("active-email")]
        public async Task<IActionResult> ActiveEmail(string token)
        {
            try
            {
                await _accountService.ActiveEmail(token);
                return CreateResponse(null, "active success", 200, null);
            }
            catch (CustomException e)
            {
                return CreateResponse(null, e.Message, 500, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return CreateResponse(null, "active fail", 500, null);
            }
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateTouristInfo(TouristUpdateDTO dto)
        {
            try
            {
                await _accountService.UpdateTouristInfo(dto, User);
                return CreateResponse(null, "register success", 200, null);
            }
            catch (CustomException e)
            {
                return CreateResponse(null, e.Message, 500, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return CreateResponse(null, "register fail", 500, null);
            }
        }
    }
}
