using Microsoft.AspNetCore.Mvc;
using vigo.Controllers.Base;
using vigo.Domain.Helper;
using vigo.Service.Application.IServiceApp;
using vigo.Service.Application.ServiceApp;
using vigo.Service.DTO;
using vigo.Service.DTO.Application.Room;
using vigo.Service.DTO.Shared;

namespace vigo.Controllers
{
    [Route("api/application/search")]
    [ApiController]
    public class SearchController : BaseController
    {
        private readonly ISearchAppService _searchAppService;
        private readonly IConfiguration _configuration;
        public SearchController(ISearchAppService searchAppService, IConfiguration configuration)
        {
            _searchAppService = searchAppService;
            _configuration = configuration;
        }

        [HttpGet("suggest-result")]
        public async Task<IActionResult> ReturnSearchTyping(string? searchInput)
        {
            try
            {
                var data = await _searchAppService.ReturnSearchTyping(searchInput);
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

        [HttpPost("get-result")]
        public async Task<IActionResult> ReturnSearchResult(GetRoomSearchDTO dto)
        {
            try
            {
                var data = await _searchAppService.ReturnSearchResult(dto);
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
    }
}
