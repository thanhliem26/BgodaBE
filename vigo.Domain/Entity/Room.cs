using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Domain.Entity
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Thumbnail { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Avaiable {  get; set; }
        public int RoomTypeId { get; set; }
        public int BookNumber { get; set; }
        public int BusinessPartnerId { get; set; }
        public decimal DefaultDiscount { get; set; }
        public decimal Star { get; set; } = 5;
        public string ProvinceId {  get; set; } = string.Empty;
        public string DistrictId {  get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
