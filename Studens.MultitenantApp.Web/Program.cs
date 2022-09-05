using AuthPermissions.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rev.AuthPermissions;
using Rev.AuthPermissions.AdminCode;
using Rev.AuthPermissions.BaseCode;
using Rev.AuthPermissions.BaseCode.DataLayer.Classes;
using Rev.AuthPermissions.BaseCode.DataLayer.EfCode;
using Rev.AuthPermissions.BaseCode.SetupCode;
using Rev.AuthPermissions.SupportCode.ShardingServices;
using RunMethodsSequentially;
using Studens.Data.Migration;
using Studens.Data.Seed;
using Studens.MultitenantApp.Web;
using Studens.MultitenantApp.Web.Data;
using Studens.MultitenantApp.Web.Permissions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseDefaultServiceProvider(opt =>
{
	opt.ValidateScopes = true;
	opt.ValidateOnBuild = true;
});

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
	options.UseSqlServer(connectionString);
	EntityFramework.Exceptions.SqlServer.ExceptionProcessorExtensions.UseExceptionProcessor(options);
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>()
	.AddEntityFrameworkStores<AuthPermissionsDbContext>();

builder.Services
.AddControllersWithViews()
.Services
.AddScoped<IDataMigrationManager, DataMigrationManager>()
.Configure<DataSeedOptions>(options => options.Environment = builder.Environment.EnvironmentName)
.AddScoped<IDataSeedManager, DataSeedManager>()
.AddDataSeedContributorFromMarkers(typeof(Program));

builder.Services.RegisterAuthPermissions<AppPermissions>(options =>
{
	options.TenantType = TenantTypes.SingleLevel | TenantTypes.AddSharding;
	options.EncryptionKey = builder.Configuration[nameof(AuthPermissionsOptions.EncryptionKey)];
	options.PathToFolderToLock = builder.Environment.WebRootPath;
	options.Configuration = builder.Configuration;
})
.UsingEfCoreSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
.IndividualAccountsAuthentication<User>()
.SetupAspNetCoreAndDatabase(options =>
{
	//Migrate individual account database and seed data
	options.RegisterServiceToRunInJob<StartupServiceMigrateDatabase>();
});

builder.Services.AddScoped<ITenantDbContextFactory, DbTenantContextFactory>();
builder.Services.AddTransient<IAccessDatabaseInformation, AccessDatabaseInformation>();
builder.Services.AddTransient<ITenantChangeService, TenantChangeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
	// Lets sign in super admin
	var userManager = context.RequestServices.GetRequiredService<UserManager<User>>();
	var signInManager = context.RequestServices.GetRequiredService<SignInManager<User>>();

	var user = await userManager.FindByEmailAsync("superadmin@app.ba");

	await signInManager.SignInAsync(user, true);

	await next(context);
});

app.MapGet("/get", async context =>
{
	var ser = context.RequestServices.GetRequiredService<IAccessDatabaseInformation>();
	var list = ser.ReadShardingSettingsFile();

	await context.Response.WriteAsJsonAsync(list);
});

app.MapGet("/get-new-tenant", async context =>
{
	var ser = context.RequestServices.GetRequiredService<IAuthTenantAdminService>();
	var status = await ser.AddSingleTenantAsync("East", null,
			   hasOwnDb: true, databaseInfoName: "DatabaseEast1");

	await context.Response.WriteAsJsonAsync(status);
});

app.MapGet("/user-login", async ctx =>
{
	var userManager = ctx.RequestServices.GetRequiredService<UserManager<User>>();
	var signInManager = ctx.RequestServices.GetRequiredService<SignInManager<User>>();
	var user = await userManager.FindByIdAsync("7124caff-bda7-4497-a9fd-1e5c06829fdc");

	await signInManager.SignInAsync(user, true);

	await ctx.Response.WriteAsJsonAsync("User logged in");
});

app.MapGet("/user-claims", async ctx =>
{
	var userClaims = ctx.User.Claims.Select(c => new
	{
		Name = c.Type,
		Value = c.Value
	});

	await ctx.Response.WriteAsJsonAsync(userClaims);
});

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();