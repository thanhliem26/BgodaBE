using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Service.DTO.Admin.Role
{
    public class PermissionDTO
    {
        public string Name { get; set; } = string.Empty;
        public List<string> Permission { get; set; } = new List<string>();
        public string Image { get; set; } = string.Empty;
    }
}
