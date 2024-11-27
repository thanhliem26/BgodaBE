using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.Helper;
using vigo.Service.Admin.IService;

namespace vigo.Service.Admin.Service
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _env;
        public ImageService(IWebHostEnvironment env)
        {
            _env = env;
        }
        public async Task<string> Upload(IFormFile image, ClaimsPrincipal user)
        {
            string imageExtension = Path.GetExtension(ContentDispositionHeaderValue.Parse(image.ContentDisposition).FileName!.Trim('"'));
            string imageFileName = GenerateString.GenerateRandomString(5) + DateTime.Now.ToString("yyyyMMddHHmmssfff") + imageExtension;
            string imageFilepath = Path.Combine(_env.ContentRootPath, "volume/image", imageFileName);
            using (var stream = new FileStream(imageFilepath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            return FileHelper.FileUri(imageFileName);
        }
    }
}
