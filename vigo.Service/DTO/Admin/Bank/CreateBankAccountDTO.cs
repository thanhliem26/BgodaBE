using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Service.DTO.Admin.Bank
{
    public class CreateBankAccountDTO
    {
        public string OwnerName { get; set; } = string.Empty;
        public int BankId { get; set; }
        public string BankNumber { get; set; } = string.Empty;
    }
}
