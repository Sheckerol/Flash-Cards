using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FlashCards;
using FlashCards.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// App services. In WebAssembly there is a single user/session, so scoped == singleton.
builder.Services.AddScoped<LocalStorage>();
builder.Services.AddScoped<DeckStore>();

await builder.Build().RunAsync();
