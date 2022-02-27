using Serilog;
using System.Text.Json;
using Weakling.WebUI.Configuration;

namespace Weakling.WebUI;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        _configuration = configuration;
        _webHostEnvironment = webHostEnvironment;
    }

    public virtual void ConfigureServices(IServiceCollection services)
    {
        services
            .AddControllersWithViews()
            .AddFeatureFolders()
            .AddAreaFeatureFolders()
            .Services
            //.AddDatabaseDeveloperPageExceptionFilter() Use when db is configured
            .AddPocoOptions<AppConfig>(nameof(AppConfig), _configuration)
            .AddIf(_webHostEnvironment.IsDevelopment(), services.AddCustomMiniProfiler);        
    }

    public virtual void Configure(IApplicationBuilder app)
    {
        app
           // This should be conditional. For more info see https://andrewlock.net/adding-host-filtering-to-kestrel-in-aspnetcore/
           //.UseHostFiltering()
           .UseIf(_webHostEnvironment.IsDevelopment(), app.UseDeveloperExceptionPage)
           // TODO: Conditional builder with params
           //.UseIf(!_webHostEnvironment.IsDevelopment(), app.UseExceptionHandler("/Home/Error"))
           .UseIf(!_webHostEnvironment.IsDevelopment(), app.UseHsts)
           .UseHttpsRedirection()
           .UseStaticFiles()
           .UseRouting()
           .UseIf(_webHostEnvironment.IsDevelopment(), app.UseMiniProfiler)
           .UseAuthentication()
           .UseIf(_webHostEnvironment.IsDevelopment(), app.UseClaimsDisplay)
           .UseAuthorization()
           .UseSerilogRequestLogging()
           .UseEndpoints(endpoints =>
           {
               endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Home}/{action=Index}/{id?}");

               endpoints.MapGet("testovka", async (context) =>
               {
                   var logFact = context.RequestServices.GetRequiredService<AppConfig>();
                   var value = JsonSerializer.Serialize(logFact);
                   await context.Response.WriteAsync(value);
               });
           });
    }
}