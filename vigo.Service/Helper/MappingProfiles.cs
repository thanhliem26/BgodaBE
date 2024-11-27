using AutoMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using vigo.Domain.AccountFolder;
using vigo.Domain.Entity;
using vigo.Service.DTO.Admin.Role;
using vigo.Service.DTO.Admin.Account;
using vigo.Domain.User;
using vigo.Service.DTO.Admin.Booking;
using vigo.Service.DTO.Admin.Room;
using vigo.Service.DTO.Admin.Discount;
using vigo.Service.DTO.Admin.Rating;
using vigo.Service.DTO.Admin.Service;
using vigo.Service.DTO.Application.Booking;
using vigo.Service.DTO.Application.Discount;
using vigo.Service.DTO.Application.Rating;
using vigo.Service.DTO.Application.UI;
using vigo.Service.DTO.Shared;
using vigo.Service.DTO.Application.Room;

namespace vigo.Service.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Role, RoleDTO>()
            .ForMember(dest => dest.Permission, opt => opt.MapFrom(src => SplitPermissions(src.Permission)));
            CreateMap<Role, RoleDetailDTO>()
            .ForMember(dest => dest.Permission, opt => opt.MapFrom(src => SplitPermissions(src.Permission)));
            CreateMap<RolePermission, RolePermissionDTO>();
            CreateMap<BusinessPartner, BusinessPartnerDTO>();
            CreateMap<SystemEmployee, EmployeeDTO>().ForMember(dest => dest.Name, opt => opt.MapFrom(e => e.FullName));
            CreateMap<Booking, BookingDetailDTO>();
            CreateMap<Tourist, TouristDetailDTO>().ForMember(dest => dest.Name, opt => opt.MapFrom(e => e.FullName));
            CreateMap<Room, RoomDetailDTO>();
            CreateMap<Booking, BookingDTO>();
            CreateMap<DiscountCoupon, DiscountCouponDetailDTO>();
            CreateMap<DiscountCoupon, DiscountCouponDTO>();
            CreateMap<Rating, RatingDTO>().ForMember(dest => dest.Rate, opt => opt.MapFrom(e => e.Star));
            CreateMap<Rating, RoomRatingDTO>().ForMember(dest => dest.Rate, opt => opt.MapFrom(e => e.Star));
            CreateMap<RoomType, RoomTypeDTO>();
            CreateMap<Room, RoomDetailDTO > ();
            CreateMap<Room, RoomDTO> ();
            CreateMap<Room, RoomAppDTO>();
            CreateMap<RoomType, RoomTypeDTO>();
            CreateMap<ServiceR, ServiceDTO>();
            CreateMap<BusinessPartner, BusinessPartnerShortDTO>();
            CreateMap<Tourist, TouristDTO>().ForMember(dest => dest.Name, opt => opt.MapFrom(e => e.FullName));
            CreateMap<Booking, BookingAppDTO>();
            CreateMap<DiscountCoupon, DiscountCouponAppDTO>();
            CreateMap<Province, VisitProvinceDTO>();
            CreateMap<Bank, BankDTO>();
            CreateMap<Room, RoomShortDTO>();
        }
        public static List<string> SplitPermissions(string permissions)
        {
            return permissions?.Split(',').ToList() ?? new List<string>();
        }
    }
}
