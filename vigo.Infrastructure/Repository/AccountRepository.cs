using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.AccountFolder;
using vigo.Domain.Helper;
using vigo.Domain.Interface.IRepository;
using vigo.Infrastructure.DBContext;
using vigo.Infrastructure.Generic;

namespace vigo.Infrastructure.Repository
{
    public class AccountRepository : VigoGeneric<Account>, IAccountRepository
    {
        public AccountRepository(VigoDatabaseContext context) : base(context)
        {
        }

        public async Task<UserAuthen> LoginViaForm(string email, string password)
        {
            var data = await _context.Accounts.Where(e => e.Email.Equals(email)).FirstOrDefaultAsync();
            if (data == null)
            {
                throw new Exception("user not found");
            }
            if (data.UserType == "Tourist" && !data.EmailActive)
            {
                throw new CustomException("Tài khoản chưa xác thực email");
            }
            if (PasswordHasher.HashPassword(password, data.Salt).Equals(data.Password))
            {
                var userAuthen = new UserAuthen();
                if (data.UserType.Equals("BusinessPartner"))
                {
                    var info = await _context.BusinessPartners.Where(e => e.AccountId.Equals(data.Id)).FirstOrDefaultAsync();
                    userAuthen.BusinessKey = info!.BusinessKey;
                    userAuthen.RoleId = data.RoleId;
                    userAuthen.UserType = "BusinessPartner";
                    userAuthen.InfoId = info.Id;
                }
                else if(data.UserType.Equals("SystemEmployee"))
                {
                    var info = await _context.SystemEmployees.Where(e => e.AccountId.Equals(data.Id)).FirstOrDefaultAsync();
                    userAuthen.RoleId = data.RoleId;
                    userAuthen.UserType = "SystemEmployee";
                    userAuthen.InfoId = info!.Id;
                }
                else
                {
                    var info = await _context.Tourists.Where(e => e.AccountId.Equals(data.Id)).FirstOrDefaultAsync();
                    userAuthen.RoleId = data.RoleId;
                    userAuthen.UserType = "Tourist";
                    userAuthen.InfoId = info!.Id;
                }
                return userAuthen;
            }
            throw new CustomException("Thông tin đăng nhập không chính xác");
        }
    }
}
