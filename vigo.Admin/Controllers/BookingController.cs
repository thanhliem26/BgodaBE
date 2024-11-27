using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vigo.Admin.Controllers.Base;
using vigo.Domain.Helper;
using vigo.Service.Admin.IService;
using vigo.Service.Admin.Service;
using vigo.Service.DTO.Shared;

namespace vigo.Admin.Controllers
{
    [Route("api/admin/bookings")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BookingController : BaseController
    {
        private readonly IBookingService _bookingService;
        private readonly IConfiguration _configuration;
        public BookingController(IBookingService bookingService, IConfiguration configuration)
        {
            _bookingService = bookingService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaging(int page, int perPage, bool? isReceived, string? sortType, string? sortField)
        {
            try
            {
                var data = await _bookingService.GetPaging(page, perPage, isReceived, sortType, sortField, User);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            try
            {
                var data = await _bookingService.GetDetail(id, User);
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

        [HttpPost("received")]
        public async Task<IActionResult> ReceiveBooking(List<int> ids)
        {
            try
            {
                await _bookingService.ReceiveBooking(ids, User);
                return CreateResponse(null, "recieve success", 200, null);
            }
            catch (CustomException e)
            {
                return CreateResponse(null, e.Message, 500, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return CreateResponse(null, "receive fail", 500, null);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(List<int> ids)
        {
            try
            {
                await _bookingService.Delete(ids, User);
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