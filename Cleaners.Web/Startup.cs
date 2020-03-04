using Cleaners.Web.Extensions;
using Cleaners.Web.Infrastructure.Routing;
using Cleaners.Web.Infrastructure.Stuntman;
using Corvo.AspNetCore.Mvc.Middleware.Claims;
using Corvo.AspNetCore.Mvc.Middleware.RegisteredServices;
using Corvo.AspNetCore.Mvc.UI.Alerts;
using Corvo.AspNetCore.Mvc.UI.Navigation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RimDev.Stuntman.Core;
using System.Collections.Generic;

namespace Cleaners.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.ConfigureAppSettings(Configuration);

            services.AddRouting(conf =>
            {
                conf.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
            });

            services.ConfigureRazorViewEngine();

            //services.ConfigureLocalization();

            services.ConfigureAutoMapper();

            services.ConfigureDatabase(Configuration);

            services.RegisterApplicationServices();

            services.ConfigureAntiforgery();

            // Configures identity for authentication and authorization
            services.ConfigureIdentity(Configuration);

            // Override authentication schema for stuntman in development mode
            if (HostingEnvironment.IsDevelopment())
            {
                services.ConfigureStuntman();
            }

            services.AddTempDataAlertManager();

            services.ConfigureMiniProfiler();

            services.AddSingleton<INavigationMenuManager, NavigationMenuManager>(factory =>
            {
                var menuManager = new NavigationMenuManager();

                return menuManager;
            });

            // Register file provider options from appsettings
            //services.Configure<CorvoFileProviderOptions>(Configuration.GetSection(AppSettingsSectionNames.CorvoFileProviderOptions));
            //services.AddCorvoFileProvider();

            //services.AddScoped<ITagHelperComponent, MetaTagHelperComponent>();
            //services.AddScoped<ITagHelperComponent, NavTagHelperComponent>();

            services.ConfigureMvc();

            services.AddSingleton<RegisteredServicesConfig>(config =>
            {
                return new RegisteredServicesConfig
                {
                    Services = new List<ServiceDescriptor>(services)
                };
            });

            //services.AddRouteAnalyzer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment host)
        {
            if (host.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseShowRegisteredServices();
                app.UseStuntman();
                app.UseClaimsDisplay();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/error/{0}");
                app.UseHsts();
            }

            // Serve files from wwwroot directory
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            // Enable endpoint routing
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.ConfigureLocalization();

            // Since mini-profiler is lightweight we can leave it ON in all evironments
            // In that case, make sure to authorize it.
            app.UseMiniProfiler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}