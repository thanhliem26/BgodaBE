using Microsoft.AspNetCore.Mvc;
using vigo.Controllers.Base;
using vigo.Domain.Helper;
using vigo.Service.Application.IServiceApp;
using vigo.Service.DTO.Shared;

namespace vigo.Controllers
{
    [Route("api/application/services")]
    [ApiController]
    public class ServiceController : BaseController
    {
        private readonly IServiceAppService _serviceAppService;
        private readonly IConfiguration _configuration;
        public ServiceController(IServiceAppService serviceAppService, IConfiguration configuration)
        {
            _serviceAppService = serviceAppService;
            _configuration = configuration;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _serviceAppService.GetAll();
                Option options = new Option()
                {
                    Name = "",
                    Page = 1,
                    PageSize = data.Count(),
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
    }
}
