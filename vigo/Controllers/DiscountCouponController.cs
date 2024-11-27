using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vigo.Controllers.Base;
using vigo.Domain.Helper;
using vigo.Service.Application.IServiceApp;
using vigo.Service.Application.ServiceApp;
using static System.Runtime.InteropServices.JavaScript.JSType;
using vigo.Service.DTO.Shared;

namespace vigo.Controllers
{
    [Route("api/application/discount-coupons")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DiscountCouponController : BaseController
    {
        private readonly IDiscountCouponAppService _discountCouponAppService;
        private readonly IConfiguration _configuration;
        public DiscountCouponController(IDiscountCouponAppService discountCouponAppService, IConfiguration configuration)
        {
            _discountCouponAppService = discountCouponAppService;
            _configuration = configuration;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPaging(int page, int perPage, string? searchName)
        {
            try
            {
                var data = await _discountCouponAppService.GetPaging(page, perPage, searchName, User);
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
        [AllowAnonymous]
        public async Task<IActionResult> GetDetail(int id)
        {
            try
            {
                var data = await _discountCouponAppService.GetDetail(id, User);
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

        [HttpGet("use-able-room")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllUseAbleRoom(int couponId)
        {
            try
            {
                var data = await _discountCouponAppService.GetAllUseAbleRoom(couponId, User);
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

        [HttpGet("use-able-coupon")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllUseAble(int roomId)
        {
            try
            {
                var data = await _discountCouponAppService.GetAllUseAble(roomId, User);
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
