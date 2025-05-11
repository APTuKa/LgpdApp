using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Microsoft.Extensions.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// ��������� Blazored.LocalStorage
builder.Services.AddBlazoredLocalStorage();

// HttpClient � ������� �������
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// ��������� ��������� �������������� (�� ������ ������)
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

// ���� � ���� ���� AuthMessageHandler ��� ���������� ������ � ��������
builder.Services.AddScoped<AuthMessageHandler>();
builder.Services.AddHttpClient("ServerAPI", client =>
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<AuthMessageHandler>();

await builder.Build().RunAsync();