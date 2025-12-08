using CineSoul.Data;
using CineSoul.Models;
using CineSoul.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Veritabanı Bağlantısı
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Identity (Login/Signup) Ayarları
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Controller ve View servisleri
builder.Services.AddControllersWithViews();

// TMDB HttpClient Servisi
builder.Services.AddHttpClient<ITmdbService, TmdbService>();

// ====================================================================
// SWAGGER SERVİS YAPILANDIRMASI
// ====================================================================

// API Explorer'ı etkinleştir
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    // API belgesine temel bilgileri ekle
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CineSoul API",
        Version = "v1",
        Description = "CineSoul Film Platformu için TMDB ve Kullanıcı Listeleri API'si."
    });


    // XML dosya yolunu belirle
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    // XML yorumlarını Swagger'a dahil et
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// ====================================================================
// 2. MIDDLEWARE YAPILANDIRMASI
// ====================================================================

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
else
{
    // SWAGGER MIDDLEWARE (Sadece Geliştirme Ortamında Çalışır)
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // Swagger UI, https://localhost:PORT/swagger adresinde açılır.
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CineSoul API V1");
    });
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();