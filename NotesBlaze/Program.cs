using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NotesBlaze;
using NotesBlaze.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);



builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<ToastService>();


builder.Services.AddHttpClient<IApiCallHandler, ApiCallHandler>(client =>
   client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler(provider =>
    {
        var jsRuntime = provider.GetRequiredService<IJSRuntime>();
        return new AuthDelegatingHandler(jsRuntime);
    });

builder.Services.AddScoped<INotesDataService,NotesDataService>();

builder.Services.AddScoped<StateContainer>();



builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
//builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();

