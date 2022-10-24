using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simplicity.AspNetCore.Authentication.JwtBearer.Identity;
using Simplicity.AspNetCore.Authentication.JwtBearer.Models;
using Simplicity.Net6.WebApi.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ApplicationContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.Configure<JwtBearerAuthOptions>(builder.Configuration.GetSection(nameof(JwtBearerAuthOptions)));

builder.Services.AddJwtBearerIdentity<IdentityUser, IdentityRole, IdentityUserAccessToken<string>>(opt =>
{
    opt.Issuer = builder.Configuration[""];
})
.AddJwtEntityFrameworkStores<IdentityUserAccessToken<string>, ApplicationContext>();

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/users/create", async ([FromServices] JwtUserManager<IdentityUser> manager) =>
{
    var user = new IdentityUser("miki")
    {
        Email = "miki@miki.miki",
        PhoneNumber = "000-000-000"
    };

    await manager.CreateAsync(user, "Password123!");

    return user.ToString();
})
.WithName("CreteUser");

app.MapPost("/auth/token", async ([FromServices] JwtSignInManager<IdentityUser> manager) =>
{
    var user = await manager.UserManager.FindByIdAsync("bc09a8ca-2461-416f-ba4c-02dc66a64fff");
    var token = await manager.CreateAccessTokenAsync(user);

    return token;
})
.WithName("GetAccessToken");

app.MapGet("/unauthorized-value", () =>
{
    return "This is public message";
});

app.MapGet("/authorized-value", [Authorize] () =>
{
    return "If you see this message, that means you are authorized.";
});

app.Run();