using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using vigo.Admin.Controllers.Base;
using vigo.Domain.Helper;
using vigo.Service.Admin.IService;
using vigo.Service.Admin.Service;
using vigo.Service.DTO.Admin.Role;
using vigo.Service.DTO.Admin.Room;
using vigo.Service.DTO.Shared;

namespace vigo.Admin.Controllers
{
    [Route("api/admin/rooms")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RoomController : BaseController
    {
        private readonly IRoomService _roomService;
        private readonly IRoomTypeService _roomTypeService;
        public RoomController(IRoomService roomService, IRoomTypeService roomTypeService)
        {
            _roomService = roomService;
            _roomTypeService = roomTypeService;
        }

        [HttpGet("room-types/get-all")]
        public async Task<IActionResult> GetAllType()
        {
            try
            {
                var data = await _roomTypeService.GetAll(User);
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

        [HttpGet("room-types")]
        public async Task<IActionResult> GetPagingType(int page, int perPage, string? sortType, string? sortField, string? searchName)
        {
            try
            {
                var data = await _roomTypeService.GetPaging(page, perPage, sortType, sortField, searchName, User);
                Option options = new Option()
                {
                    Name = "",
                    Page = data.PageIndex,
                    PageSize = data.PageSize,
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

        [HttpGet("room-types/{id}")]
        public async Task<IActionResult> GetDetailRT(int id)
        {
            try
            {
                var data = await _roomTypeService.GetDetail(id, User);
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

        [HttpPost("room-types")]
        public async Task<IActionResult> Create(RoomTypeCreateDTO dto)
        {
            try
            {
                await _roomTypeService.Create(dto, User);
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

        [HttpPut("room-types")]
        public async Task<IActionResult> Update(RoomTypeUpdateDTO dto)
        {
            try
            {
                await _roomTypeService.Update(dto, User);
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

        [HttpDelete("room-types/{id}")]
        public async Task<IActionResult> DeleteRT(int id)
        {
            try
            {
                await _roomTypeService.Delete(id, User);
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

        [HttpGet]
        public async Task<IActionResult> GetPaging(int page, int perPage, int? roomTypeId, int? businessPartnerId, string? sortType, string? sortField, string? searchName)
        {
            try
            {
                var data = await _roomService.GetPaging(page, perPage, roomTypeId, businessPartnerId, sortType, sortField, searchName, User);
                Option options = new Option()
                {
                    Name = "",
                    Page = data.PageIndex,
                    PageSize = data.PageSize,
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            try
            {
                var data = await _roomService.GetDetail(id, User);
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

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoomDTO dto)
        {
            try
            {
                await _roomService.Create(dto, User);
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

        [HttpPut]
        public async Task<IActionResult> Update(UpdateRoomDTO dto)
        {
            try
            {
                await _roomService.Update(dto, User);
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

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _roomService.Delete(id, User);
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
    }
}
