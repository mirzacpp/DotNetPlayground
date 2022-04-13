using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studens.AspNetCore.Authentication.JwtBearer.Identity;
using Studens.AspNetCore.Authentication.JwtBearer.Models;
using Studens.Net6.WebApi.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ApplicationContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddJwtIdentity<IdentityUser, IdentityRole, IdentityUserAccessToken<string>>()
    .AddJwtEntityFrameworkStores<IdentityUserAccessToken<string>, ApplicationContext >();    

builder.Services.AddAuthorization();

builder.Services.Configure<IdentityOptions>(opt =>
{
    opt.Stores.ProtectPersonalData = true;
});

//builder.Services.ConfigureJwtOptions ...

//builder.Services.AddAuthentication(opt =>
//{
//    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(opt =>
//{
//    opt.RequireHttpsMetadata = false;
//    opt.SaveToken = true;
//    opt.Audience = "Audience";
//    opt.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("Secret brato")),
//        ValidateIssuer = false,
//        ValidateAudience = false,
//        ValidateLifetime = true,
//        // When token expiration time is being validated it is added on ClockSkew(default 5min. + token set expiration)
//        ClockSkew = TimeSpan.Zero,
//        RequireExpirationTime = true
//    };
//});

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

app.MapPost("/auth/token", async ([FromServices] JwtUserManager<IdentityUser, IdentityUserAccessToken<string>> manager) =>
{
    var token = await manager.GetAccessTokenAsync(new IdentityUser("vlado"));

    return token;
})
.WithName("GetAccessToken");

app.MapGet("/unauthorized-value", () =>
{
    return "This is public message";
});

app.MapGet("/authorized-value", [Authorize]() =>
{
    return "If you see this message, that means you are authorized.";
});

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}