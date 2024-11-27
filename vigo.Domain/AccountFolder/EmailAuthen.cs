using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vigo.Domain.AccountFolder
{
    public class EmailAuthen
    {
        public int Id { get; set; }
        public Guid AccountId { get; set; }
        public string Token {  get; set; } = string.Empty;
        public DateTime ExprireDate { get; set; }
    }
}
