//using Armeccor.Client;
//using Armeccor.Client.Servicios;
//using Armeccor.Client.Shared;
//using Microsoft.AspNetCore.Components.Web;
//using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Net.Http;

//var builder = WebAssemblyHostBuilder.CreateDefault(args);
//builder.RootComponents.Add<App>("#app");
//builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//builder.Services.AddScoped<IHttpServicio, HttpServicio>();

//// Program.cs en el proyecto Blazor (Client)
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//// Agrega esta línea para registrar el servicio de estado
//builder.Services.AddSingleton<OrdenStateService>();
//builder.Services.AddScoped<INotificationService, NotificationService>();

//await builder.Build().RunAsync();

using Armeccor.Client;
using Armeccor.Client.Servicios;
using Armeccor.Client.Shared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClient base
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Servicio propio
builder.Services.AddScoped<IHttpServicio, HttpServicio>();

// Estado y notificaciones
builder.Services.AddSingleton<OrdenStateService>();
builder.Services.AddScoped<INotificationService, NotificationService>();


// ? Servicio que inyecta automáticamente el JWT en cada request
builder.Services.AddScoped(sp =>
{
    var js = sp.GetRequiredService<IJSRuntime>();
    var http = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };

    // Captura el token desde localStorage
    js.InvokeAsync<string>("localStorage.getItem", "authToken").AsTask().ContinueWith(task =>
    {
        var token = task.Result;
        if (!string.IsNullOrEmpty(token))
        {
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    });

    return http;
});

await builder.Build().RunAsync();
