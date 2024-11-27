using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
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
using vigo.Service.DTO.Application.Account;
using vigo.Service.DTO.Application.Room;

namespace vigo.Service.Application.ServiceApp
{
    public class RoomAppService : IRoomAppService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWorkVigo _unitOfWorkVigo;

        public RoomAppService(IUnitOfWorkVigo unitOfWorkVigo, IMapper mapper)
        {
            _unitOfWorkVigo = unitOfWorkVigo;
            _mapper = mapper;
        }

        public async Task<RoomDetailDTO> GetDetail(int id)
        {
            var result = _mapper.Map<RoomDetailDTO>(await _unitOfWorkVigo.Rooms.GetById(id));
            List<Expression<Func<RoomServiceR, bool>>> con = new List<Expression<Func<RoomServiceR, bool>>>()
            {
                e => e.RoomId == result.Id
            };
            var roomServices = await _unitOfWorkVigo.RoomServices.GetAll(con);
            foreach (var service in roomServices)
            {
                result.Services.Add(_mapper.Map<ServiceDTO>(await _unitOfWorkVigo.Services.GetById(service.ServiceId)));
            }
            List<Expression<Func<Image, bool>>> con2 = new List<Expression<Func<Image, bool>>>()
            {
                e => e.RoomId == result.Id
            };
            var images = await _unitOfWorkVigo.Images.GetAll(con2);
            List<string> type = new List<string>();
            foreach (var image in images)
            {
                if (type.Contains(image.Type))
                {
                    result.Images.Where(e => e.Type == image.Type).FirstOrDefault()!.Urls.Add(image.Url);
                }
                else
                {
                    type.Add(image.Type);
                    var temp = new RoomImageDTO()
                    {
                        Type = image.Type
                    };
                    temp.Urls.Add(image.Url);
                    result.Images.Add(temp);
                }
            }
            result.BusinessPartnerName = (await _unitOfWorkVigo.BusinessPartners.GetById(result.BusinessPartnerId)).Name;
            result.Province = (await _unitOfWorkVigo.Provinces.GetDetailBy(e => e.Id.Equals(result.ProvinceId)))!.Name;
            result.District = (await _unitOfWorkVigo.Districts.GetDetailBy(e => e.Id.Equals(result.DistrictId)))!.Name;
            return result;
        }

        public async Task<ProvinceV2DTO> GetPaging(GetRoomDTO dto)
        {
            List<Expression<Func<Room, bool>>> conditions = new List<Expression<Func<Room, bool>>>()
            {
                e => e.ProvinceId.Equals(dto.ProvinceId)
            };
            if (dto.RoomTypeId != null)
            {
                conditions.Add(e => e.RoomTypeId == dto.RoomTypeId);
            }
            var room = await _unitOfWorkVigo.Rooms.GetAll(conditions);
            var roomResult = new List<Room>();

            if (dto.Stars != null)
            {
                foreach (var item in dto.Stars)
                {
                    roomResult.AddRange(room.Where(e => Math.Floor(e.Star + 0.5m) == item));
                }
            }
            else
            {
                roomResult = room.ToList();
            }

            if (dto.DistrictIds != null)
            {
                for (int i = roomResult.Count - 1; i >= 0; i--)
                {
                    var item = roomResult[i];
                    if (!dto.DistrictIds.Contains(item.DistrictId))
                    {
                        roomResult.RemoveAt(i);
                    }
                }
            }

            for (int i = roomResult.Count - 1; i >= 0; i--)
            {
                var item = roomResult[i];
                if (item.Price < dto.MinPrice || item.Price > dto.MaxPrice)
                {
                    roomResult.RemoveAt(i);
                }
            }

            if (dto.Services != null)
            {
                List<int> roomIds = new List<int>();
                foreach (var item in dto.Services)
                {
                    List<Expression<Func<RoomServiceR, bool>>> serviceCon = new List<Expression<Func<RoomServiceR, bool>>>()
                    {
                        e => e.ServiceId == item
                    };
                    roomIds.AddRange((await _unitOfWorkVigo.RoomServices.GetAll(serviceCon)).Select(e => e.RoomId));
                }
                for (int i = roomResult.Count - 1; i >= 0; i--)
                {
                    var item = roomResult[i];
                    if (!roomIds.Contains(item.Id))
                    {
                        roomResult.RemoveAt(i);
                    }
                }
            }

            for (int i = roomResult.Count - 1; i >= 0; i--)
            {
                var roomTempResult = roomResult[i];
                DateTime DateNow = DateTime.Now;
                List<Expression<Func<Booking, bool>>> bookCon = new List<Expression<Func<Booking, bool>>>()
                                {
                                    e => e.RoomId == roomTempResult.Id,
                                    e => e.CheckOutDate > DateNow,
                                    e => e.CheckOutDate < dto.CheckIn
                                };
                var booking = await _unitOfWorkVigo.Bookings.GetAll(bookCon);
                if (!(booking.Count() > 0 - roomTempResult.Avaiable))
                {
                    roomResult.RemoveAt(i);
                }
            }
            
            var tempRoomCount = roomResult.Count();
            roomResult = roomResult.Skip((dto.Page - 1)* dto.PerPage).Take(dto.PerPage).ToList();
            var province = await _unitOfWorkVigo.Provinces.GetDetailBy(e => e.Id.Equals(dto.ProvinceId));
            List<RoomAppDTO> temp = new List<RoomAppDTO>();
            foreach (var item in roomResult)
            {
                List<Expression<Func<Image, bool>>> imageCon = new List<Expression<Func<Image, bool>>>()
                {
                    e => e.RoomId == item.Id
                };
                var imageTemp = await _unitOfWorkVigo.Images.GetPaging(imageCon, null, null, null, 1, 8);
                temp.Add(new RoomAppDTO()
                {
                    ProvinceId = item.ProvinceId,
                    DistrictId = item.DistrictId,
                    Address = item.Address,
                    Avaiable = item.Avaiable,
                    DefaultDiscount = item.DefaultDiscount,
                    Description = item.Description,
                    District = (await _unitOfWorkVigo.Districts.GetDetailBy(e => e.Id.Equals(item.DistrictId)))!.Name,
                    Name = item.Name,
                    Id = item.Id,
                    Images = imageTemp.Items.Select(e => e.Url).ToList(),
                    Price = item.Price,
                    Province = (await _unitOfWorkVigo.Provinces.GetDetailBy(e => e.Id.Equals(item.ProvinceId)))!.Name,
                    Star = item.Star,
                    Thumbnail = item.Thumbnail
                });
            }
            return new ProvinceV2DTO()
            {
                Id = province!.Id,
                Image = province.Image,
                Name = province.Name,
                RoomNumber = tempRoomCount,
                Rooms = temp
            };
        }

        public async Task<PriceRangeDTO> GetPriceRange()
        {
            List<Expression<Func<Room, bool>>> conditions = new List<Expression<Func<Room, bool>>>()
            {
                e => e.DeletedDate == null
            };
            var prices = (await _unitOfWorkVigo.Rooms.GetAll(conditions)).Select(e => e.Price);
            return new PriceRangeDTO()
            {
                Max = prices.Max(),
                Min = prices.Min()
            };
        }

        public async Task<List<RoomAppDTO>> GetRelatedRoom(int businessPartnerId)
        {
            List<Expression<Func<Room, bool>>> conditions = new List<Expression<Func<Room, bool>>>()
            {
                e => e.BusinessPartnerId == businessPartnerId,
                e => e.DeletedDate == null
            };
            var rooms = await _unitOfWorkVigo.Rooms.GetPaging(conditions, null, e => e.BookNumber, null, 1, 8, true);
            List <RoomAppDTO> temp = new List<RoomAppDTO>();
            foreach (var room in rooms.Items)
            {
                List<Expression<Func<Image, bool>>> imageCon = new List<Expression<Func<Image, bool>>>()
                {
                    e => e.RoomId == room.Id
                };
                var imageTemp = await _unitOfWorkVigo.Images.GetPaging(imageCon, null, null, null, 1, 8);
                temp.Add(new RoomAppDTO()
                {
                    Address = room.Address,
                    Avaiable = room.Avaiable,
                    DefaultDiscount = room.DefaultDiscount,
                    Description = room.Description,
                    District = (await _unitOfWorkVigo.Districts.GetDetailBy(e => e.Id.Equals(room.DistrictId)))!.Name,
                    Name = room.Name,
                    Id = room.Id,
                    Price = room.Price,
                    Images = imageTemp.Items.Select(e => e.Url).ToList(),
                    DistrictId = room.DistrictId,
                    ProvinceId = room.ProvinceId,
                    Province = (await _unitOfWorkVigo.Provinces.GetDetailBy(e => e.Id.Equals(room.ProvinceId)))!.Name,
                    Star = room.Star,
                    Thumbnail = room.Thumbnail
                });
            }
            return temp;
        }
    }
}
