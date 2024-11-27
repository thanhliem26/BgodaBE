using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.Entity;
using vigo.Domain.Helper;
using vigo.Domain.Interface.IUnitOfWork;
using vigo.Domain.User;
using vigo.Service.Application.IServiceApp;
using vigo.Service.DTO.Admin.Bank;
using vigo.Service.DTO.Application.Account;
using vigo.Service.DTO.Application.Room;
using vigo.Service.DTO.Application.Search;
using vigo.Service.EmailAuthenModule;

namespace vigo.Service.Application.ServiceApp
{
    public class SearchAppService : ISearchAppService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWorkVigo _unitOfWorkVigo;

        public SearchAppService(IUnitOfWorkVigo unitOfWorkVigo, IMapper mapper)
        {
            _unitOfWorkVigo = unitOfWorkVigo;
            _mapper = mapper;
        }

        public async Task<PagedResultCustom<RoomAppDTO>> ReturnSearchResult(GetRoomSearchDTO dto)
        {
            var conditions = dto.SearchInput.IsNullOrEmpty() ? null : new List<Expression<Func<Province, bool>>>()
            {
                e => e.Name.ToLower().Contains(dto.SearchInput!.ToLower())
            };
            var provinces = await _unitOfWorkVigo.Provinces.GetAll(conditions);
            if (provinces.Count() != 0) {
                PagedResultCustom<RoomAppDTO> result = new PagedResultCustom<RoomAppDTO>(new List<RoomAppDTO>(),0,dto.Page,dto.PerPage);
                int totalRecords = 0;
                foreach (var province in provinces)
                {
                    List<Expression<Func<Room, bool>>> con2 = new List<Expression<Func<Room, bool>>>()
                    {
                        e => e.ProvinceId.Equals(province.Id)
                    };
                    var rooms = await _unitOfWorkVigo.Rooms.GetAll(con2);
                    var roomFilter = new List<Room>();
                    var roomResult = new List<Room>();
                    foreach (var item in rooms)
                    {
                        if (item.Avaiable > 0)
                        {
                            roomFilter.Add(item);
                        }
                        else
                        {
                            DateTime DateNow = DateTime.Now;
                            List<Expression<Func<Booking, bool>>> bookCon = new List<Expression<Func<Booking, bool>>>()
                                {
                                    e => e.RoomId == item.Id,
                                    e => e.CheckOutDate > DateNow,
                                    e => e.CheckOutDate < dto.CheckIn
                                };
                            var booking = await _unitOfWorkVigo.Bookings.GetAll(bookCon);
                            if (booking.Count() > 0 - item.Avaiable)
                            {
                                roomFilter.Add(item);
                            }
                        }
                    }

                    for (int i = roomFilter.Count - 1; i >= 0; i--)
                    {
                        var item = roomFilter[i];
                        if (item.Price < dto.MinPrice || item.Price > dto.MaxPrice)
                        {
                            roomFilter.RemoveAt(i);
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
                        for (int i = roomFilter.Count - 1; i >= 0; i--)
                        {
                            var item = roomFilter[i];
                            if (!roomIds.Contains(item.Id))
                            {
                                roomFilter.RemoveAt(i);
                            }
                        }
                    }

                    if (dto.Stars != null)
                    {
                        foreach (var star in dto.Stars)
                        {
                            roomResult.AddRange(roomFilter.Where(e => Math.Floor(e.Star + 0.5m) == star));
                        }
                    }
                    else
                    {
                        roomResult = roomFilter.ToList();
                    }
                    totalRecords += roomResult.Count();
                    roomResult = roomResult.Skip((dto.Page-1)*dto.PerPage).Take(dto.PerPage).ToList();
                    foreach (var item in roomResult)
                    {
                        List<Expression<Func<Image, bool>>> imageCon = new List<Expression<Func<Image, bool>>>()
                        {
                            e => e.RoomId == item.Id
                        };
                        var imageTemp = await _unitOfWorkVigo.Images.GetPaging(imageCon, null, null, null, 1, 8);
                        result.Items.Add(new RoomAppDTO()
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
                }
                result.TotalRecords = totalRecords;
                return result;
            }
            else
            {
                PagedResultCustom<RoomAppDTO> result = new PagedResultCustom<RoomAppDTO>(new List<RoomAppDTO>(), 0, dto.Page, dto.PerPage);
                int totalRecords = 0;
                List<Expression<Func<BusinessPartner, bool>>> con2 = new List<Expression<Func<BusinessPartner, bool>>>()
                {
                    e => e.Name.ToLower().Contains(dto.SearchInput!.ToLower())
                };
                var business = await _unitOfWorkVigo.BusinessPartners.GetAll(con2);
                foreach (var businessPartner in business) {
                    List<Expression<Func<Room, bool>>> conRoom = new List<Expression<Func<Room, bool>>>()
                    {
                        e => e.BusinessPartnerId == businessPartner.Id
                    };
                    var rooms = await _unitOfWorkVigo.Rooms.GetAll(conRoom);
                    var roomFilter = new List<Room>();
                    var roomResult = new List<Room>();
                    foreach (var item in rooms)
                    {
                        if (item.Avaiable > 0)
                        {
                            roomFilter.Add(item);
                        }
                        else
                        {
                            DateTime DateNow = DateTime.Now;
                            List<Expression<Func<Booking, bool>>> bookCon = new List<Expression<Func<Booking, bool>>>()
                                {
                                    e => e.RoomId == item.Id,
                                    e => e.CheckOutDate > DateNow,
                                    e => e.CheckOutDate < dto.CheckIn
                                };
                            var booking = await _unitOfWorkVigo.Bookings.GetAll(bookCon);
                            if (booking.Count() > 0 - item.Avaiable)
                            {
                                roomFilter.Add(item);
                            }
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
                        for (int i = roomFilter.Count - 1; i >= 0; i--)
                        {
                            var item = roomFilter[i];
                            if (!roomIds.Contains(item.Id))
                            {
                                roomFilter.RemoveAt(i);
                            }
                        }
                    }

                    if (dto.Stars != null)
                    {
                        foreach (var star in dto.Stars)
                        {
                            roomResult.AddRange(roomFilter.Where(e => Math.Floor(e.Star + 0.5m) == star));
                        }
                    }
                    else
                    {
                        roomResult = roomFilter.ToList();
                    }
                    totalRecords += roomResult.Count();
                    roomResult = roomResult.Skip((dto.Page - 1) * dto.PerPage).Take(dto.PerPage).ToList();
                    foreach (var item in roomResult)
                    {
                        List<Expression<Func<Image, bool>>> imageCon = new List<Expression<Func<Image, bool>>>()
                        {
                            e => e.RoomId == item.Id
                        };
                        var imageTemp = await _unitOfWorkVigo.Images.GetPaging(imageCon, null, null, null, 1, 8);
                        result.Items.Add(new RoomAppDTO()
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
                }
                result.TotalRecords = totalRecords;
                return result;
            }
        }

        public async Task<SearchResultDTO> ReturnSearchTyping(string? searchInput)
        {
            if (!searchInput.IsNullOrEmpty())
            {
                List<Expression<Func<Province, bool>>> conditions = new List<Expression<Func<Province, bool>>>()
                {
                    e => e.Name.ToLower().Contains(searchInput!.ToLower())
                };
                List<Expression<Func<BusinessPartner, bool>>> conditions2 = new List<Expression<Func<BusinessPartner, bool>>>()
                {
                    e => e.Name.ToLower().Contains(searchInput!.ToLower())
                };
                var province = await _unitOfWorkVigo.Provinces.GetAll(conditions);
                var business = await _unitOfWorkVigo.BusinessPartners.GetAll(conditions2);
                var result = new SearchResultDTO();
                foreach (var item in province)
                {
                    List<Expression<Func<Room, bool>>> con = new List<Expression<Func<Room, bool>>>()
                    {
                        e => e.ProvinceId.Equals(item.Id)
                    };
                    result.ProvinceShortDTOs.Add(new ProvinceShortDTO()
                    {
                        Name = item.Name,
                        Image = item.Image,
                        RoomNumber = (await _unitOfWorkVigo.Rooms.GetAll(con)).Count()
                    });
                }
                foreach (var item in business)
                {
                    List<Expression<Func<Room, bool>>> con = new List<Expression<Func<Room, bool>>>()
                    {
                        e => e.BusinessPartnerId == item.Id
                    };
                    result.BPShortDTOs.Add(new BPShortDTO()
                    {
                        Name = item.Name,
                        Logo = item.Logo,
                        RoomNumber = (await _unitOfWorkVigo.Rooms.GetAll(con)).Count()
                    });
                }
                return result;
            }
            else
            {
                var result = new SearchResultDTO();
                var province = await _unitOfWorkVigo.Provinces.GetAll(null);
                foreach (var item in province)
                {
                    List<Expression<Func<Room, bool>>> con = new List<Expression<Func<Room, bool>>>()
                    {
                        e => e.ProvinceId.Equals(item.Id)
                    };
                    result.ProvinceShortDTOs.Add(new ProvinceShortDTO()
                    {
                        Name = item.Name,
                        Image = item.Image,
                        RoomNumber = (await _unitOfWorkVigo.Rooms.GetAll(con)).Count()
                    });
                }
                return result;
            }
        }
    }
}
