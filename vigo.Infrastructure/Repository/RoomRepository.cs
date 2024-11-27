using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.AccountFolder;
using vigo.Domain.Entity;
using vigo.Domain.Interface.IRepository;
using vigo.Infrastructure.DBContext;
using vigo.Infrastructure.Generic;

namespace vigo.Infrastructure.Repository
{
    public class RoomRepository : VigoGeneric<Room>, IRoomRepository
    {
        public RoomRepository(VigoDatabaseContext context) : base(context)
        {
        }
    }
}
