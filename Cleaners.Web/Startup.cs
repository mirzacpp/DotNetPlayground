using Cleaners.Web.Constants;
using Cleaners.Web.Extensions;
using Cleaners.Web.Infrastructure.Files;
using Cleaners.Web.Infrastructure.Stuntman;
using Cleaners.Web.Services;
using Cleaners.Web.TagHelpers.Nav;
using Corvo.AspNetCore.Mvc.UI.Alerts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RimDev.Stuntman.Core;

namespace Cleaners.Web
{
    public class Startup
    {
        public static readonly StuntmanOptions StuntmanOptions = new StuntmanOptions();

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.ConfigureAppSettings(Configuration);

            services.ConfigureRazorViewEngine();

            services.ConfigureLocalization();

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

            services.AddScoped<ICsvFileService, CsvFileService>();

            // Register file provider options from appsettings
            services.Configure<CorvoFileProviderOptions>(Configuration.GetSection(AppSettingsSectionNames.CorvoFileProviderOptions));
            services.AddCorvoFileProvider();

            //services.AddScoped<ITagHelperComponent, MetaTagHelperComponent>();
            services.AddScoped<ITagHelperComponent, NavTagHelperComponent>();

            services.ConfigureMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/error/{0}");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            // Serve files from wwwroot directory
            app.UseStaticFiles();

            app.UseAuthentication();

            // Register stuntman middleware in development environment
            if (env.IsDevelopment())
            {
                app.UseStuntman();
            }

            app.ConfigureLocalization();

            // Since mini-profiler is lightweight we can leave it ON for all evironment types
            app.UseMiniProfiler();

            app.UseMvcWithDefaultRoute();
        }
    }
}