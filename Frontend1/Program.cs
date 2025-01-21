using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IBolumService, BolumManager>();
builder.Services.AddScoped<IBolumDal, BolumDal>();

builder.Services.AddScoped<IDersService, DersManager>();
builder.Services.AddScoped<IDersDal, DersDal>();

builder.Services.AddScoped<IAkademikPersonelService, AkademikPersonelManager>();
builder.Services.AddScoped<IAkademikPersonelDal, AkademikPersonelDal>();

builder.Services.AddScoped<IDBAPService, DBAPManager>();
builder.Services.AddScoped<IDBAPDal, DBAPDal>();

builder.Services.AddScoped<IDerslikDal, DerslikDal>();
builder.Services.AddScoped<IDerslikService, DerslikManager>();

builder.Services.AddScoped<ISinavDetayDal, EfSinavDetayDal>();
builder.Services.AddScoped<ISinavDetayService, SinavDetayManager>();

builder.Services.AddScoped<ISinavDerslikDal, EfSinavDerslikDal>();
builder.Services.AddScoped<ISinavDerslikService, SinavDerslikManager>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
