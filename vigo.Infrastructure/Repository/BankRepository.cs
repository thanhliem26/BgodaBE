using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.Entity;
using vigo.Domain.Interface.IRepository;
using vigo.Infrastructure.DBContext;
using vigo.Infrastructure.Generic;

namespace vigo.Infrastructure.Repository
{
    public class BankRepository : VigoGeneric<Bank>, IBankRepository
    {
        public BankRepository(VigoDatabaseContext context) : base(context)
        {
        }
    }
}
