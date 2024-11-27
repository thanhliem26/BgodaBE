using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Domain.AccountFolder
{
    public class RolePermission
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string RoleLabel {  get; set; } = string.Empty;
    }
}
