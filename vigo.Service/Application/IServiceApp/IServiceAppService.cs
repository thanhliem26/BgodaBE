using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vigo.Service.DTO.Admin.Service;

namespace vigo.Service.Application.IServiceApp
{
    public interface IServiceAppService
    {
        Task<List<ServiceDTO>> GetAll();
    }
}
