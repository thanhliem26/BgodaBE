using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.Entity;
using vigo.Domain.Interface.IUnitOfWork;
using vigo.Service.Application.IServiceApp;
using vigo.Service.DTO.Admin.Service;

namespace vigo.Service.Application.ServiceApp
{
    public class ServiceAppService : IServiceAppService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWorkVigo _unitOfWorkVigo;

        public ServiceAppService(IUnitOfWorkVigo unitOfWorkVigo, IMapper mapper)
        {
            _unitOfWorkVigo = unitOfWorkVigo;
            _mapper = mapper;
        }

        public async Task<List<ServiceDTO>> GetAll()
        {
            List<Expression<Func<ServiceR, bool>>> conditions = new List<Expression<Func<ServiceR, bool>>>()
            {
                e => e.DeletedDate == null
            };
            return _mapper.Map<List<ServiceDTO>>(await _unitOfWorkVigo.Services.GetAll(conditions));
        }
    }
}
