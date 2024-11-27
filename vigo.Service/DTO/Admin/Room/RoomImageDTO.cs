using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Service.DTO.Admin.Room
{
    public class RoomImageDTO
    {
        public string Type { get; set; } = string.Empty;
        public List<string> Urls { get; set; } = new List<string>();
    }
}
