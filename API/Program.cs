using Business.Abstract;
using Business.Concrete;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
});

// CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

// Service Dependencies
builder.Services.AddScoped<IBolumService, BolumManager>();
builder.Services.AddScoped<IBolumDal, BolumDal>();

builder.Services.AddScoped<IDersService, DersManager>();
builder.Services.AddScoped<IDersDal, DersDal>();

builder.Services.AddScoped<IAkademikPersonelService, AkademikPersonelManager>();
builder.Services.AddScoped<IAkademikPersonelDal, EfAkademikPersonelDal>();

builder.Services.AddScoped<IDBAPService, DBAPManager>();
builder.Services.AddScoped<IDBAPDal, DBAPDal>();

builder.Services.AddScoped<IDerslikDal, DerslikDal>();
builder.Services.AddScoped<IDerslikService, DerslikManager>();

builder.Services.AddScoped<ISinavDetayDal, EfSinavDetayDal>();
builder.Services.AddScoped<ISinavDetayService, SinavDetayManager>();

builder.Services.AddScoped<ISinavDerslikDal, EfSinavDerslikDal>();
builder.Services.AddScoped<ISinavDerslikService, SinavDerslikManager>();

builder.Services.AddScoped<IAuthService, AuthManager>();
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<IUserDal, EfUserDal>();
builder.Services.AddScoped<ITokenHelper, JwtHelper>();

// Operation Claims servisleri
builder.Services.AddScoped<IOperationClaimService, OperationClaimManager>();
builder.Services.AddScoped<IOperationClaimDal, EfOperationClaimDal>();

// User Operation Claims servisleri
builder.Services.AddScoped<IUserOperationClaimService, UserOperationClaimManager>();
builder.Services.AddScoped<IUserOperationClaimDal, EfUserOperationClaimDal>();

// JWT Configuration
builder.Services.Configure<TokenOptions>(builder.Configuration.GetSection("TokenOptions"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
