using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.Helper;
using vigo.Domain.Interface.IGeneric;
using vigo.Domain.User;

namespace vigo.Domain.Interface.IRepository
{
    public interface IBusinessPartnerRepository : IVigoGeneric<BusinessPartner>
    {
    }
}
