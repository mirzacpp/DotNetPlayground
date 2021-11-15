using Studens.MvcNet6.WebUI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingletonDependenciesFromAttributes(typeof(Program).Assembly)
    .AddScopedDependenciesFromAttributes(typeof(Program).Assembly)
    .AddTransientDependenciesFromAttributes(typeof(Program).Assembly);

foreach (var item in builder.Services.Where(t => t.ServiceType == typeof(ITestService)))
{
    Console.WriteLine($"{item.ServiceType} - {item.ImplementationType} - {item.Lifetime}");
}

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
    var list = context.RequestServices.GetServices<ITestService>();
    var rez = string.Join(",", list.Select(s => s.GetMessage()));
    
    await context.Response.WriteAsync(rez);
});

app.Run();