using Serilog;
using Simplicity.Templates.WebUI.Configuration;

namespace Simplicity.Templates.WebUI;

public class Startup
{
	private readonly IConfiguration _configuration;
	private readonly IWebHostEnvironment _webHostEnvironment;

	public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
	{
		_configuration = configuration;
		_webHostEnvironment = webHostEnvironment;
	}

	/// <summary>
	/// TODO:
	/// Configure miniprofiler
	/// </summary>
	/// <param name="services"></param>
	public virtual void ConfigureServices(IServiceCollection services)
	{
		services
			.AddControllersWithViews()
			.AddFeatureFolders()
			.AddAreaFeatureFolders()			
			.Services
			//.AddDatabaseDeveloperPageExceptionFilter() // Use when db is configured
			.AddPocoOptions<ApplicationOptions>(nameof(ApplicationOptions), _configuration, out var appOptions)
			.AddIf(appOptions.Setup.EnableMiniProfiler, s => s.AddMiniProfilerConfigured());
	}

	public virtual void Configure(IApplicationBuilder app)
	{
		var isDevelopment = _webHostEnvironment.IsDevelopment();
		var appOptions = _configuration.GetRequiredOptions<ApplicationOptions>(nameof(ApplicationOptions));

		app
		   // This should be conditional. For more info see https://andrewlock.net/adding-host-filtering-to-kestrel-in-aspnetcore/		   
		   .UseIf(appOptions.Setup.EnableHttpLogging, app => app.UseHostFiltering())		   
		   .UseIf(isDevelopment, app => app.UseDeveloperExceptionPage())
		   .UseIf(!isDevelopment, app => app.UseExceptionHandler("/Home/Error"))
		   .UseIf(!isDevelopment, app => app.UseHsts())
		   .UseIf(appOptions.Setup.EnableHttpsRedirection, app => app.UseHttpsRedirection())		   
		   .UseStaticFiles()
		   .UseIf(appOptions.Setup.EnableHttpLogging, app => app.UseSerilogRequestLogging())
		   .UseIf(appOptions.Setup.EnableMiniProfiler, app => app.UseMiniProfiler())
		   .UseRouting()
		   .UseAuthentication()
		   .UseIf(isDevelopment, app => app.UseClaimsDisplay())
		   .UseAuthorization()
		   .UseEndpoints(endpoints =>
		   {
			   endpoints.MapDefaultControllerRoute();
		   });
	}
}