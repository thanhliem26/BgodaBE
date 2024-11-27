using Microsoft.AspNetCore.Mvc;
using vigo.Admin.Controllers.Base;
using vigo.Domain.Helper;
using vigo.Service.Admin.IService;
using vigo.Service.DTO.Admin.Room;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using vigo.Service.DTO.Admin.Service;
using vigo.Service.DTO.Shared;

namespace vigo.Admin.Controllers
{
    [Route("api/admin/services")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ServiceController : BaseController
    {
        private readonly IServiceService _serviceService;
        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaging(int page, int perPage, string? sortType, string? sortField, string? searchName)
        {
            try
            {
                var data = await _serviceService.GetPaging(page, perPage, sortType, sortField, searchName, User);
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

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _serviceService.GetAll(User);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            try
            {
                var data = await _serviceService.GetDetail(id, User);
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
        public async Task<IActionResult> Create(ServiceCreateDTO dto)
        {
            try
            {
                await _serviceService.Create(dto, User);
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
        public async Task<IActionResult> Update(ServiceUpdateDTO dto)
        {
            try
            {
                await _serviceService.Update(dto, User);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _serviceService.Delete(id, User);
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
