using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Service.DTO.Application.Account
{
    public class TouristRegisterDTO
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FullName { get; set; } = string.Empty;
        //public DateTime DOB { get; set; }
        //public string Gender { get; set; } = string.Empty;
        //public string Avatar { get; set; } = string.Empty;
    }
}
