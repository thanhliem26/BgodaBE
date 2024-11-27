using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Service.DTO.Admin.Account
{
    public class EmployeeDetailDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = null!;
        public DateTime DOB { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public string Bank { get; set; } = string.Empty;
        public string BankNumber { get; set; } = string.Empty;
        public Guid AccountId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
