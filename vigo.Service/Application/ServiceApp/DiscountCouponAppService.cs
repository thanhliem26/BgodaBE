using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
using vigo.Domain.User;
using vigo.Service.Application.IServiceApp;
using vigo.Service.DTO.Admin.Account;
using vigo.Service.DTO.Admin.Room;
using vigo.Service.DTO.Admin.Service;
using vigo.Service.DTO.Application.Discount;

namespace vigo.Service.Application.ServiceApp
{
    public class DiscountCouponAppService : IDiscountCouponAppService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWorkVigo _unitOfWorkVigo;

        public DiscountCouponAppService(IUnitOfWorkVigo unitOfWorkVigo, IMapper mapper)
        {
            _unitOfWorkVigo = unitOfWorkVigo;
            _mapper = mapper;
        }

        public async Task<List<DiscountCouponAppDTO>> GetAllUseAble(int roomId, ClaimsPrincipal user)
        {
            int? infoId = user.FindFirst("InfoId") != null ? int.Parse(user.FindFirst("InfoId")!.Value) : null;
            List<DiscountCouponAppDTO> result = new List<DiscountCouponAppDTO>();
            List<Expression<Func<DiscountCoupon, bool>>> con = new List<Expression<Func<DiscountCoupon, bool>>>()
            {
                e => e.EndDate > DateTime.Now,
                e => e.DeletedDate == null
            };
            var data = await _unitOfWorkVigo.DiscountCoupons.GetAll(null);
            foreach (var item in data) {
                if (item.RoomApply.Split(",").Contains(roomId.ToString()))
                {
                    result.Add(new DiscountCouponAppDTO()
                    {
                        Description = item.Description,
                        DiscountCode = item.DiscountCode,
                        DiscountCount = item.DiscountCount,
                        DiscountMax = item.DiscountMax,
                        DiscountType = item.DiscountType,
                        DiscountValue = item.DiscountValue,
                        EndDate = item.EndDate,
                        Id = item.Id,
                        Image = item.Image,
                        Name = item.Name,
                        StartDate = item.StartDate,
                        UseAble = !item.UserUsed.Split(',').Contains(infoId.ToString())
                    });
                }
            }
            return result;
        }

        public async Task<DiscountCouponAppDTO> GetDetail(int id, ClaimsPrincipal user)
        {
            int? infoId = user.FindFirst("InfoId") != null ? int.Parse(user.FindFirst("InfoId")!.Value) : null;
            var data = await _unitOfWorkVigo.DiscountCoupons.GetById(id);
            return new DiscountCouponAppDTO()
            {
                Description = data.Description,
                DiscountCode = data.DiscountCode,
                DiscountCount = data.DiscountCount,
                DiscountMax = data.DiscountMax,
                DiscountType = data.DiscountType,
                DiscountValue = data.DiscountValue,
                EndDate = data.EndDate,
                Id = data.Id,
                Image = data.Image,
                Name = data.Name,
                StartDate = data.StartDate,
                UseAble = !data.UserUsed.Split(',').Contains(infoId.ToString())
            };
        }

        public async Task<List<RoomDTO>> GetAllUseAbleRoom(int couponId, ClaimsPrincipal user)
        {
            var data = await _unitOfWorkVigo.DiscountCoupons.GetById(couponId);
            List<int> roomIds = new List<int>();
            foreach (var item in data.RoomApply.Split(",")) {
                roomIds.Add(int.Parse(item));
            }
            List<RoomDTO> result = new List<RoomDTO>();
            foreach (var item in roomIds) {
                var roomData = await _unitOfWorkVigo.Rooms.GetById(item);
                var temp = _mapper.Map<RoomDTO>(roomData);
                List<Expression<Func<RoomServiceR, bool>>> con = new List<Expression<Func<RoomServiceR, bool>>>()
                {
                    e => e.RoomId == item,
                };
                var roomServices = await _unitOfWorkVigo.RoomServices.GetAll(con);
                foreach (var service in roomServices)
                {
                    temp.Services.Add(_mapper.Map<ServiceDTO>(await _unitOfWorkVigo.Services.GetById(service.ServiceId)));
                }
                temp.BusinessPartner = _mapper.Map<BusinessPartnerShortDTO>(await _unitOfWorkVigo.BusinessPartners.GetById(temp.BusinessPartnerId));
                temp.Province = (await _unitOfWorkVigo.Provinces.GetDetailBy(e => e.Id.Equals(roomData.ProvinceId)))!.Name;
                temp.District = (await _unitOfWorkVigo.Districts.GetDetailBy(e => e.Id.Equals(roomData.DistrictId)))!.Name;
                result.Add(temp);
            }
            return result;
        }

        public async Task<PagedResultCustom<DiscountCouponAppDTO>> GetPaging(int page, int perPage, string? searchName, ClaimsPrincipal user)
        {
            int? infoId = user.FindFirst("InfoId") != null ? int.Parse(user.FindFirst("InfoId")!.Value) : null;
            List<Expression<Func<DiscountCoupon, bool>>> conditions = new List<Expression<Func<DiscountCoupon, bool>>>()
            {
                e => e.EndDate > DateTime.Now,
                e => e.DeletedDate == null
            };
            if (searchName != null)
            {
                conditions.Add(e => e.Name.ToLower().Contains(searchName.ToLower()));
            }
            var data = await _unitOfWorkVigo.DiscountCoupons.GetPaging(conditions,
                                                                       null,
                                                                       null,
                                                                       e => e.CreatedDate,
                                                                       page,
                                                                       perPage,
                                                                       true);
            var result = new List<DiscountCouponAppDTO>();
            foreach (var item in data.Items) {
                result.Add(new DiscountCouponAppDTO {
                    Description = item.Description,
                    DiscountCode = item.DiscountCode,
                    DiscountCount = item.DiscountCount,
                    DiscountMax = item.DiscountMax,
                    DiscountType = item.DiscountType,
                    DiscountValue = item.DiscountValue,
                    EndDate = item.EndDate,
                    Id = item.Id,
                    Image = item.Image,
                    Name = item.Name,
                    StartDate = item.StartDate,
                    UseAble = !item.UserUsed.Split(',').Contains(infoId.ToString())
                });
            }
            return new PagedResultCustom<DiscountCouponAppDTO>(result, data.TotalRecords, data.PageIndex, data.PageSize);
        }
    }
}
