using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Domain.Entity
{
    public class BusinessPartnerBank
    {
        public int Id { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public int BankId { get; set; }
        public int BusinessPartnerId { get; set; }
        public string BankNumber { get; set; } = string.Empty;
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
