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
    public class BookingRepository : VigoGeneric<Booking>, IBookingRepository
    {
        public BookingRepository(VigoDatabaseContext context) : base(context)
        {
        }
    }
}
