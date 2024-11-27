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
using vigo.Domain.User;
using vigo.Service.Admin.IService;
using vigo.Service.DTO.Admin.Account;
using vigo.Service.DTO.Admin.Booking;
using vigo.Service.DTO.Admin.Room;

namespace vigo.Service.Admin.Service
{
    public class BookingService : IBookingService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWorkVigo _unitOfWorkVigo;

        public BookingService(IUnitOfWorkVigo unitOfWorkVigo, IMapper mapper)
        {
            _unitOfWorkVigo = unitOfWorkVigo;
            _mapper = mapper;
        }

        public async Task Delete(List<int> ids, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("booking_manage"))
            {
                throw new CustomException("không có quyền");
            }
            List<Booking> data = new List<Booking>();
            DateTime DateNow = DateTime.Now;
            foreach (int id in ids)
            {
                data.Add(await _unitOfWorkVigo.Bookings.GetById(id));
            }
            foreach (Booking item in data)
            {
                item.DeletedDate = DateNow;
            }
            await _unitOfWorkVigo.Complete();
        }

        public async Task<BookingDetailDTO> GetDetail(int id, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("booking_manage"))
            {
                throw new CustomException("không có quyền");
            }
            var booking = await _unitOfWorkVigo.Bookings.GetById(id);
            var tourist = await _unitOfWorkVigo.Tourists.GetById(booking.TouristId);
            var room = await _unitOfWorkVigo.Rooms.GetById(booking.RoomId);
            var bookingDTO = _mapper.Map<BookingDetailDTO>(booking);
            bookingDTO.Tourist = _mapper.Map<TouristDetailDTO>(tourist);
            bookingDTO.Room = _mapper.Map<RoomDetailDTO>(room);
            var province = await _unitOfWorkVigo.Provinces.GetDetailBy(e => e.Id.Equals(room.ProvinceId));
            var district = await _unitOfWorkVigo.Districts.GetDetailBy(e => e.Id.Equals(room.DistrictId));
            bookingDTO.Room.Province = province!.Name;
            bookingDTO.Room.District = district!.Name;

            return bookingDTO;
        }

        public async Task<PagedResultCustom<BookingDTO>> GetPaging(int page, int perPage, bool? isReceived, string? sortType, string? sortField, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("booking_manage"))
            {
                throw new CustomException("không có quyền");
            }
            string userType = user.FindFirst("UserType")!.Value;
            List<Expression<Func<Booking, bool>>> conditions = new List<Expression<Func<Booking, bool>>>()
            {
                e => e.DeletedDate == null
            };
            if (userType.Equals("BusinessPartner"))
            {
                conditions.Add(e => e.BusinessPartnerId == int.Parse(user.FindFirst("InfoId")!.Value));
            }
            if (isReceived != null)
            {
                conditions.Add(e => e.Approved == isReceived);
            }
            bool sortDown = false;
            if (sortType != null && sortType == "DESC")
            {
                sortDown = true;
            }
            var data = await _unitOfWorkVigo.Bookings.GetPaging(conditions,
                                                                null,
                                                                sortField != null && sortField.Equals("totalPrice") ? e => e.TotalPrice : null,
                                                                sortField != null && sortField.Equals("createdDate") ? e => e.CreatedDate : null,
                                                                page,
                                                                perPage,
                                                                sortDown);
            return new PagedResultCustom<BookingDTO>(_mapper.Map<List<BookingDTO>>(data.Items), data.TotalRecords, data.PageIndex, data.PageSize);
        }

        public async Task ReceiveBooking(List<int> ids, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("booking_manage"))
            {
                throw new CustomException("không có quyền");
            }
            List<Booking> data = new List<Booking>();
            foreach (int id in ids) {
                data.Add(await _unitOfWorkVigo.Bookings.GetById(id));
            }
            foreach (Booking item in data) {
                var room = await _unitOfWorkVigo.Rooms.GetById(item.RoomId);
                var coupon = await _unitOfWorkVigo.DiscountCoupons.GetDetailBy(e => e.DiscountCode.Equals(item.DiscountCode));
                room.BookNumber += 1;
                room.Avaiable -=1;
                item.Approved = true;
                if (coupon != null)
                {
                    coupon!.UserUsed = coupon.UserUsed.IsNullOrEmpty() ? item.TouristId.ToString() : coupon.UserUsed + $",{item.TouristId}";
                }
            }
            await _unitOfWorkVigo.Complete();
        }
    }
}
