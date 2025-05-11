using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Microsoft.Extensions.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Добавляем Blazored.LocalStorage
builder.Services.AddBlazoredLocalStorage();

// HttpClient с базовым адресом
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Кастомный провайдер аутентификации (на основе токена)
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

// Если у тебя есть AuthMessageHandler для добавления токена к запросам
builder.Services.AddScoped<AuthMessageHandler>();
builder.Services.AddHttpClient("ServerAPI", client =>
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<AuthMessageHandler>();

await builder.Build().RunAsync();