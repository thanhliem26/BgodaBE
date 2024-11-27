using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vigo.Controllers.Base;
using vigo.Domain.Helper;
using vigo.Service.Application.IServiceApp;
using vigo.Service.Application.ServiceApp;
using vigo.Service.DTO.Application.Rating;
using vigo.Service.DTO.Shared;

namespace vigo.Controllers
{
    [Route("api/application/ratings")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RatingController : BaseController
    {
        private readonly IRatingAppService _ratingAppService;
        private readonly IConfiguration _configuration;
        public RatingController(IRatingAppService ratingAppService, IConfiguration configuration)
        {
            _ratingAppService = ratingAppService;
            _configuration = configuration;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetRoomRating(int roomId)
        {
            try
            {
                var data = await _ratingAppService.GetRoomRating(roomId, User);
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

        [HttpPost]
        public async Task<IActionResult> RateRoom(RateRoomDTO dto)
        {
            try
            {
                await _ratingAppService.RateRoom(dto, User);
                return CreateResponse(null, "đánh giá đã được gửi để kiểm duyệt", 200, null);
            }
            catch (CustomException e)
            {
                return CreateResponse(null, e.Message, 500, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return CreateResponse(null, "rate fail", 500, null);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRateRoom(UpdateRateRoomDTO dto)
        {
            try
            {
                await _ratingAppService.UpdateRateRoom(dto, User);
                return CreateResponse(null, "chỉnh sửa đang được kiểm duyệt", 200, null);
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
