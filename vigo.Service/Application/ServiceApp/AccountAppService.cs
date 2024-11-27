using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using vigo.Domain.AccountFolder;
using vigo.Domain.Helper;
using vigo.Domain.Interface.IUnitOfWork;
using vigo.Domain.User;
using vigo.Service.Application.IServiceApp;
using vigo.Service.DTO.Admin.Account;
using vigo.Service.DTO.Application.Account;
using vigo.Service.DTO.Shared;
using vigo.Service.EmailAuthenModule;

namespace vigo.Service.Application.ServiceApp
{
    public class AccountAppService : IAccountAppService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWorkVigo _unitOfWorkVigo;
        private readonly EmailAuthenProducer _emailAuthenProducer;

        public AccountAppService(IUnitOfWorkVigo unitOfWorkVigo, IMapper mapper)
        {
            _unitOfWorkVigo = unitOfWorkVigo;
            _mapper = mapper;
            _emailAuthenProducer = EmailAuthenProducer.Instance;
        }

        public async Task ActiveEmail(string token)
        {
            var emailAuthen = await _unitOfWorkVigo.EmailAuthens.GetDetailBy(e => e.Token.Equals(token));
            if (emailAuthen!.ExprireDate < DateTime.Now)
            {
                throw new CustomException("hết thời hạn xác thực, hãy đăng ký xác thực lại");
            }
            var account = await _unitOfWorkVigo.Accounts.GetDetailBy(e => e.Id == emailAuthen.AccountId);
            account!.EmailActive = true;
            await _unitOfWorkVigo.Complete();
        }

        public async Task<TouristDTO> GetTouristInfo(ClaimsPrincipal user)
        {
            int infoId = int.Parse(user.FindFirst("InfoId")!.Value);
            var info = await _unitOfWorkVigo.Tourists.GetById(infoId);
            var account = await _unitOfWorkVigo.Accounts.GetDetailBy(e => e.Id.Equals(info.AccountId));
            var data = _mapper.Map<TouristDTO>(info);
            data.Email = account!.Email;
            return _mapper.Map<TouristDTO>(data);
        }

        public async Task Register(TouristRegisterDTO dto)
        {
            var checkUnique = await _unitOfWorkVigo.Accounts.GetDetailBy(e => e.Email == dto.Email);
            if (checkUnique != null)
            {
                throw new CustomException("email đã tồn tại");
            }
            if (!Regex.IsMatch(dto.Email, $@"{ConstRegex.EMAIL_REGEX}"))
            {
                throw new CustomException("email không hợp lệ");
            }
            if (dto.Password.Length < 8)
            {
                throw new CustomException("độ dài mật khẩu không đạt");
            }
            var salt = PasswordHasher.CreateSalt();
            var hashedPassword = PasswordHasher.HashPassword(dto.Password, salt);
            Guid accountId = Guid.NewGuid();
            var DateNow = DateTime.Now;
            string[] temp = dto.FullName.Split(' ');

            Account account = new Account()
            {
                Id = accountId,
                CreatedDate = DateNow,
                DeletedDate = null,
                UpdatedDate = DateNow,
                Email = dto.Email,
                Password = hashedPassword,
                EmailActive = false,
                RoleId = 2,
                UserType = "Tourist",
                Salt = salt
            };
            _unitOfWorkVigo.Accounts.Create(account);

            Tourist info = new Tourist()
            {
                AccountId = accountId,
                CreatedDate = DateNow,
                DeletedDate = null,
                UpdatedDate = DateNow,
                Name = temp.Last(),
                FullName = dto.FullName,
                Avatar = "http://localhost:2002/resource/default-avatar.jpg",
                Address = "",
                Gender = "",
                PhoneNumber = ""
            };
            _unitOfWorkVigo.Tourists.Create(info);

            EmailAuthen emailAuthen = new EmailAuthen()
            {
                AccountId = accountId,
                ExprireDate = DateNow.AddDays(1),
                Token = PasswordHasher.HashPassword(dto.Email + DateNow.AddDays(1).ToString(), PasswordHasher.CreateSalt())
            };
            _unitOfWorkVigo.EmailAuthens.Create(emailAuthen);
            await _unitOfWorkVigo.Complete();

            EmailAuthenDTO emailAuthenDTO = new EmailAuthenDTO()
            {
                Email = dto.Email,
                Url = $"http://localhost:2002/api/application/accounts/active-email?token={emailAuthen.Token}"
            };
            _emailAuthenProducer.SendEmailAuthen(emailAuthenDTO);
        }

        public async Task ResendActiveEmail(string email)
        {
            var data = await _unitOfWorkVigo.Accounts.GetDetailBy(e => e.Email.Equals(email));
            if (data == null) {
                throw new CustomException("tài khoản không tồn tại");
            }
            DateTime DateNow = DateTime.Now;
            EmailAuthen emailAuthen = new EmailAuthen()
            {
                AccountId = data.Id,
                ExprireDate = DateNow.AddDays(1),
                Token = PasswordHasher.HashPassword(data.Email + DateNow.AddDays(1).ToString(), PasswordHasher.CreateSalt())
            };
            _unitOfWorkVigo.EmailAuthens.Create(emailAuthen);
            await _unitOfWorkVigo.Complete();

            EmailAuthenDTO emailAuthenDTO = new EmailAuthenDTO()
            {
                Email = data.Email,
                Url = $"http://localhost:2002/api/application/accounts/active-email/{emailAuthen.Token}"
            };
            _emailAuthenProducer.SendEmailAuthen(emailAuthenDTO);
        }

        public async Task UpdatePassword(UpdatePasswordAppDTO dto, ClaimsPrincipal user)
        {
            if (dto.NewPassword.Length < 8)
            {
                throw new CustomException("độ dài mật khẩu không đạt");
            }
            int infoId = int.Parse(user.FindFirst("InfoId")!.Value);
            var info = await _unitOfWorkVigo.Tourists.GetDetailBy(e => e.Id == infoId);
            var account = await _unitOfWorkVigo.Accounts.GetDetailBy(e => e.Id.Equals(info!.AccountId));
            if (!account!.Password.Equals(PasswordHasher.HashPassword(dto.OldPassword, account!.Salt)))
            {
                throw new CustomException("mật khẩu cũ không chính xác");
            }
            var hashedPassword = PasswordHasher.HashPassword(dto.NewPassword, account.Salt);
            account.Password = hashedPassword;

            await _unitOfWorkVigo.Complete();
        }

        public async Task UpdateTouristInfo(TouristUpdateDTO dto, ClaimsPrincipal user)
        {
            int infoId = int.Parse(user.FindFirst("InfoId")!.Value);
            var info = await _unitOfWorkVigo.Tourists.GetById(infoId);
            var account = await _unitOfWorkVigo.Accounts.GetDetailBy(e => e.Id == info.AccountId);
            if (!dto.PhoneNumber.IsNullOrEmpty() && !Regex.IsMatch(dto.PhoneNumber!, $@"{ConstRegex.PHONE_REGEX}"))
            {
                throw new CustomException("số điện thoại không hợp lệ");
            }
            DateTime dateNow = DateTime.Now;
            account!.UpdatedDate = dateNow;

            info.UpdatedDate = dateNow;
            info.DOB = dto.DOB != null ? (DateTime)dto.DOB : info.DOB;
            info.Gender = dto.Gender != null ? dto.Gender : "";
            info.FullName = dto.FullName;
            info.Name = dto.FullName.Split(' ').Last();
            info.Avatar = dto.Avatar;
            info.PhoneNumber = dto.PhoneNumber != null ? dto.PhoneNumber : "";
            info.Address = dto.Address != null ? dto.Address : "";

            await _unitOfWorkVigo.Complete();
        }
    }
}
