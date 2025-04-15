using Business.Abstract;
using Business.Concrete;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.Cookie.Name = "SinavTakvimiAuth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
    });

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
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Statik dosyalar için middleware
app.UseStaticFiles();

app.UseCors("AllowAll");

app.UseRouting();

// Kimlik doğrulama ve yetkilendirme middleware'lerinin sırası önemli
app.UseAuthentication();
app.UseAuthorization();

// Özel middleware - Token kontrolü ve yönlendirme
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value?.ToLower();
    
    // Auth ile ilgili sayfalara ve statik dosyalara doğrudan erişime izin ver
    if (path != null && (
        path.StartsWith("/auth/") || 
        path.StartsWith("/lib/") || 
        path.StartsWith("/css/") || 
        path.StartsWith("/js/") ||
        path.StartsWith("/images/")))
    {
        await next();
        return;
    }

    // AuthToken cookie'sini kontrol et
    var authToken = context.Request.Cookies["AuthToken"];
    if (!string.IsNullOrEmpty(authToken))
    {
        await next();
        return;
    }

    // AJAX isteklerini kontrol et
    if (context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsJsonAsync(new { message = "Oturum süresi doldu. Lütfen tekrar giriş yapın." });
        return;
    }

    // Kimliği doğrulanmamış kullanıcıları login sayfasına yönlendir
    if (!path.Equals("/auth/login"))
    {
        context.Response.Redirect("/Auth/Login");
        return;
    }

    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
