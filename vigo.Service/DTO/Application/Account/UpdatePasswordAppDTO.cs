using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Service.DTO.Application.Account
{
    public class UpdatePasswordAppDTO
    {
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword {  get; set; } = string.Empty;
    }
}
