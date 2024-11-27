using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using vigo.Admin;
using vigo.Domain.Interface.IUnitOfWork;
using vigo.Infrastructure.DBContext;
using vigo.Infrastructure.UnitOfWork;
using vigo.Service.Admin;
using vigo.Service.Admin.IService;
using vigo.Service.Admin.Service;
using vigo.Service.Helper;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("default");
builder.Services.AddDbContext<VigoDatabaseContext>(options =>
            options.UseMySql(connectionString,
            ServerVersion.AutoDetect(connectionString)));

// Add services to the container.

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "VigoAdminApi", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter JWT Bearer token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };
    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    };
    c.AddSecurityRequirement(securityRequirement);
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtSettings:Key").Value!)),
            ClockSkew = TimeSpan.Zero
        };
    });

//Service Dependency Injection
#region service
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IDiscountCouponService, DiscountCouponService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IRatingService, RatingService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();
builder.Services.AddScoped<IBankService, BankService>();
builder.Services.AddScoped<ICheckOutService, CheckOutService>();
//builder.Services.AddScoped<ISystemEmployeeService, SystemEmployeeService>();
//builder.Services.AddScoped<ITouristService, TouristService>();
#endregion

//UnitOfWork Dependency Injection
builder.Services.AddScoped<IUnitOfWorkVigo, UnitOfWorkVigo>();

//DbContext Denpendency Injection
builder.Services.AddScoped<VigoDatabaseContext, VigoDatabaseContext>();

builder.Services.AddHostedService<VigoBackGroundService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfiles));

builder.Services.AddCors(option =>
{
    option.AddPolicy(
        name: "CorsPolicy",
        configurePolicy: builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = null;
});

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(80);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(option => option.SwaggerEndpoint("/swagger/v1/swagger.json", "Vigo Admin Api"));

app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

var startup = new Startup(app.Services);
startup.Configure(app);
await startup.Init();

app.Run();