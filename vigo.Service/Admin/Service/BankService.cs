using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.Entity;
using vigo.Domain.Helper;
using vigo.Domain.Interface.IUnitOfWork;
using vigo.Service.Admin.IService;
using vigo.Service.DTO.Admin.Bank;
using vigo.Service.DTO.Admin.Booking;
using vigo.Service.DTO.Shared;

namespace vigo.Service.Admin.Service
{
    public class BankService : IBankService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWorkVigo _unitOfWorkVigo;

        public BankService(IUnitOfWorkVigo unitOfWorkVigo, IMapper mapper)
        {
            _unitOfWorkVigo = unitOfWorkVigo;
            _mapper = mapper;
        }
        public async Task AddBankAccount(CreateBankAccountDTO dto, ClaimsPrincipal user)
        {
            int infoId = int.Parse(user.FindFirst("InfoId")!.Value);
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            string businessKey = user.FindFirst("BusinessKey")!.Value;
            if (businessKey.IsNullOrEmpty() || !role.Permission.Split(",").Contains("bank_manage"))
            {
                throw new CustomException("không có quyền");
            }
            var checkUnique = await _unitOfWorkVigo.BusinessPartnerBanks.GetDetailBy(e => e.BankId == dto.BankId &&
                                                                                          e.BusinessPartnerId == infoId &&
                                                                                          e.BankNumber == dto.BankNumber);
            if (checkUnique != null)
            {
                throw new CustomException("tài khoản ngân hàng đã tồn tại");
            }
            DateTime DateNow = DateTime.Now;
            _unitOfWorkVigo.BusinessPartnerBanks.Create(new BusinessPartnerBank()
            {
                BankId = dto.BankId,
                OwnerName = dto.OwnerName,
                BankNumber = dto.BankNumber,
                BusinessPartnerId = infoId,
                CreatedDate = DateNow,
                DeletedDate = null,
                UpdatedDate = DateNow,
                Status = true
            });
            await _unitOfWorkVigo.Complete();
        }

        public async Task DeleteBankAccount(int id, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("bank_manage"))
            {
                throw new CustomException("không có quyền");
            }
            var data = await _unitOfWorkVigo.BusinessPartnerBanks.GetById(id);
            _unitOfWorkVigo.BusinessPartnerBanks.Delete(data);
            await _unitOfWorkVigo.Complete();
        }

        public async Task<List<BankDTO>> GetAll(ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("bank_manage"))
            {
                throw new CustomException("không có quyền");
            }
            return _mapper.Map<List<BankDTO>>(await _unitOfWorkVigo.Banks.GetAll(null));
        }

        public async Task<BusinessPartnerBankDTO> GetBusinessBankDetail(int id, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("bank_manage"))
            {
                throw new CustomException("không có quyền");
            }
            var data = await _unitOfWorkVigo.BusinessPartnerBanks.GetById(id);
            return new BusinessPartnerBankDTO()
            {
                BankNumber = data.BankNumber,
                CreatedDate = data.CreatedDate,
                DeletedDate = data.DeletedDate,
                Id = id,
                BankId = data.BankId,
                Status = data.Status,
                UpdatedDate = data.UpdatedDate,
                OwnerName = data.OwnerName,
                Logo = (await _unitOfWorkVigo.Banks.GetById(data.BankId)).Logo,
                BankName = (await _unitOfWorkVigo.Banks.GetById(data.BankId)).Name
            };
        }

        public async Task<PagedResultCustom<BusinessPartnerBankDTO>> GetPagingBusinessBank(int page, int perPage, string? sortType, string? sortField,ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            string userType = user.FindFirst("UserType")!.Value;
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("bank_manage"))
            {
                throw new CustomException("không có quyền");
            }
            List<Expression<Func<BusinessPartnerBank, bool>>> conditions = new List<Expression<Func<BusinessPartnerBank, bool>>>();

            if (userType.Equals("BusinessPartner"))
            {
                conditions.Add(e => e.BusinessPartnerId == int.Parse(user.FindFirst("InfoId")!.Value));
            }
            bool sortDown = false;
            if (sortType != null && sortType.Equals("DESC"))
            {
                sortDown = true;
            }
            var data = await _unitOfWorkVigo.BusinessPartnerBanks.GetPaging(conditions,
                                                                            null,
                                                                            null,
                                                                            sortField != null && sortField.Equals("createdDate") ? e => e.CreatedDate : null,
                                                                            page,
                                                                            perPage,
                                                                            sortDown);
            var result = new List<BusinessPartnerBankDTO>();
            foreach (var item in data.Items) {
                result.Add(new BusinessPartnerBankDTO {
                    BankNumber = item.BankNumber,
                    BankName = (await _unitOfWorkVigo.Banks.GetById(item.BankId)).Name,
                    CreatedDate = item.CreatedDate,
                    DeletedDate = item.DeletedDate,
                    Id = item.Id,
                    BankId = item.BankId,
                    Status = item.Status,
                    OwnerName = item.OwnerName,
                    Logo = (await _unitOfWorkVigo.Banks.GetById(item.BankId)).Logo,
                    UpdatedDate = item.UpdatedDate
                });
            }
            return new PagedResultCustom<BusinessPartnerBankDTO>(result, data.TotalRecords, data.PageIndex, data.PageSize);
        }

        public async Task UpdateBankAccount(UpdateBankAccountDTO dto, ClaimsPrincipal user)
        {
            int infoId = int.Parse(user.FindFirst("InfoId")!.Value);
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("bank_manage"))
            {
                throw new CustomException("không có quyền");
            }
            var data = await _unitOfWorkVigo.BusinessPartnerBanks.GetById(dto.Id);
            var checkUnique = await _unitOfWorkVigo.BusinessPartnerBanks.GetDetailBy(e => e.BankId == data.BankId &&
                                                                                          e.BusinessPartnerId == infoId &&
                                                                                          e.BankNumber == dto.BankNumber &&
                                                                                          e.Id != dto.Id);
            if (checkUnique != null)
            {
                throw new CustomException("tài khoản ngân hàng đã tồn tại");
            }
            data.BankNumber = dto.BankNumber;
            data.Status = dto.Status;
            data.OwnerName = dto.OwnerName;
            await _unitOfWorkVigo.Complete();
        }
    }
}
