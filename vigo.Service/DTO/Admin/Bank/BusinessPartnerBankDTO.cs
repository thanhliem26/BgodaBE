using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Service.DTO.Admin.Bank
{
    public class BusinessPartnerBankDTO
    {
        public int Id { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public int BankId { get; set; }
        public string BankName { get; set; } = string.Empty;
        public string BankNumber { get; set; } = string.Empty;
        public string Logo {  get; set; } = string.Empty;
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
