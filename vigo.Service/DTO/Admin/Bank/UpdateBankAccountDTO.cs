using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Service.DTO.Admin.Bank
{
    public class UpdateBankAccountDTO
    {
        public int Id { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public string BankNumber { get; set; } = string.Empty;
        public bool Status { get; set; }
    }
}
