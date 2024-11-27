using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vigo.Controllers.Base;
using vigo.Domain.Helper;
using vigo.Service.Application.IServiceApp;
using vigo.Service.Application.ServiceApp;
using vigo.Service.DTO.Application.Room;
using vigo.Service.DTO.Shared;

namespace vigo.Controllers
{
    [Route("api/application/rooms")]
    [ApiController]
    public class RoomController : BaseController
    {
        private readonly IRoomAppService _roomAppService;
        private readonly IRoomTypeAppService _roomTypeAppService;
        private readonly IConfiguration _configuration;
        public RoomController(IRoomAppService roomAppService, IRoomTypeAppService roomTypeAppService, IConfiguration configuration)
        {
            _roomAppService = roomAppService;
            _roomTypeAppService = roomTypeAppService;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> GetPaging(GetRoomDTO dto)
        {
            try
            {
                var data = await _roomAppService.GetPaging(dto);
                Option options = new Option()
                {
                    Name = "",
                    PageSize = dto.PerPage,
                    Page = dto.Page,
                    TotalRecords = data.RoomNumber
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            try
            {
                var data = await _roomAppService.GetDetail(id);
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

        [HttpGet("price-range")]
        public async Task<IActionResult> GetPriceRange()
        {
            try
            {
                var data = await _roomAppService.GetPriceRange();
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

        [HttpGet("related-room")]
        public async Task<IActionResult> GetRelatedRoom(int businessPartnerId)
        {
            try
            {
                var data = await _roomAppService.GetRelatedRoom(businessPartnerId);
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

        [HttpGet("roomType/get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _roomTypeAppService.GetAll();
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
    }
}
