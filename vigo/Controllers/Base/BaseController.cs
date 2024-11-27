using Microsoft.AspNetCore.Mvc;
using vigo.Service.DTO;
using vigo.Service.DTO.Shared;

namespace vigo.Controllers.Base
{
    public class BaseController : ControllerBase
    {
        protected IActionResult CreateResponse(object? data, string message, int status, Option? options)
        {
            var response = new ResponseData
            {
                Message = message,
                Status = status,
                MetaData = data,
                Options = options
            };
            return Ok(response);
        }
    }
}
