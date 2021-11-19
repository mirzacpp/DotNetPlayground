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
            .Services
            .AddCustomMiniProfiler();
    }

    public virtual void Configure(IApplicationBuilder app)
    {       
        app
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
           .UseEndpoints(endpoints =>
           {
               endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Home}/{action=Index}/{id?}");
           });
    }
}
