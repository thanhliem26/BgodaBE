using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vigo.Admin.Controllers.Base;
using vigo.Domain.Helper;
using vigo.Service.Admin.IService;
using vigo.Service.Admin.Service;
using vigo.Service.DTO.Admin.Bank;
using vigo.Service.DTO.Shared;

namespace vigo.Admin.Controllers
{
    [Route("api/admin/banks")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BankController : BaseController
    {
        private readonly IBankService _bankService;
        private readonly IConfiguration _configuration;
        public BankController(IBankService bankService, IConfiguration configuration)
        {
            _bankService = bankService;
            _configuration = configuration;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _bankService.GetAll(User);
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

        [HttpGet("business-partner-banks")]
        public async Task<IActionResult> GetPaging(int page, int perPage, string? sortType, string? sortField)
        {
            try
            {
                var data = await _bankService.GetPagingBusinessBank(page, perPage, sortType, sortField, User);
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

        [HttpGet("business-partner-banks/{id}")]
        public async Task<IActionResult> GetBusinessBankDetail(int id)
        {
            try
            {
                var data = await _bankService.GetBusinessBankDetail(id, User);
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

        [HttpPost("business-partner-banks")]
        public async Task<IActionResult> AddBankAccount(CreateBankAccountDTO dto)
        {
            try
            {
                await _bankService.AddBankAccount(dto, User);
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

        [HttpPut("business-partner-banks")]
        public async Task<IActionResult> UpdateBankAccount(UpdateBankAccountDTO dto)
        {
            try
            {
                await _bankService.UpdateBankAccount(dto, User);
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

        [HttpDelete("business-partner-banks/{id}")]
        public async Task<IActionResult> DeleteBankAccount(int id)
        {
            try
            {
                await _bankService.DeleteBankAccount(id, User);
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
    }
}
