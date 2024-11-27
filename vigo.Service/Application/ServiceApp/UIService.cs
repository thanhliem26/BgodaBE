using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.Entity;
using vigo.Domain.Interface.IUnitOfWork;
using vigo.Domain.User;
using vigo.Service.Application.IServiceApp;
using vigo.Service.DTO.Admin.Account;
using vigo.Service.DTO.Application.Room;
using vigo.Service.DTO.Application.Search;
using vigo.Service.DTO.Application.UI;

namespace vigo.Service.Application.ServiceApp
{
    public class UIService : IUIService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWorkVigo _unitOfWorkVigo;

        public UIService(IUnitOfWorkVigo unitOfWorkVigo, IMapper mapper)
        {
            _unitOfWorkVigo = unitOfWorkVigo;
            _mapper = mapper;
        }

        public async Task<List<BusinessPartnerShortDTO>> GetAllBusinessPartnerShort()
        {
            List<Expression<Func<BusinessPartner, bool>>> conditions = new List<Expression<Func<BusinessPartner, bool>>>()
            {
                e => e.DeletedDate == null
            };
            return _mapper.Map<List<BusinessPartnerShortDTO>>((await _unitOfWorkVigo.BusinessPartners.GetAll(conditions)).Take(12));
        }

        public async Task<List<RecommendPlaceDTO>> GetPopularVisit()
        {
            List<string> provinceIds = new List<string>()
            {
                "79","48","01","77","92"
            };
            List<RecommendPlaceDTO> result = new List<RecommendPlaceDTO>();
            foreach (string provinceId in provinceIds)
            {
                RecommendPlaceDTO temp = new RecommendPlaceDTO();
                var province = await _unitOfWorkVigo.Provinces.GetDetailBy(e => e.Id.Equals(provinceId));
                temp.Name = province!.Name;
                List<Expression<Func<Room, bool>>> conditions = new List<Expression<Func<Room, bool>>>()
                {
                    e => e.DeletedDate == null,
                    e => e.ProvinceId.Equals(provinceId),
                };
                var ttt = (await _unitOfWorkVigo.Rooms.GetPaging(conditions, null, e => e.Star, null, 1, 4)).Items;
                List<RoomAppDTO> ttemp = new List<RoomAppDTO>();
                foreach (var aa in ttt)
                {
                    ttemp.Add(new RoomAppDTO()
                    {
                        ProvinceId = aa.ProvinceId,
                        DistrictId = aa.DistrictId,
                        Address = aa.Address,
                        Avaiable = aa.Avaiable,
                        DefaultDiscount = aa.DefaultDiscount,
                        Description = aa.Description,
                        District = (await _unitOfWorkVigo.Districts.GetDetailBy(e => e.Id.Equals(aa.DistrictId)))!.Name,
                        Name = aa.Name,
                        Id = aa.Id,
                        Price = aa.Price,
                        Province = (await _unitOfWorkVigo.Provinces.GetDetailBy(e => e.Id.Equals(aa.ProvinceId)))!.Name,
                        Star = aa.Star,
                        Thumbnail = aa.Thumbnail
                    });
                }
                temp.Rooms = ttemp;
                result.Add(temp);
            }
            return result;
        }

        public async Task<List<VisitProvinceDTO>> GetVisitProvince()
        {
            var data = await _unitOfWorkVigo.Provinces.GetAll(null);
            List<VisitProvinceDTO> result = new List<VisitProvinceDTO>();
            foreach (var province in data) {
                List<Expression<Func<Room, bool>>> conditions = new List<Expression<Func<Room, bool>>>()
                {
                    e => e.ProvinceId.Equals(province.Id)
                };
                result.Add(new VisitProvinceDTO()
                {
                    Id = province.Id,
                    Name = province.Name,
                    Image = province.Image,
                    RoomNumber = (await _unitOfWorkVigo.Rooms.GetAll(conditions)).Count()
                });
            }
            result = result.OrderByDescending(e => e.RoomNumber).ToList();
            return result;
        }
    }
}
