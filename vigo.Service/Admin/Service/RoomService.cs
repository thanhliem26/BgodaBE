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
using vigo.Service.DTO.Admin.Room;
using vigo.Service.DTO.Admin.Service;

namespace vigo.Service.Admin.Service
{
    public class RoomService : IRoomService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWorkVigo _unitOfWorkVigo;

        public RoomService(IUnitOfWorkVigo unitOfWorkVigo, IMapper mapper)
        {
            _unitOfWorkVigo = unitOfWorkVigo;
            _mapper = mapper;
        }

        public async Task Create(CreateRoomDTO dto, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            string businessKey = user.FindFirst("BusinessKey")!.Value;
            if (businessKey.IsNullOrEmpty() || !role.Permission.Split(",").Contains("room_manage"))
            {
                throw new CustomException("không có quyền");
            }
            var businessPartner = await _unitOfWorkVigo.BusinessPartners.GetDetailBy(e => e.BusinessKey.Equals(businessKey));
            DateTime dateNow = DateTime.Now;
            Room room = new Room()
            {
                Name = dto.Name,
                Avaiable = dto.Avaiable,
                CreatedDate = dateNow,
                DeletedDate = null,
                Description = dto.Description,
                Price = dto.Price,
                RoomTypeId = dto.RoomTypeId,
                Thumbnail = dto.Thumbnail,
                UpdatedDate = dateNow,
                Address = businessPartner!.Address,
                DistrictId = businessPartner.DistrictId,
                ProvinceId = businessPartner.ProvinceId,
                DefaultDiscount = dto.DefaultDiscount,
                BusinessPartnerId = businessPartner.Id
            };
            _unitOfWorkVigo.Rooms.Create(room);
            await _unitOfWorkVigo.Complete();

            try{
                List<RoomServiceR> roomServices = new List<RoomServiceR>();
                foreach (var item in dto.Services)
                {
                    roomServices.Add(new RoomServiceR()
                    {
                        RoomId = room.Id,
                        ServiceId = item
                    });
                }
                _unitOfWorkVigo.RoomServices.CreateRange(roomServices);
                await _unitOfWorkVigo.Complete();
            }
            catch(CustomException)
            {
                throw new CustomException("không đăng ký được dịch vụ cho phòng");
            }

            try
            {
                List<Image> images = new List<Image>();
                foreach (var item in dto.Images)
                {
                    foreach (var image in item.Urls)
                    {
                        images.Add(new Image()
                        {
                            RoomId = room.Id,
                            Type = item.Type,
                            Url = image
                        });
                    }
                }
                _unitOfWorkVigo.Images.CreateRange(images);
                await _unitOfWorkVigo.Complete();
            }
            catch (CustomException)
            {
                throw new CustomException("không lưu được ảnh cho phòng cho phòng");
            }
        }

        public async Task Delete(int id, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("room_manage"))
            {
                throw new CustomException("không có quyền");
            }
            var data = await _unitOfWorkVigo.Rooms.GetById(id);
            data.DeletedDate = DateTime.Now;
            await _unitOfWorkVigo.Complete();
        }

        public async Task<RoomDetailDTO> GetDetail(int id, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("room_manage"))
            {
                throw new CustomException("không có quyền");
            }
            var room = await _unitOfWorkVigo.Rooms.GetById(id);
            var result = _mapper.Map<RoomDetailDTO>(room);
            result.Province = (await _unitOfWorkVigo.Provinces.GetDetailBy(e => e.Id.Equals(room.ProvinceId)))!.Name;
            result.District = (await _unitOfWorkVigo.Districts.GetDetailBy(e => e.Id.Equals(room.DistrictId)))!.Name;
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
                if (type.Contains(image.Type)) {
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
            var roomType = await _unitOfWorkVigo.RoomTypes.GetById(room.RoomTypeId);
            result.RoomType = roomType.Name;
            return result;
        }

        public async Task<PagedResultCustom<RoomDTO>> GetPaging(int page, int perPage, int? roomTypeId, int? businessPartnerId, string? sortType, string? sortField, string? searchName, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("room_manage"))
            {
                throw new CustomException("không có quyền");
            }
            string userType = user.FindFirst("UserType")!.Value;
            List<Expression<Func<Room, bool>>> conditions = new List<Expression<Func<Room, bool>>>()
            {
                e => e.DeletedDate == null,
            };
            if (businessPartnerId != null)
            {
                conditions.Add(e => e.BusinessPartnerId == businessPartnerId);
            }
            if (roomTypeId != null)
            {
                conditions.Add(e => e.RoomTypeId == roomTypeId);
            }
            if (searchName != null)
            {
                conditions.Add(e => e.Name.ToLower().Contains(searchName.ToLower()));
            }
            if (userType.Equals("BusinessPartner"))
            {
                conditions.Add(e => e.BusinessPartnerId == int.Parse(user.FindFirst("InfoId")!.Value));
            }
            bool sortDown = false;
            if (sortType != null && sortType.Equals("DESC"))
            {
                sortDown = true;
            }
            var data = await _unitOfWorkVigo.Rooms.GetPaging(conditions,
                                                             null,
                                                             sortField != null && sortField.Equals("price") ? e => e.Price : null,
                                                             sortField != null && sortField.Equals("createdDate") ? e => e.CreatedDate : null,
                                                             page,
                                                             perPage,
                                                             sortDown);
            var result = new PagedResultCustom<RoomDTO>(_mapper.Map<List<RoomDTO>>(data.Items), data.TotalRecords, data.PageIndex, data.PageSize);
            foreach (var item in result.Items) {
                List<Expression<Func<RoomServiceR, bool>>> con = new List<Expression<Func<RoomServiceR, bool>>>()
                {
                    e => e.RoomId == item.Id,
                };
                var roomServices = await _unitOfWorkVigo.RoomServices.GetAll(con);
                foreach (var service in roomServices) {
                    item.Services.Add(_mapper.Map<ServiceDTO>(await _unitOfWorkVigo.Services.GetById(service.ServiceId)));
                }
                item.BusinessPartner = _mapper.Map<BusinessPartnerShortDTO>(await _unitOfWorkVigo.BusinessPartners.GetById(item.BusinessPartnerId));
            }
            return result;
        }

        public async Task Update(UpdateRoomDTO dto, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            string businessKey = user.FindFirst("BusinessKey")!.Value;
            if (businessKey.IsNullOrEmpty() || !role.Permission.Split(",").Contains("room_manage"))
            {
                throw new CustomException("không có quyền");
            }
            var businessPartner = await _unitOfWorkVigo.BusinessPartners.GetDetailBy(e => e.BusinessKey.Equals(businessKey));
            var data = await _unitOfWorkVigo.Rooms.GetById(dto.Id);
            if (businessPartner!.Id != data.BusinessPartnerId)
            {
                throw new CustomException("đối tác không có quyền với dữ liệu này");
            }
            data.Thumbnail = dto.Thumbnail;
            data.Price = dto.Price;
            data.UpdatedDate = DateTime.Now;
            data.Avaiable = dto.Avaiable;
            data.Description = dto.Description;
            data.Name = dto.Name;
            data.RoomTypeId = dto.RoomTypeId;
            data.Address = businessPartner.Address;
            data.ProvinceId = businessPartner.ProvinceId;
            data.DistrictId = businessPartner.DistrictId;
            data.DefaultDiscount = dto.DefaultDiscount;

            List<Expression<Func<RoomServiceR, bool>>> con = new List<Expression<Func<RoomServiceR, bool>>>()
            {
                e => e.RoomId == dto.Id,
            };

            var roomService = await _unitOfWorkVigo.RoomServices.GetAll(con);
            foreach (var r in roomService) {
                if (!dto.Services.Contains(r.ServiceId))
                {
                    _unitOfWorkVigo.RoomServices.Delete(r);
                }
                else
                {
                    dto.Services.Remove(r.ServiceId);
                }
            }
            foreach (var item in dto.Services) {
                _unitOfWorkVigo.RoomServices.Create(new RoomServiceR()
                {
                    RoomId = dto.Id,
                    ServiceId = item
                });
            }
            await _unitOfWorkVigo.Complete();

            try
            {
                List<Image> images = new List<Image>();
                foreach (var item in dto.Images)
                {
                    foreach (var image in item.Urls)
                    {
                        images.Add(new Image()
                        {
                            RoomId = data.Id,
                            Type = item.Type,
                            Url = image
                        });
                    }
                }
                _unitOfWorkVigo.Images.CreateRange(images);
                await _unitOfWorkVigo.Complete();
            }
            catch (CustomException)
            {
                throw new CustomException("không lưu được ảnh cho phòng cho phòng");
            }
        }
    }
}
