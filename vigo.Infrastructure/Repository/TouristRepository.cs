using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.Interface.IRepository;
using vigo.Domain.User;
using vigo.Infrastructure.DBContext;
using vigo.Infrastructure.Generic;

namespace vigo.Infrastructure.Repository
{
    public class TouristRepository : VigoGeneric<Tourist>, ITouristRepository
    {
        public TouristRepository(VigoDatabaseContext context) : base(context)
        {
        }
    }
}
