using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Text.Json.Serialization;
using Tienda_Datos._01_DB;
using Tienda_Dominio._03_Interfaces;
using Tienda_Dominio._01_Clases;
using Tienda_Presentacion.Data;
using Tienda_Servicios._01_Cache;
using Tienda_Servicios._02_Afip;
using Tienda_Servicios._03_Tarjeta;
using Tienda_Aplicacion._01_Venta;
using Tienda_Aplicacion._02_Cache;
using Tienda_Datos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Configuraciones>(builder.Configuration.GetSection("Configuraciones"));
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDbContext<TiendaContexto>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("TiendaDB")));

// AQUI DECLARO LA INYECCION DE DEPENDENCIAS
//builder.Services.AddScoped<IServCache, ServicioCacheDist>();
builder.Services.AddScoped<IServCache, ServicioCacheMemoria>();
builder.Services.AddScoped<IServVenta, ServicioVenta>();
builder.Services.AddScoped<IServDatos, Tienda_DB_Repositorio>();
builder.Services.AddScoped<IServAFIP, ServicioAfip>();
builder.Services.AddScoped<IServTarjeta, ServicioTarjeta>();


builder.Services.AddDefaultIdentity<Tienda_Presentacion.Data.Usuario>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
//pattern: "{controller=Home}/{action=Index}/{id?}");
pattern: "{controller=Venta}/{action=Venta}");
app.MapRazorPages();

app.Run();
