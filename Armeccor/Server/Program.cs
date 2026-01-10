//using Armeccor.Datos;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.FileProviders;
//using Microsoft.Extensions.Hosting;
//using Microsoft.OpenApi.Models;
//using System.IO;
//using System.Text.Json.Serialization;


//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//var connectionString = builder.Configuration.GetConnectionString("Conexion");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));

//builder.Services.AddAutoMapper(typeof(Program));

//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ArmecCor", Version = "v1" });
//    c.SupportNonNullableReferenceTypes();
//});

//// ✅ COMIENZO DE LA CONFIGURACIÓN DE CORS
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAllOrigins",
//        policy =>
//        {
//            policy//.AllowAnyOrigin()
//                  .AllowAnyMethod()
//                  .AllowAnyHeader()
//                  .SetIsOriginAllowed(origin => true)            
//                  .AllowCredentials();
//        });
//});
//// ✅ FIN DE LA CONFIGURACIÓN DE CORS

//builder.Services.AddControllersWithViews()
//    .AddJsonOptions(x => x.JsonSerializerOptions
//        .ReferenceHandler = ReferenceHandler.IgnoreCycles);
//builder.Services.ConfigureHttpJsonOptions(options =>
//{
//    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
//    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
//});

//builder.Services.AddControllersWithViews();
//builder.Services.AddRazorPages();

//var app = builder.Build();


//////
////var rutaPlanos = Path.Combine(app.Environment.WebRootPath, "Planos");

////if (!Directory.Exists(rutaPlanos))
////    Directory.CreateDirectory(rutaPlanos);

////// ● Esto hace accesible la carpeta /planos/ como archivos públicos
////app.UseStaticFiles(new StaticFileOptions
////{
////    FileProvider = new PhysicalFileProvider(rutaPlanos),
////    RequestPath = "/Planos"
////});
//////



////DESDE ACA

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

////HASTA ACA SOLO COMENTADO

//app.UseHttpsRedirection();
//app.UseBlazorFrameworkFiles();
//app.UseStaticFiles();


//app.UseRouting();

//// ✅ AQUÍ SE HABILITA EL MIDDLEWARE DE CORS
//app.UseCors("AllowAllOrigins");
//// ✅ FIN DE LA HABILITACIÓN DEL MIDDLEWARE DE CORS

//app.UseAuthentication();
//app.UseAuthorization();
//app.MapControllers();
//app.MapRazorPages();
//app.MapFallbackToFile("index.html");
//app.Run();

using Armeccor.Datos;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Text.Json.Serialization;

// 🔑 JWT
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("Conexion");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ArmecCor", Version = "v1" });
    c.SupportNonNullableReferenceTypes();
});

// ✅ Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials();
        });
});

// ✅ Configuración de JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "Armeccor",   // tu issuer
        ValidAudience = "Armeccor", // tu audience
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("clave-secreta-super-segura")) // clave secreta
    };
});

builder.Services.AddAuthorization();

builder.Services.AddControllersWithViews()
    .AddJsonOptions(x => x.JsonSerializerOptions
        .ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Armeccor"));

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

// ✅ Middleware de CORS
app.UseCors("AllowAllOrigins");

// ✅ Middleware de autenticación/autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();
app.MapFallbackToFile("index.html");

app.Run();
