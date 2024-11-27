using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using vigo.Domain.AccountFolder;
using vigo.Domain.Entity;
using vigo.Domain.User;
using vigo.Infrastructure.DBContext;
using vigo.Service.DTO.Shared;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace vigo.Admin
{
    public class Startup
    {
        private readonly IServiceProvider _serviceProvider;
        public Startup(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public void ConfigureServices()
        {
        }
        public void Configure(IApplicationBuilder app)
        {
            bool conActive = false;
            while (!conActive)
            {
                try
                {
                    Thread.Sleep(1000);
                    using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<VigoDatabaseContext>();
                        if (dbContext.Database.CanConnect())
                        {
                            if (dbContext.Database.GetPendingMigrations().Any())
                            {
                                dbContext.Database.Migrate();
                            }
                            conActive = true;
                        }
                        else
                        {
                            dbContext.Database.Migrate();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public async Task Init()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                try
                {
                    var _vigoContext = scopedServices.GetRequiredService<VigoDatabaseContext>();
                    var adminRole = _vigoContext.Roles.Where(e => e.Name == "admin").FirstOrDefault();
                    if (adminRole == null)
                    {
                        Role role = new Role()
                        {
                            Id = 1,
                            Name = "admin",
                            Permission = "account_manage,booking_manage,role_manage,discount_manage,rating_manage,room_manage,service_manage,bank_manage",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            DeletedDate = null
                        };
                        _vigoContext.Roles.Add(role);
                        Role role2 = new Role()
                        {
                            Id = 2,
                            Name = "tourist",
                            Permission = "",
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            DeletedDate = null
                        };
                        _vigoContext.Roles.Add(role);
                        _vigoContext.Roles.Add(role2);
                        _vigoContext.SaveChanges();
                    }
                    var admin = _vigoContext.Accounts.Where(e => e.Email == "admin456@gmail.com").FirstOrDefault();
                    if (admin == null)
                    {
                        var accountId = Guid.NewGuid();
                        var DateNow = DateTime.Now;
                        var salt = PasswordHasher.CreateSalt();
                        var hashedPassword = PasswordHasher.HashPassword("admin", salt);
                        Account account = new Account() {
                            Id = accountId,
                            Email = "admin456@gmail.com",
                            Password = hashedPassword,
                            Salt = salt,
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            EmailActive = true,
                            DeletedDate = null,
                            RoleId = 1,
                            UserType = "SystemEmployee"
                        };
                        _vigoContext.Accounts.Add(account);
                        SystemEmployee systemEmployee = new SystemEmployee()
                        {
                            AccountId = accountId,
                            Address = string.Empty,
                            Avatar = "http://localhost:2002/resource/default-avatar.jpg",
                            Bank = string.Empty,
                            BankNumber = string.Empty,
                            CreatedDate = DateNow,
                            DeletedDate= null,
                            DOB = DateNow,
                            FullName = "admin",
                            Name = "admin",
                            PhoneNumber = string.Empty,
                            Salary = 0,
                            UpdatedDate= DateNow,
                        };
                        _vigoContext.SystemEmployees.Add(systemEmployee);
                        _vigoContext.SaveChanges();
                    }

                    using (HttpClient client = new HttpClient())
                    {
                        string urlP = "https://esgoo.net/api-tinhthanh/1/0.htm";
                        HttpResponseMessage responseP = await client.GetAsync(urlP);
                        string responseBodyP = await responseP.Content.ReadAsStringAsync();
                        DataTemp<Province> dataP = JsonConvert.DeserializeObject<DataTemp<Province>>(responseBodyP)!;
                        _vigoContext.Provinces.AddRange(dataP.Data);
                        Random r = new Random();
                        foreach (Province province in dataP.Data)
                        {
                            province.Image = $"http://localhost:2002/resource/{r.Next(1,12)}.jpg";
                            string urlD = $"https://esgoo.net/api-tinhthanh/2/{province.Id}.htm";
                            HttpResponseMessage responseD = await client.GetAsync(urlD);
                            string responseBodyD = await responseD.Content.ReadAsStringAsync();
                            DataTemp<District> dataD = JsonConvert.DeserializeObject<DataTemp<District>>(responseBodyD)!;
                            foreach (var district in dataD!.Data)
                            {
                                district.ProvinceId = province.Id;
                            }
                            _vigoContext.Districts.AddRange(dataD.Data);
                        }

                        string bankUrl = "https://api.vietqr.io/v2/banks";
                        HttpResponseMessage responseBank = await client.GetAsync(bankUrl);
                        string responseBodyBank = await responseBank.Content.ReadAsStringAsync();
                        DataTemp<Bank> dataBank = JsonConvert.DeserializeObject<DataTemp<Bank>>(responseBodyBank)!;
                        _vigoContext.Banks.AddRange(dataBank.Data);
                        _vigoContext.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
