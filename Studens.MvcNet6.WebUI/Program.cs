using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Studens.AspNetCore.Identity;
using Studens.AspNetCore.Mvc.Middleware.RegisteredServices;
using Studens.AspNetCore.Mvc.UI.TagHelpers.GoogleMaps;
using Studens.AspNetCore.Mvc.UI.TagHelpers.Localization;
using Studens.Commons.Localization;
using Studens.Data.Migration;
using Studens.Data.Seed;
using Studens.MvcNet6.WebUI.Data;
using Studens.MvcNet6.WebUI.MediatR.Services;
using Studens.MvcNet6.WebUI.OutOfProcess;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseDefaultServiceProvider((context, options) =>
{
	var isDevelopment = context.HostingEnvironment.IsDevelopment();
	options.ValidateScopes = isDevelopment;
	options.ValidateOnBuild = isDevelopment;
});

// Add services to the container.
builder.Services
.AddControllersWithViews()
.AddFeatureFolders()
.AddAreaFeatureFolders()
.Services.AddDisplayRegisteredServices();

builder.Services.Configure<GoogleMapsOptions>(options =>
{
	options.UrlPlaceholder = "https://localhost:7132/";
	options.ApiKeys.Add("miki1", "miki1-value");
});

builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddSingleton<ICustomerService, CustomerService>();

// Add db context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("StudensMvc6"));
}, ServiceLifetime.Scoped);

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
//builder.Services.AddPhysicalFileManager("C://ITO");
//builder.Services.AddAmazonFileManager(options =>
//{
//    options.RootPath = "";
//    options.BucketName = "test.ito.dev/test";
//    options.AccessKeyId = builder.Configuration["AmazonOptions:AccessKeyId"];
//    options.SecretAccessKey = builder.Configuration["AmazonOptions:SecretAccessKey"];
//});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
	.AddDefaultTokenProviders()
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddStudensEntityFrameworkStores<ApplicationDbContext>()
	.AddStudensRoleManager()
	.AddStudensUserManager()
	.AddStudensPasswordManager()
	.Services.AddScoped<IDataMigrationManager, DataMigrationManager>()
	.Configure<DataSeedOptions>(options => options.Environment = builder.Environment.EnvironmentName)
	.AddScoped<IDataSeedManager, DataSeedManager>()
	.AddTransient<ILanguageProvider, DefaultLanguageProvider>()
	.AddFluentValidationAutoValidation(cfg =>
	{
		cfg.DisableDataAnnotationsValidation = true;
	})
	.AddValidatorsFromAssemblyContaining<Program>()
	.Configure<LocalizationOptions>(options =>
	{
		var defaultLango = new LanguageInfo("bs-Latn-BA", "bs-Latn-BA", "Bosnian, Latin", "ba");

		options.DefaultLanguage = defaultLango;
		options.Languages.Add(defaultLango);
		options.Languages.Add(new LanguageInfo("en-GB", "en-GB", "English, GB", "gb-eng"));
		options.Languages.Add(new LanguageInfo("ar-EG", "ar-EG", "Arabic, Egypt", "ar"));
	})
	.AddSingleton<IInputControlGenerator, Bootstrap5ControlGenerator>()
	.AddDataSeedContributorFromMarkers(typeof(Program))
	.AddDisplayRegisteredServices("/testovka");

builder.Services
.AddScoped<IEmailSender, DumbEmailSender>()
.AddScoped<EmailWorker>()
.AddHostedService<EmailBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	//app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
	app.UseDisplayRegisteredServices();
	app.UseHttpLogging();
	app.Logger.LogDebug((app.Configuration as IConfigurationRoot).GetDebugView());
}

app.UseHttpsRedirection();
app.UseStaticFiles();
//app.UsePhysicalFileManagerBrowser("/file-browser");
app.UseRouting();
app.UseAuthentication();
app.UseClaimsDisplay();
app.UseAuthorization();

app.UseMediatRTestEndpoints();
app.MapOutOfProcessEndpoints();

app.MapAreaControllerRoute(
	name: "auth_area",
	areaName: "Auth",
	pattern: "Auth/{controller=Account}/{action=Index}/{id?}");

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseDisplayRegisteredServices();

//app.MapGet("/files", async (context) =>
//{
//    var fileManager = context.RequestServices.GetService<IFileManager<Amazon>();
//    var fileName = Path.Combine(Directory.GetCurrentDirectory(), "upload.txt");
//    using var fs = File.OpenRead(fileName);
//    var bytes = fs.GetAllBytes();

//    var result = await fileManager.SaveAsync(new PersistFileInfo(bytes, "tests2.txt", "vlado/vlado2", false));
//    var converted = JsonSerializer.Serialize(result);

//    await context.Response.WriteAsync(converted);
//});

//app.MapGet("/files-delete", async (context) =>
//{
//    var fileManager = context.RequestServices.GetService<IFileManager>();

//    var result = await fileManager.DeleteAsync("/vlado/vlado2/test2.txt");
//    var converted = JsonSerializer.Serialize(result);

//    await context.Response.WriteAsync(converted);
//});

//app.MapGet("/files-enum", async (context) =>
//{
//    var fileManager = context.RequestServices.GetService<IFileManager>();

//    var result = await fileManager.EnumerateFilesAsync("vlado/vlado2", "*", true);
//    var converted = JsonSerializer.Serialize(result);

//    await context.Response.WriteAsync(converted);
//});

//app.MapGet("/files-info", async (context) =>
//{
//    var fileManager = context.RequestServices.GetService<IFileManager>();

//    var result = await fileManager.GetFileInfoAsync("vlado/vlado2/tests.txt");
//    var converted = JsonSerializer.Serialize(result);

//    await context.Response.WriteAsync(converted);
//});

//Run migrator
//using (var scope = app.Services.CreateScope())
//{
//	try
//	{
//		var migrator = scope.ServiceProvider.GetRequiredService<IDataMigrationManager>();
//		await migrator.MigrateAsync();
//	}
//	catch (Exception ex)
//	{
//		Console.WriteLine(ex.Message);
//		throw;
//	}
//}

app.Run();