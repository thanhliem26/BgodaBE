using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.Interface.IRepository;

namespace vigo.Domain.Interface.IUnitOfWork
{
    public interface IUnitOfWorkVigo
    {
        IAccountRepository Accounts { get; }
        IBookingRepository Bookings { get; }

        IDiscountRepository DiscountCoupons { get; }
        IDistrictRepository Districts { get; }

        IImageRepository Images { get; }

        IProvinceRepository Provinces { get; }

        IRatingRepository Ratings { get; }
        IRoleRepository Roles { get; }
        IRolePermissionRepository RolePermissions { get; }

        IRoomRepository Rooms { get; }
        IRoomServiceRepository RoomServices { get; }

        IRoomTypeRepository RoomTypes { get; }
        IServiceRepository Services { get; }

        IBusinessPartnerRepository BusinessPartners { get; }

        ITouristRepository Tourists { get; }
        ISystemEmployeeRepository SystemEmployees { get; }
        IEmailAuthenRepository EmailAuthens { get; }
        IBankRepository Banks { get; }
        IBusinessPartnerBankRepository BusinessPartnerBanks { get; }

        Task<int> Complete();
    }
}
