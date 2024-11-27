using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Domain.Entity
{
    public class Rating
    {
        public int Id { get; set; }
        public int TouristId { get; set; }
        public int RoomId { get; set; }
        public int Star { get; set; }
        public string Comment { get; set; } = string.Empty;
        public string UpdateComment { get; set; } = string.Empty;
        public DateTime LastUpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool Status { get; set; }
    }
}
