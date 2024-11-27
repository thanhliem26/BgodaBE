using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Service.DTO.Application.Rating
{
    public class RoomRatingDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Avatar {  get; set; } = string.Empty;
        public int Rate { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime LastUpdatedDate { get; set; }
        public bool UpdateAble {  get; set; } 
    }
}
