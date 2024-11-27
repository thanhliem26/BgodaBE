using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Domain.Entity
{
    public class Booking
    {
        public int Id { get; set; }
        public int TouristId { get; set; }
        public int BusinessPartnerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string CCCD {  get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int RoomId { get; set; }
        public decimal Price { get; set; }
        public string DiscountCode { get; set; } = string.Empty;
        public decimal DiscountPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public bool Approved { get; set; }
        public bool IsCheckOut { get; set; }
    }
}
