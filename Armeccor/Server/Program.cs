//using Armeccor.Datos;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.OpenApi.Models;
//using System.Text.Json.Serialization;

//var builder = WebApplication.CreateBuilder(args);


//// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));
////builder.Services.AddDatabaseDeveloperPageExceptionFilter();


//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("conn"));
//});

//builder.Services.AddAutoMapper(typeof(Program));

//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Alquileress", Version = "v1" });
//});

//builder.Services.AddControllersWithViews()
//                .AddJsonOptions(x => x.JsonSerializerOptions
//                                      .ReferenceHandler = ReferenceHandler
//                                      .IgnoreCycles);



//builder.Services.AddControllersWithViews();
//builder.Services.AddRazorPages();

//var app = builder.Build();

//app.UseSwagger();
//app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json",
//                                        "Armeccor"));

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    //app.UseMigrationsEndPoint();
//    app.UseWebAssemblyDebugging();
//}
//else
//{
//    app.UseExceptionHandler("/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();

//app.UseBlazorFrameworkFiles();
//app.UseStaticFiles();

//app.UseRouting();

////app.UseIdentityServer();
//app.UseAuthentication();
//app.UseAuthorization();


//app.MapRazorPages();
//app.MapControllers();
//app.MapFallbackToFile("index.html");

//app.Run();


//using Armeccor.Datos;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.OpenApi.Models;
//using System.Text.Json.Serialization;

//var builder = WebApplication.CreateBuilder(args);


//// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));
////builder.Services.AddDatabaseDeveloperPageExceptionFilter();


//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("conn"));
//});

//builder.Services.AddAutoMapper(typeof(Program));

//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Alquileress", Version = "v1" });
//});

//builder.Services.AddControllersWithViews()
//                .AddJsonOptions(x => x.JsonSerializerOptions
//                                      .ReferenceHandler = ReferenceHandler
//                                      .IgnoreCycles);



//builder.Services.AddControllersWithViews();
//builder.Services.AddRazorPages();

//var app = builder.Build();

//app.UseSwagger();
//app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json",
//                                        "Armeccor"));

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    //app.UseMigrationsEndPoint();
//    app.UseWebAssemblyDebugging();
//}
//else
//{
//    app.UseExceptionHandler("/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();

//app.UseBlazorFrameworkFiles();
//app.UseStaticFiles();

//app.UseRouting();

////app.UseIdentityServer();
//app.UseAuthentication();
//app.UseAuthorization();


//app.MapRazorPages();
//app.MapControllers();
//app.MapFallbackToFile("index.html");

//app.Run();


//using Armeccor.Datos;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.OpenApi.Models;
//using System.Text.Json.Serialization;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("conn"));
//});

//builder.Services.AddAutoMapper(typeof(Program));

//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Alquileress", Version = "v1" });
//});

//builder.Services.AddControllersWithViews()
//    .AddJsonOptions(x => x.JsonSerializerOptions
//        .ReferenceHandler = ReferenceHandler.IgnoreCycles);

//builder.Services.AddControllersWithViews();
//builder.Services.AddRazorPages();

//var app = builder.Build();

//app.UseSwagger();
//app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json",
//    "Armeccor"));

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseWebAssemblyDebugging();
//}
//else
//{
//    app.UseExceptionHandler("/Error");
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseBlazorFrameworkFiles();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthentication();
//app.UseAuthorization();

//// ⚡️ Orden corregido ⚡️
//// 1. Mapea las rutas de Razor Pages (si las usas)
//app.MapRazorPages();

//// 2. Mapea las rutas de los controladores de API.
//// Este debe ir después de MapRazorPages.
//app.MapControllers();

//// 3. Este debe ser el ÚLTIMO en el pipeline de enrutamiento.
//// Captura todo lo que no fue manejado y lo redirige a tu app Blazor.
//app.MapFallbackToFile("index.html");

//app.Run();


using Armeccor.Datos;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("conn"));
});

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ArmecCor", Version = "v1" });
    c.SupportNonNullableReferenceTypes();
});

builder.Services.AddControllersWithViews()
    .AddJsonOptions(x => x.JsonSerializerOptions
        .ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json",
    "Armeccor"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// ⚡️ ESTE ES EL ORDEN CORRECTO ⚡️
// 1. Mapea las rutas de los controladores de API primero.
app.MapControllers();

// 2. Mapea las rutas de Razor Pages (si las usas).
app.MapRazorPages();

// 3. El middleware "catch-all" para la aplicación Blazor.
// Debe ser la ÚLTIMA línea de enrutamiento.
app.MapFallbackToFile("index.html");

app.Run();

