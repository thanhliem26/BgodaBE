using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.AccountFolder;
using vigo.Domain.Helper;
using vigo.Domain.Interface.IUnitOfWork;
using vigo.Domain.User;
using vigo.Service.Admin.IService;
using vigo.Service.DTO.Admin.Account;
using vigo.Service.DTO.Admin.Role;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace vigo.Service.Admin.Service
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWorkVigo _unitOfWorkVigo;
        public RoleService(IMapper mapper, IUnitOfWorkVigo unitOfWorkVigo)
        {
            _mapper = mapper;
            _unitOfWorkVigo = unitOfWorkVigo;
        }
        public async Task Create(RoleCreateDTO dto, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("role_manage"))
            {
                throw new CustomException("không có quyền");
            }
            var checkUnique = await _unitOfWorkVigo.Roles.GetDetailBy(e => e.Name.Equals(dto.Name));
            if (checkUnique != null) {
                throw new CustomException("quyền đã tồn tại");
            }
            var data = new Role()
            {
                Name = dto.Name,
                Permission = dto.Permission != null ? string.Join(",",dto.Permission) : "",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                DeletedDate = null
            };
            _unitOfWorkVigo.Roles.Create(data);
            await _unitOfWorkVigo.Complete();
        }

        public async Task Delete(int id, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("role_manage"))
            {
                throw new CustomException("không có quyền");
            }
            var data = await _unitOfWorkVigo.Roles.GetById(id);
            data.DeletedDate = DateTime.Now;
            await _unitOfWorkVigo.Complete();
        }

        public async Task<List<RoleDTO>> GetAll(ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("role_manage"))
            {
                throw new CustomException("không có quyền");
            }
            List<Expression<Func<Role, bool>>> conditions = new List<Expression<Func<Role, bool>>>()
            {
                e => e.DeletedDate == null,
                e => e.Id != 1
            };
            return _mapper.Map<List<RoleDTO>>(await _unitOfWorkVigo.Roles.GetAll(conditions));
        }

        public async Task<RoleDetailDTO> GetDetail(int id, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("role_manage"))
            {
                throw new CustomException("không có quyền");
            }
            return _mapper.Map<RoleDetailDTO>(await _unitOfWorkVigo.Roles.GetById(id));
        }

        public async Task<PagedResultCustom<RoleDTO>> GetPaging(int page, int perPage, string? sortType, string? sortField, string? searchName, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("role_manage"))
            {
                throw new CustomException("không có quyền");
            }
            List<Expression<Func<Role, bool>>> conditions = new List<Expression<Func<Role, bool>>>()
            {
                e => e.DeletedDate == null,
                e => e.Id != 1
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
            var data = await _unitOfWorkVigo.Roles.GetPaging(conditions,
                                                             null,
                                                             null,
                                                             sortField != null && sortField.Equals("createdDate") ? e => e.CreatedDate : null,
                                                             page,
                                                             perPage,
                                                             sortDown);
            return new PagedResultCustom<RoleDTO>(_mapper.Map<List<RoleDTO>>(data.Items), data.TotalRecords, data.PageIndex, data.PageSize);
        }
        public async Task<List<RolePermissionDTO>> GetPermission(ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("role_manage"))
            {
                throw new CustomException("không có quyền");
            }
            return _mapper.Map<List<RolePermissionDTO>>(await _unitOfWorkVigo.RolePermissions.GetAll(null));
        }

        public async Task Update(RoleUpdateDTO dto, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("role_manage"))
            {
                throw new CustomException("không có quyền");
            }
            var data = await _unitOfWorkVigo.Roles.GetById(dto.Id);
            var checkUnique = await _unitOfWorkVigo.Roles.GetDetailBy(e => e.Name.Equals(dto.Name));
            if (dto.Name != data!.Name && checkUnique != null)
            {
                throw new CustomException("tên bị trùng lặp");
            }
            data.Name = dto.Name;
            data.Permission = dto.Permission != null ? string.Join(",", dto.Permission) : "";
            data.UpdatedDate = DateTime.Now;
            await _unitOfWorkVigo.Complete();
        }
    }
}
