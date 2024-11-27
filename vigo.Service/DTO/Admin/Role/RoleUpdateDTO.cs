using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Service.DTO.Admin.Role
{
    public class RoleUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<string>? Permission { get; set; }
    }
}
