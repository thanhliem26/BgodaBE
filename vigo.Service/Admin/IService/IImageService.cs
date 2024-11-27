﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Service.Admin.IService
{
    public interface IImageService
    {
        Task<string> Upload(IFormFile image, ClaimsPrincipal user);
    }
}
