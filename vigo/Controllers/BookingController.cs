using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vigo.Controllers.Base;
using vigo.Domain.Helper;
using vigo.Service.Application.IServiceApp;
using static System.Runtime.InteropServices.JavaScript.JSType;
using vigo.Service.DTO.Shared;
using System.ComponentModel.DataAnnotations;
using vigo.Service.DTO.Application.Booking;

namespace vigo.Controllers
{
    [Route("api/application/bookings")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BookingController : BaseController
    {
        private readonly IBookingAppService _bookingAppService;
        private readonly IConfiguration _configuration;
        public BookingController(IBookingAppService bookingAppService, IConfiguration configuration)
        {
            _bookingAppService = bookingAppService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaging(int page, int perPage)
        {
            try
            {
                var data = await _bookingAppService.GetPaging(page, perPage, User);
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

        [HttpGet("price")]
        public async Task<IActionResult> GetPrice(int roomId, int couponId, DateTime checkInDate, DateTime checkOutDate)
        {
            try
            {
                var data = await _bookingAppService.GetPrice(roomId, couponId, checkInDate, checkOutDate, User);
                PriceDTO result = new PriceDTO()
                {
                    Price = data
                };
                return CreateResponse(result, "get success", 200, null);
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

        [HttpPost("create-book")]
        public async Task<IActionResult> Book(CreateBookDTO dto)
        {
            try
            {
                var data = await _bookingAppService.Book(dto, User);
                Option options = new Option()
                {
                    Name = "",
                    PageSize = data.Count(),
                    Page = 1,
                    TotalRecords = data.Count()
                };
                return CreateResponse(data, "book success", 200, options);
            }
            catch (CustomException e)
            {
                return CreateResponse(null, e.Message, 500, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return CreateResponse(null, "book fail", 500, null);
            }
        }
    }
}
