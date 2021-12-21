using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Studens.AspNetCore.Identity;
using Studens.AspNetCore.Identity.EntityFrameworkCore;
using Studens.MvcNet6.WebUI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseDefaultServiceProvider((context, options) =>
{
    var isDevelopment = context.HostingEnvironment.IsDevelopment();
    options.ValidateScopes = isDevelopment;
    options.ValidateOnBuild = isDevelopment;
});

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add db context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("StudensMvc6"));
}, ServiceLifetime.Scoped);

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddIdentityEntityFrameworkStores<ApplicationDbContext>() // This should be    
    .AddUserManager<IdentityUserManager<IdentityUser>>();

//builder.Services.AddScoped<IIdentityUserStore<IdentityUser>, IdentityUserStore<IdentityUser>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseClaimsDisplay();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();