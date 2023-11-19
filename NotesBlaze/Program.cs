using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NotesBlaze;
using NotesBlaze.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);



builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<ToastService>();

builder.Services.AddHttpClient<INotesDataService, NotesDataService>(client =>
   client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

builder.Services.AddSingleton<StateContainer>();



builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
//builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();

