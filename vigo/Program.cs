using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using vigo.Domain.Interface.IUnitOfWork;
using vigo.Infrastructure.DBContext;
using vigo.Infrastructure.UnitOfWork;
using vigo.Service.Application.IServiceApp;
using vigo.Service.Application.ServiceApp;
using vigo.Service.EmailAuthenModule;
using vigo.Service.Helper;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("default");
builder.Services.AddDbContext<VigoDatabaseContext>(options =>
            options.UseMySql(connectionString,
            ServerVersion.AutoDetect(connectionString)));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "VigoApplicationApi", Version = "v1" });

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
builder.Services.AddScoped<IAccountAppService, AccountAppService>();
builder.Services.AddScoped<IBookingAppService, BookingAppService>();
builder.Services.AddScoped<IDiscountCouponAppService, DiscountCouponAppService>();
builder.Services.AddScoped<IRatingAppService, RatingAppService>();
builder.Services.AddScoped<IRoomAppService, RoomAppService>();
builder.Services.AddScoped<IRoomTypeAppService, RoomTypeAppService>();
builder.Services.AddScoped<IServiceAppService, ServiceAppService>();
builder.Services.AddScoped<IUIService, UIService>();
builder.Services.AddScoped<ISearchAppService, SearchAppService>();
#endregion

//UnitOfWork Dependency Injection
builder.Services.AddScoped<IUnitOfWorkVigo, UnitOfWorkVigo>();

//DbContext Denpendency Injection
builder.Services.AddScoped<VigoDatabaseContext, VigoDatabaseContext>();


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
app.UseSwaggerUI(option => option.SwaggerEndpoint("/swagger/v1/swagger.json", "Vigo Application Api"));

app.UseRouting();
app.UseCors("CorsPolicy");

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider("/app/volume/image"),
    RequestPath = "/resource"
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

var temp = EmailAuthenProducer.Instance;

app.Run();