using AutoMapper;
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
using vigo.Service.DTO.Admin.Account;
using vigo.Service.DTO.Admin.Discount;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace vigo.Service.Admin.Service
{
    public class DiscountCouponService : IDiscountCouponService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWorkVigo _unitOfWorkVigo;

        public DiscountCouponService(IUnitOfWorkVigo unitOfWorkVigo, IMapper mapper)
        {
            _unitOfWorkVigo = unitOfWorkVigo;
            _mapper = mapper;
        }

        public async Task Create(CreateDiscountCouponDTO dto, ClaimsPrincipal user)
        {
            int infoId = int.Parse(user.FindFirst("InfoId")!.Value);
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("discount_manage") || user.FindFirst("BusinessKey")!.Value.IsNullOrEmpty())
            {
                throw new CustomException("không có quyền");
            }
            var DateNow = DateTime.Now;
            List<Expression<Func<DiscountCoupon, bool>>> conditions = new List<Expression<Func<DiscountCoupon, bool>>>()
            {
                e => e.DeletedDate == null
            };
            var count = (await _unitOfWorkVigo.DiscountCoupons.GetAll(conditions)).Count();
            DiscountCoupon data = new DiscountCoupon()
            {
                EndDate = dto.EndDate,
                StartDate = dto.StartDate,
                CreatedDate = DateNow,
                DeletedDate = null,
                Description = dto.Description,
                DiscountCode = GenerateString.GenerateRandomString(6) + count,
                DiscountCount = 0,
                DiscountMax = dto.DiscountMax,
                DiscountType = dto.DiscountType,
                Image = dto.Image,
                Name = dto.Name,
                DiscountValue = dto.DiscountValue,
                BusinessPartnerId = infoId,
                UpdatedDate = DateNow,
                RoomApply = string.Join(",", dto.RoomApplyIds)
            };
            _unitOfWorkVigo.DiscountCoupons.Create(data);
            await _unitOfWorkVigo.Complete();
        }

        public async Task Delete(int id, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("discount_manage"))
            {
                throw new CustomException("không có quyền");
            }
            var DateNow = DateTime.Now;
            var data = await _unitOfWorkVigo.DiscountCoupons.GetById(id);
            data.DeletedDate = DateNow;
            await _unitOfWorkVigo.Complete();
        }

        public async Task<List<DiscountCouponDTO>> GetAll(ClaimsPrincipal user)
        {
            List<Expression<Func<DiscountCoupon, bool>>> conditions = new List<Expression<Func<DiscountCoupon, bool>>>()
            {
                e => e.DeletedDate == null,
            };
            return _mapper.Map<List<DiscountCouponDTO>>(await _unitOfWorkVigo.DiscountCoupons.GetAll(conditions));
        }

        public async Task<DiscountCouponDetailDTO> GetDetail(int id, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("discount_manage"))
            {
                throw new CustomException("không có quyền");
            }
            var data = await _unitOfWorkVigo.DiscountCoupons.GetById(id);
            return new DiscountCouponDetailDTO()
            {
                CreatedDate = data.CreatedDate,
                DeletedDate = data.DeletedDate,
                Description = data.Description,
                DiscountCode = data.DiscountCode,
                DiscountCount = data.DiscountCount,
                DiscountMax = data.DiscountMax,
                DiscountType = data.DiscountType,
                EndDate = data.EndDate,
                DiscountValue = data.DiscountValue,
                Id = data.Id,
                Image = data.Image,
                Name = data.Name,
                RoomApplyIds = data.RoomApply.Split(",").Select(int.Parse).ToList(),
                StartDate = data.StartDate,
                UpdatedDate = data.UpdatedDate
            };
        }

        public async Task<PagedResultCustom<DiscountCouponDTO>> GetPaging(int page, int perPage, string? sortType, string? sortField, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            string userType = user.FindFirst("UserType")!.Value;
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("discount_manage"))
            {
                throw new CustomException("không có quyền");
            }
            List<Expression<Func<DiscountCoupon, bool>>> conditions = new List<Expression<Func<DiscountCoupon, bool>>>()
            {
                e => e.DeletedDate == null,
            };
            if (userType.Equals("BusinessPartner"))
            {
                conditions.Add(e => e.BusinessPartnerId == int.Parse(user.FindFirst("InfoId")!.Value));
            }
            bool sortDown = false;
            if (sortType != null && sortType.Equals("DESC"))
            {
                sortDown = true;
            }
            Expression<Func<DiscountCoupon, DateTime>>? func = null;
            if (sortField != null && sortField.Equals("startDate"))
            {
                func = e => e.StartDate;
            }
            if (sortField != null && sortField.Equals("endDate"))
            {
                func = e => e.EndDate;
            }
            var data = await _unitOfWorkVigo.DiscountCoupons.GetPaging(conditions,
                                                                       null,
                                                                       null,
                                                                       func,
                                                                       page,
                                                                       perPage,
                                                                       sortDown);
            return new PagedResultCustom<DiscountCouponDTO>(_mapper.Map<List<DiscountCouponDTO>>(data.Items), data.TotalRecords, data.PageIndex, data.PageSize);
        }

        public async Task<List<RoomShortDTO>> GetRoomApplyDiscount(ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            string userType = user.FindFirst("UserType")!.Value;
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("discount_manage") || !userType.Equals("BusinessPartner"))
            {
                throw new CustomException("không có quyền");
            }
            int infoId = int.Parse(user.FindFirst("InfoId")!.Value);
            List<Expression<Func<Room, bool>>> conditions = new List<Expression<Func<Room, bool>>>()
            {
                e => e.DeletedDate == null,
                e => e.BusinessPartnerId == infoId
            };
            var data = await _unitOfWorkVigo.Rooms.GetAll(conditions);
            return _mapper.Map<List<RoomShortDTO>>(data);
        }

        public async Task Update(DiscountCouponUpdateDTO dto, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("discount_manage"))
            {
                throw new CustomException("không có quyền");
            }
            var data = await _unitOfWorkVigo.DiscountCoupons.GetById(dto.Id);
            data.DiscountMax = dto.DiscountMax;
            data.RoomApply = string.Join(",", dto.RoomApplyIds);
            data.StartDate = dto.StartDate;
            data.EndDate = dto.EndDate;
            data.UpdatedDate = DateTime.Now;
            data.Description = dto.Description;
            data.Image = dto.Image;
            data.DiscountValue = dto.DiscountValue;
            data.DiscountType = dto.DiscountType;
            data.Name = dto.Name;
            await _unitOfWorkVigo.Complete();
        }
    }
}
