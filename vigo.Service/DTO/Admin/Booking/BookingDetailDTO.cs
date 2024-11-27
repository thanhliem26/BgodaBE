using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vigo.Service.DTO.Admin.Account;
using vigo.Service.DTO.Admin.Room;

namespace vigo.Service.DTO.Admin.Booking
{
    public class BookingDetailDTO
    {
        public int Id { get; set; }
        public TouristDetailDTO Tourist { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public RoomDetailDTO Room { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string CCCD { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string DiscountCode { get; set; } = string.Empty;
        public decimal DiscountPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public bool Approved { get; set; }
    }
}
