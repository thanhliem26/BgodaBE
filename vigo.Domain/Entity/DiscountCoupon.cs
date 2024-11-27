using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.Helper;

namespace vigo.Domain.Entity
{
    public class DiscountCoupon
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image {  get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public string DiscountCode { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DiscountMax { get; set; }
        public int DiscountCount { get; set; }
        public string UserUsed { get; set; } = string.Empty;
        public string RoomApply {  get; set; } = string.Empty;
        public int BusinessPartnerId {  get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
