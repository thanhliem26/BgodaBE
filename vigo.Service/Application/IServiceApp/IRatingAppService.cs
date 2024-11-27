using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using vigo.Service.DTO.Application.Rating;

namespace vigo.Service.Application.IServiceApp
{
    public interface IRatingAppService
    {
        Task<List<RoomRatingDTO>> GetRoomRating(int roomId, ClaimsPrincipal user);
        Task RateRoom(RateRoomDTO dto, ClaimsPrincipal user);
        Task UpdateRateRoom(UpdateRateRoomDTO dto, ClaimsPrincipal user);
    }
}
