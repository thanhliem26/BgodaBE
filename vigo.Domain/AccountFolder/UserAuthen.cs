using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Domain.AccountFolder
{
    public class UserAuthen
    {
        public int? InfoId { get; set; }
        public string UserType { get; set; } = string.Empty;
        public string BusinessKey { get; set; } = string.Empty;
        public int RoleId { get; set; }
    }
}
