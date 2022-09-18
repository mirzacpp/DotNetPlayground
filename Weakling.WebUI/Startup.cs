using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;
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
			.AddPocoOptions<AppConfig>(nameof(AppConfig), _configuration)
			.AddIf(_webHostEnvironment.IsDevelopment(), services.AddCustomMiniProfiler)
			.AddIf(_webHostEnvironment.IsDevelopment(), () => services.AddHttpLogging(options =>
			{
				options.LoggingFields = HttpLoggingFields.All;
			}));

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

		app
		   // This should be conditional. For more info see https://andrewlock.net/adding-host-filtering-to-kestrel-in-aspnetcore/
		   .UseHostFiltering()
		   .UseHttpLogging()
		   .UseIf(isDevelopment, app.UseDeveloperExceptionPage)
		   // TODO: Conditional builder with params
		   //.UseIf(!_webHostEnvironment.IsDevelopment(), app.UseExceptionHandler("/Home/Error"))
		   .UseIf(!isDevelopment, app.UseHsts)
		   .UseHttpsRedirection()
		   .UseStaticFiles()
		   .UseRouting()
		   .UseIf(isDevelopment, app.UseMiniProfiler) //Todo: Add miniprofiler flag instead of development
		   .UseAuthentication()
		   .UseIf(isDevelopment, app.UseClaimsDisplay)
		   .UseAuthorization()
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