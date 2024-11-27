using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Service.DTO.Admin.Rating
{
    public class RatingDTO
    {
        public int Id { get; set; }
        public int Rate { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime LastUpdatedDate { get; set; }
    }
}
