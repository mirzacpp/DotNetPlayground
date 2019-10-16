using AutoMapper;
using Cleaners.Core.Constants;
using Cleaners.Core.Domain;
using Cleaners.Core.Interfaces;
using Cleaners.Data;
using Cleaners.Services.Roles;
using Cleaners.Services.Users;
using Cleaners.Web.Constants;
using Cleaners.Web.Infrastructure;
using Cleaners.Web.Infrastructure.AppSettings;
using Cleaners.Web.Infrastructure.Authentication;
using Cleaners.Web.Infrastructure.AutoMapper;
using Cleaners.Web.Infrastructure.Localization;
using Cleaners.Web.Localization;
using Cleaners.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Cleaners.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        #region Methods

        /// <summary>
        /// Registers MVC services
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureMvc(this IServiceCollection services)
        {
            var mvcBuilder = services.AddMvc(options =>
            {
                // Enable automatic validation of antiforgerytoken
                // This way we don't have to always include ValidateAntiForgeryToken attribute
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            mvcBuilder.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);
            mvcBuilder.AddDataAnnotationsLocalization();

            mvcBuilder.AddCookieTempDataProvider(options =>
            {
                options.Cookie.Name = $"{CookieDefaults.Prefix}{CookieDefaults.TempDataCookie}";
            });
        }

        /// <summary>
        /// Registers MiniProfiler services
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureMiniProfiler(this IServiceCollection services)
        {
            services.AddMiniProfiler(options =>
            {
                options.PopupRenderPosition = StackExchange.Profiling.RenderPosition.BottomLeft;
                options.PopupShowTimeWithChildren = true;
                options.RouteBasePath = "/profiler";
                options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.SqlServerFormatter();

                // Authorize access to mini profiler data
                // Only for users in "Support" role
                options.ResultsAuthorize = request => request.HttpContext.User.IsSupport();
                options.ResultsListAuthorize = request => request.HttpContext.User.IsSupport();
            })
            // Track database calls
            .AddEntityFramework();
        }

        /// <summary>
        /// Configures app settings from appsettings.json
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppInfoConfig>(configuration.GetSection(AppSettingsSectionNames.AppInfo));
            // Register IdentityOptions so it can be used in application
            services.Configure<IdentityConfig>(configuration.GetSection(AppSettingsSectionNames.Identity));

            // Allows config to be injected directly as instance without IOptionsSnapshot<>
            services.AddScoped(provider => provider.GetRequiredService<IOptionsSnapshot<AppInfoConfig>>().Value);
            // Allows config to be injected directly as instance without IOptions<>
            services.AddSingleton(provider => provider.GetRequiredService<IOptions<IdentityConfig>>().Value);
        }

        /// <summary>
        /// Configures razor view engine
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureRazorViewEngine(this IServiceCollection services)
        {
            services.Configure<RazorViewEngineOptions>(options =>
            {
                // Clear all view locations, so we avoid Razor searching for views inside Pages folder
                // Register areas if necessary
                options.ViewLocationFormats.Clear();
                options.ViewLocationFormats.Add($"/Views/{{1}}/{{0}}{RazorViewEngine.ViewExtension}");
                options.ViewLocationFormats.Add($"/Views/Shared/{{0}}{RazorViewEngine.ViewExtension}");
            });
        }

        /// <summary>
        /// Registers localization services
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureLocalization(this IServiceCollection services)
        {
            // Use NullStringLocalizerFactory instead of default ResourceManagerStringLocalizer
            services.AddSingleton<IStringLocalizerFactory, NullStringLocalizerFactory>();

            services.AddPortableObjectLocalization(options => options.ResourcesPath = LocalizationDefaults.ResourcesPath);

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var cultures = LocalizationDefaults.GetSupportedCultureInfos();

                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
                options.DefaultRequestCulture = new RequestCulture(LocalizationDefaults.GetDefaultCultureInfo());
                options.FallBackToParentCultures = true;
                options.FallBackToParentUICultures = true;

                // Set cookie name for cookie provider
                var cookieProvider = options.RequestCultureProviders.OfType<CookieRequestCultureProvider>().First();

                cookieProvider.CookieName = $"{CookieDefaults.Prefix}{CookieDefaults.CultureCookie}";

                // Remove all culture providers so we can only use cookie localization
                // Remove this code if query string and language header providers are also used
                options.RequestCultureProviders.Clear();
                options.RequestCultureProviders.Add(cookieProvider);
            });
        }

        /// <summary>
        /// Registers auto mapper services
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            var mapperConfiguration = new MapperConfiguration(mc =>
            {
                mc.AddProfile<AutoMapperConfig>();
            });

            services.AddSingleton(mapperConfiguration.CreateMapper());
        }

        /// <summary>
        /// Registers database services
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CorvoDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(DatabaseConstants.ConnectionString));
            });

            services.AddScoped<DbContext, CorvoDbContext>();
            services.AddScoped<IRepository, EfRepository>();

            //services.AddScoped<DatabaseInitializr>();
        }

        /// <summary>
        /// Registers all necessary application services
        /// </summary>
        /// <param name="services"></param>
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserModelService, UserModelService>();
            services.AddScoped<ISelectListProviderService, SelectListProviderService>();
        }

        /// <summary>
        /// Registers identity services
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            // Register internal password change filter as singleton
            // Note that if we use IOptionsSnapshot instead of IOptions, we should register this as scoped
            services.AddSingleton<InternalPasswordResetFilter>();

            services.AddIdentity<User, Role>()
                   .AddEntityFrameworkStores<CorvoDbContext>()
                   // Register localized error messages
                   .AddErrorDescriber<LocalizedIdentityErrorDescriber>()
                   .AddDefaultTokenProviders();

            // Configure IdentityOptions from appsettings.json
            services.Configure<IdentityOptions>(configuration);

            // Configure cookie authentication
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = $"{CookieDefaults.Prefix}{CookieDefaults.AuthenticationCookie}";
                options.Cookie.HttpOnly = CookieAuthenticationDefaults.HttpOnly;
                options.Cookie.Expiration = TimeSpan.FromDays(30);
                options.ExpireTimeSpan = TimeSpan.FromDays(3);
                options.AccessDeniedPath = CookieAuthenticationDefaults.AccessDeniedPath;
                options.LoginPath = CookieAuthenticationDefaults.LoginPath;
                options.LogoutPath = CookieAuthenticationDefaults.LogoutPath;
                options.SlidingExpiration = CookieAuthenticationDefaults.SlidingExpiration;
            });
        }

        /// <summary>
        /// Configures antiforgery token
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureAntiforgery(this IServiceCollection services)
        {
            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = $"{CookieDefaults.Prefix}{CookieDefaults.AntiforgeryTokenCookie}";
                options.FormFieldName = "_f";
                options.HeaderName = "X-CRSF-TOKEN";
            });
        }

        /// <summary>
        /// Configures routing
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureRouting(this IServiceCollection services)
        {
            services.AddRouting(options =>
            {
                options.AppendTrailingSlash = true;
                options.LowercaseUrls = true;
            });
        }

        #endregion Methods
    }
}