using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.AccountFolder;
using vigo.Domain.Interface.IGeneric;

namespace vigo.Domain.Interface.IRepository
{
    public interface IAccountRepository : IVigoGeneric<Account>
    {
        Task<UserAuthen> LoginViaForm(string email, string password);
    }
}
