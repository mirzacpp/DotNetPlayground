using Microsoft.AspNetCore.HttpLogging;
using Serilog;
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
			//.AddApplicationPart(typeof(Startup).Assembly)
			.Services
			//.AddDatabaseDeveloperPageExceptionFilter() // Use when db is configured
			.AddPocoOptions<ApplicationOptions>(nameof(ApplicationOptions), _configuration)
			.AddIf(_webHostEnvironment.IsDevelopment(), services.AddCustomMiniProfiler);
			//.AddIf(_webHostEnvironment.IsDevelopment(), () => services.AddHttpLogging(options =>
			//{
			//	options.LoggingFields = HttpLoggingFields.All;
			//}));

			//services.AddHttpLogging(options =>
			//{
			//	options.LoggingFields = HttpLoggingFields.All;
			//});

		//if (_webHostEnvironment.IsDevelopment())
		//{
		//	services.AddHttpLogging(options =>
		//	{
		//		options.LoggingFields = HttpLoggingFields.All;
		//	});
		//}
	}

	public virtual void Configure(IApplicationBuilder app)
	{
		var isDevelopment = _webHostEnvironment.IsDevelopment();
		var appOptions = _configuration.GetRequiredOptions<ApplicationOptions>(nameof(ApplicationOptions));

		app
		   // This should be conditional. For more info see https://andrewlock.net/adding-host-filtering-to-kestrel-in-aspnetcore/
		   .UseHostFiltering()		   
		   //.UseSerilogRequestLogging()
		   .UseIf(isDevelopment, app.UseDeveloperExceptionPage)		   	   
		   .UseIf(!isDevelopment, () => app.UseExceptionHandler("/Home/Error"))
		   .UseIf(!isDevelopment, app.UseHsts)
		   .UseHttpsRedirection()
		   .UseStaticFiles()
		   .UseIf(appOptions.EnableHttpLogging, () => app.UseSerilogRequestLogging())
		   .UseIf(appOptions.EnableMiniProfiler, app.UseMiniProfiler)
		   .UseRouting()
		   .UseAuthentication()
		   .UseIf(isDevelopment, app.UseClaimsDisplay)
		   .UseAuthorization()
		   .UseEndpoints(endpoints =>
		   {
			   endpoints.MapDefaultControllerRoute();
		   });
	}
}