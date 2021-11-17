using Studens.MvcNet6.WebUI.Models;
using Studens.MvcNet6.WebUI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddPocoOptions<TestOptions>(nameof(TestOptions), builder.Configuration, out var options);
Console.WriteLine(options.TestProp);
var simpler = builder.Configuration.GetRequiredValue<string>("TestOptions:TestProp");
Console.WriteLine(simpler);

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

app.MapGet("testovka", async (context) =>
{
    var options = context.RequestServices.GetRequiredService<TestOptions>();   
    
    await context.Response.WriteAsync(options.TestProp);
});

app.Run();