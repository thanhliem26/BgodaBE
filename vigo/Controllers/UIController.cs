using Microsoft.AspNetCore.Mvc;
using vigo.Controllers.Base;
using vigo.Domain.Helper;
using vigo.Service.Application.IServiceApp;
using vigo.Service.Application.ServiceApp;
using vigo.Service.DTO.Shared;

namespace vigo.Controllers
{
    [Route("api/application/ui")]
    [ApiController]
    public class UIController : BaseController
    {
        private readonly IUIService _UIService;
        private readonly IConfiguration _configuration;
        public UIController(IUIService UIService, IConfiguration configuration)
        {
            _UIService = UIService;
            _configuration = configuration;
        }

        [HttpGet("get-popular-visit")]
        public async Task<IActionResult> GetPopularVisit()
        {
            try
            {
                var data = await _UIService.GetPopularVisit();
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

        [HttpGet("business-partner")]
        public async Task<IActionResult> GetAllBusinessPartnerShort()
        {
            try
            {
                var data = await _UIService.GetAllBusinessPartnerShort();
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

        [HttpGet("get-province")]
        public async Task<IActionResult> GetAllProvince()
        {
            try
            {
                var data = await _UIService.GetVisitProvince();
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
