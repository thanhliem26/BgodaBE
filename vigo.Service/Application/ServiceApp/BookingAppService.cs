using AutoMapper;
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
using vigo.Service.DTO.Application;
using vigo.Service.DTO.Application.Booking;

namespace vigo.Service.Application.ServiceApp
{
    public class BookingAppService : IBookingAppService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWorkVigo _unitOfWorkVigo;

        public BookingAppService(IUnitOfWorkVigo unitOfWorkVigo, IMapper mapper)
        {
            _unitOfWorkVigo = unitOfWorkVigo;
            _mapper = mapper;
        }

        public async Task<List<BookingBankDataDTO>> Book(CreateBookDTO dto, ClaimsPrincipal user)
        {
            int infoId = int.Parse(user.FindFirst("InfoId")!.Value);
            DiscountCoupon? coupon = null;
            if (dto.CouponId != null)
            {
                coupon = await _unitOfWorkVigo.DiscountCoupons.GetById((int)dto.CouponId);
            }
            var room = await _unitOfWorkVigo.Rooms.GetById(dto.RoomId);
            decimal price = room.Price * (dto.CheckOutDate - dto.CheckInDate).Days;
            decimal discountPrice = 0;
            if (coupon != null)
            {
                if (coupon.DiscountType.ToString().Equals("fixed_amount"))
                {
                    discountPrice = coupon.DiscountValue;
                }
                if (coupon.DiscountType.ToString().Equals("percentage"))
                {
                    discountPrice = coupon.DiscountValue * price / 100;
                }
            }
            Booking data = new Booking()
            {
                Approved = false,
                BusinessPartnerId = room.BusinessPartnerId,
                DeletedDate = null,
                CheckInDate = dto.CheckInDate,
                CheckOutDate = dto.CheckOutDate,
                CreatedDate = DateTime.Now,
                DiscountCode = coupon != null ? coupon.DiscountCode : string.Empty,
                DiscountPrice = discountPrice,
                RoomId = room.Id,
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber,
                CCCD = dto.CCCD,
                Price = price,
                TotalPrice = price - discountPrice,
                TouristId = infoId,
                IsCheckOut = false
            };
            _unitOfWorkVigo.Bookings.Create(data);
            await _unitOfWorkVigo.Complete();

            List<BookingBankDataDTO> result = new List<BookingBankDataDTO>();
            List<Expression<Func<BusinessPartnerBank, bool>>> conditions = new List<Expression<Func<BusinessPartnerBank, bool>>>()
            {
                e => e.BusinessPartnerId == room.BusinessPartnerId,
                e => e.DeletedDate == null
            };
            var businessBank = await _unitOfWorkVigo.BusinessPartnerBanks.GetAll(conditions);

            foreach (var item in businessBank) {
                var bank = await _unitOfWorkVigo.Banks.GetById(item.BankId);
                result.Add(new BookingBankDataDTO()
                {
                    BankName = bank.Name,
                    BankNumber = item.BankNumber,
                    Logo = bank.Logo,
                    OwnerName = item.OwnerName,
                    QRURL = $"https://img.vietqr.io/image/{bank.Code}-{item.BankNumber}-compact.png"
                });
            }

            return result;
        }

        public async Task<PagedResultCustom<BookingAppDTO>> GetPaging(int page, int perPage, ClaimsPrincipal user)
        {
            int infoId = int.Parse(user.FindFirst("InfoId")!.Value);
            List<Expression<Func<Booking, bool>>> conditions = new List<Expression<Func<Booking, bool>>>()
            {
                e => e.TouristId == infoId,
                e => e.DeletedDate == null
            };
            var data = await _unitOfWorkVigo.Bookings.GetPaging(conditions,
                                                                null,
                                                                null,
                                                                e => e.CreatedDate,
                                                                page,
                                                                perPage,
                                                                true);
            var books = _mapper.Map<List<BookingAppDTO>>(data.Items);
            foreach (var item in books) {
                var book = _mapper.Map<RoomDetailDTO>(await _unitOfWorkVigo.Rooms.GetById(item.RoomId));
                List<Expression<Func<RoomServiceR, bool>>> con = new List<Expression<Func<RoomServiceR, bool>>>()
                {
                    e => e.RoomId == book.Id
                };
                var roomServices = await _unitOfWorkVigo.RoomServices.GetAll(con);
                foreach (var service in roomServices)
                {
                    book.Services.Add(_mapper.Map<ServiceDTO>(await _unitOfWorkVigo.Services.GetById(service.ServiceId)));
                }
                List<Expression<Func<Image, bool>>> con2 = new List<Expression<Func<Image, bool>>>()
                {
                    e => e.RoomId == book.Id
                };
                var images = await _unitOfWorkVigo.Images.GetAll(con2);
                List<string> type = new List<string>();
                foreach (var image in images)
                {
                    if (type.Contains(image.Type))
                    {
                        book.Images.Where(e => e.Type == image.Type).FirstOrDefault()!.Urls.Add(image.Url);
                    }
                    else
                    {
                        type.Add(image.Type);
                        var temp = new RoomImageDTO()
                        {
                            Type = image.Type
                        };
                        temp.Urls.Add(image.Url);
                        book.Images.Add(temp);
                    }
                }
                book.BusinessPartnerName = (await _unitOfWorkVigo.BusinessPartners.GetById(book.BusinessPartnerId)).Name;
                book.Province = (await _unitOfWorkVigo.Provinces.GetDetailBy(e => e.Id.Equals(book.ProvinceId)))!.Name;
                book.District = (await _unitOfWorkVigo.Districts.GetDetailBy(e => e.Id.Equals(book.DistrictId)))!.Name;
                item.RoomDetail = book;
            }
            return new PagedResultCustom<BookingAppDTO>(books, data.TotalRecords, data.PageIndex, data.PageSize);
        }

        public async Task<decimal> GetPrice(int roomId, int couponId, DateTime checkInDate, DateTime checkOutDate, ClaimsPrincipal user)
        {
            int infoId = int.Parse(user.FindFirst("InfoId")!.Value);
            var room = await _unitOfWorkVigo.Rooms.GetById(roomId);
            var coupon = await _unitOfWorkVigo.DiscountCoupons.GetById(couponId);
            decimal price = room.Price * (checkOutDate-checkInDate).Days;
            if (coupon.UserUsed.Split(",").Contains(infoId.ToString()))
            {
                throw new CustomException("phiếu giảm giá đã được sử dụng");
            }
            if (coupon.DiscountType.ToString().Equals("fixed_amount"))
            {
                price -= coupon.DiscountValue;
            }
            if (coupon.DiscountType.ToString().Equals("percentage"))
            {
                price -= coupon.DiscountValue * price / 100;
            }
            return price;
        }
    }
}
