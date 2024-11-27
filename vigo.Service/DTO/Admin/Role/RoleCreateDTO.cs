using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Service.DTO.Admin.Role
{
    public class RoleCreateDTO
    {
        public string Name { get; set; } = null!;
        public List<string>? Permission { get; set; }
    }
}
