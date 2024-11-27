using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.AccountFolder;
using vigo.Domain.Entity;
using vigo.Domain.Helper;
using vigo.Domain.Interface.IUnitOfWork;
using vigo.Service.Admin.IService;
using vigo.Service.DTO.Admin.Room;

namespace vigo.Service.Admin.Service
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWorkVigo _unitOfWorkVigo;

        public RoomTypeService(IUnitOfWorkVigo unitOfWorkVigo, IMapper mapper)
        {
            _unitOfWorkVigo = unitOfWorkVigo;
            _mapper = mapper;
        }

        public async Task Create(RoomTypeCreateDTO dto, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("room_manage"))
            {
                throw new CustomException("không có quyền");
            }
            DateTime DateNow = DateTime.Now;
            RoomType data = new RoomType()
            {
                Name = dto.Name,
                Description = dto.Description,
                CreatedDate = DateNow,
                DeletedDate = null,
                UpdatedDate = DateNow
            };
            _unitOfWorkVigo.RoomTypes.Create(data);
            await _unitOfWorkVigo.Complete();
        }

        public async Task Delete(int id, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("room_manage"))
            {
                throw new CustomException("không có quyền");
            }
            var data = await _unitOfWorkVigo.RoomTypes.GetById(id);
            data.DeletedDate = DateTime.Now;
            await _unitOfWorkVigo.Complete();
        }

        public async Task<List<RoomTypeDTO>> GetAll(ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("room_manage"))
            {
                throw new CustomException("không có quyền");
            }
            List<Expression<Func<RoomType, bool>>> conditions = new List<Expression<Func<RoomType, bool>>>()
            {
                e => e.DeletedDate == null
            };
            return _mapper.Map<List<RoomTypeDTO>>(await _unitOfWorkVigo.RoomTypes.GetAll(conditions));
        }

        public async Task<RoomTypeDTO> GetDetail(int id, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("room_manage"))
            {
                throw new CustomException("không có quyền");
            }
            return _mapper.Map<RoomTypeDTO>(await _unitOfWorkVigo.RoomTypes.GetById(id));
        }

        public async Task<PagedResultCustom<RoomTypeDTO>> GetPaging(int page, int perPage, string? sortType, string? sortField, string? searchName, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("room_manage"))
            {
                throw new CustomException("không có quyền");
            }
            List<Expression<Func<RoomType, bool>>> conditions = new List<Expression<Func<RoomType, bool>>>()
            {
                e => e.DeletedDate == null
            };
            if (searchName != null)
            {
                conditions.Add(e => e.Name.ToLower().Contains(searchName.ToLower()));
            }
            bool sortDown = false;
            if (sortType != null && sortType.Equals("DESC"))
            {
                sortDown = true;
            }
            var data = await _unitOfWorkVigo.RoomTypes.GetPaging(conditions,
                                                                 null,
                                                                 null,
                                                                 sortField != null && sortField.Equals("createdDate") ? e => e.CreatedDate : null,
                                                                 page,
                                                                 perPage,
                                                                 sortDown);
            return new PagedResultCustom<RoomTypeDTO>(_mapper.Map<List<RoomTypeDTO>>(data.Items), data.TotalRecords, data.PageIndex, data.PageSize);
        }

        public async Task Update(RoomTypeUpdateDTO dto, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("room_manage"))
            {
                throw new CustomException("không có quyền");
            }
            var data = await _unitOfWorkVigo.RoomTypes.GetById(dto.Id);
            data.UpdatedDate = DateTime.Now;
            data.Name = dto.Name;
            data.Description = dto.Description;
            await _unitOfWorkVigo.Complete();
        }
    }
}
