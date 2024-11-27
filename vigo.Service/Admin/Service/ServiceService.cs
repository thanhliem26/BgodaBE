using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
using vigo.Service.DTO.Admin.Service;

namespace vigo.Service.Admin.Service
{
    public class ServiceService : IServiceService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWorkVigo _unitOfWorkVigo;

        public ServiceService(IUnitOfWorkVigo unitOfWorkVigo, IMapper mapper)
        {
            _unitOfWorkVigo = unitOfWorkVigo;
            _mapper = mapper;
        }

        public async Task Create(ServiceCreateDTO dto, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            string businessKey = user.FindFirst("RoleId")!.Value;
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("service_manage"))
            {
                throw new CustomException("không có quyền");
            }
            var checkUnique = await _unitOfWorkVigo.Services.GetDetailBy(e => e.Name == dto.Name);
            if (checkUnique != null)
            {
                throw new CustomException("dịch vụ đã tồn tại");
            }
            DateTime DateNow = DateTime.Now;
            ServiceR data = new ServiceR()
            {
                CreatedDate = DateNow,
                UpdatedDate = DateNow,
                DeletedDate = null,
                Name = dto.Name,
                Description = dto.Description,
                Icon = dto.Icon,
            };
            _unitOfWorkVigo.Services.Create(data);
            await _unitOfWorkVigo.Complete();
        }

        public async Task Delete(int id, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("service_manage"))
            {
                throw new CustomException("không có quyền");
            }
            var data = await _unitOfWorkVigo.Services.GetById(id);
            data.DeletedDate = DateTime.Now;
            await _unitOfWorkVigo.Complete();
        }

        public async Task<List<ServiceDTO>> GetAll(ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("service_manage"))
            {
                throw new CustomException("không có quyền");
            }
            List<Expression<Func<ServiceR, bool>>> conditions = new List<Expression<Func<ServiceR, bool>>>()
            {
                e => e.DeletedDate == null
            };
            return _mapper.Map<List<ServiceDTO>>(await _unitOfWorkVigo.Services.GetAll(conditions));
        }

        public async Task<ServiceDTO> GetDetail(int id, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("service_manage"))
            {
                throw new CustomException("không có quyền");
            }
            return _mapper.Map<ServiceDTO>(await _unitOfWorkVigo.Services.GetById(id));
        }

        public async Task<PagedResultCustom<ServiceDTO>> GetPaging(int page, int perPage, string? sortType, string? sortField, string? searchName, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("service_manage"))
            {
                throw new CustomException("không có quyền");
            }
            List<Expression<Func<ServiceR, bool>>> conditions = new List<Expression<Func<ServiceR, bool>>>()
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
            var data = await _unitOfWorkVigo.Services.GetPaging(conditions,
                                                                 null,
                                                                 null,
                                                                 sortField != null && sortField.Equals("createdDate") ? e => e.CreatedDate : null,
                                                                 page,
                                                                 perPage,
                                                                 sortDown);
            return new PagedResultCustom<ServiceDTO>(_mapper.Map<List<ServiceDTO>>(data.Items), data.TotalRecords, data.PageIndex, data.PageSize);
        }

        public async Task Update(ServiceUpdateDTO dto, ClaimsPrincipal user)
        {
            int roleId = int.Parse(user.FindFirst("RoleId")!.Value);
            var role = await _unitOfWorkVigo.Roles.GetById(roleId);
            if (!role.Permission.Split(",").Contains("service_manage"))
            {
                throw new CustomException("không có quyền");
            }
            var service = await _unitOfWorkVigo.Services.GetDetailBy(e => e.Id == dto.Id);
            var checkUnique = await _unitOfWorkVigo.Services.GetDetailBy(e => e.Name == dto.Name);
            if (dto.Name != service!.Name && checkUnique != null)
            {
                throw new CustomException("dịch vụ đã tồn tại");
            }
            var data = await _unitOfWorkVigo.Services.GetById(dto.Id);
            data.UpdatedDate = DateTime.Now;
            data.Name = dto.Name;
            data.Description = dto.Description;
            data.Icon = dto.Icon;
            await _unitOfWorkVigo.Complete();
        }
    }
}
