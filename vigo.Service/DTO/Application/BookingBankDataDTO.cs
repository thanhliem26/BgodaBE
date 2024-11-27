using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Service.DTO.Application
{
    public class BookingBankDataDTO
    {
        public string OwnerName { get; set; } = string.Empty;
        public string BankNumber { get; set; } = string.Empty;
        public string BankName { get; set;} = string.Empty;
        public string Logo { get; set;} = string.Empty;
        public string QRURL { get; set;} = string.Empty;
    }
}
