using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vigo.Service.DTO.Admin.Room;

namespace vigo.Service.Application.IServiceApp
{
    public interface IRoomTypeAppService
    {
        Task<List<RoomTypeDTO>> GetAll();
    }
}
