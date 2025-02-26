using BobMeetsAlice.Components;
using BobMeetsAlice.Services;

namespace BobMeetsAlice;

public class Program
{
  public static void Main(string[] args)
  {
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();

    builder.Services.AddSingleton<InvoiceService>(); // Register Singleton
    builder.Services.AddSingleton<UpdateService>(); // Register Singleton

    WebApplication app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
      app.UseExceptionHandler("/Error");
    }

    app.UseStaticFiles();
    app.UseAntiforgery();

    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    app.Run();
  }
}
