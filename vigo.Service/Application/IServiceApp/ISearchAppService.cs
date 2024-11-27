using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.Helper;
using vigo.Service.DTO.Application.Room;
using vigo.Service.DTO.Application.Search;

namespace vigo.Service.Application.IServiceApp
{
    public interface ISearchAppService
    {
        Task<SearchResultDTO> ReturnSearchTyping(string? searchInput);
        Task<PagedResultCustom<RoomAppDTO>> ReturnSearchResult(GetRoomSearchDTO dto);
    }
}
