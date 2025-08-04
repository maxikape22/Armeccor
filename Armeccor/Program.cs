using Armeccor.Datos;
using Armeccor.Entidades;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//AREA DE SERVICIOS

builder.Services.AddControllers();


builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
opciones.UseSqlServer("name=DefaultConnection"));

var app = builder.Build();

//AREA DE MIDDLEWARES

app.MapControllers();

app.Run();
