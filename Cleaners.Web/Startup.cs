using Cleaners.Web.Extensions;
using Cleaners.Web.Infrastructure.Alerts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cleaners.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.ConfigureAppSettings(Configuration);

            services.ConfigureRazorViewEngine();

            services.ConfigureLocalization();

            services.ConfigureAutoMapper();

            services.ConfigureDatabase(Configuration);

            services.RegisterServices();

            services.ConfigureAntiforgery();

            // Configures identity for authentication and authorization
            services.ConfigureIdentity(Configuration);

            services.AddScoped<TempDataAlertManager>();

            services.ConfigureMiniProfiler();

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
            app.UseStaticFiles();
            app.UseAuthentication();
            app.ConfigureLocalization();

            // Since mini-profiler is lightweight we can leave it ON for all evironment types
            app.UseMiniProfiler();

            app.UseMvcWithDefaultRoute();
        }
    }
}