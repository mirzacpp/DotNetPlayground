using Microsoft.Extensions.DependencyInjection;
using RimDev.Stuntman.Core;
using System;

namespace Cleaners.Web.Infrastructure.Stuntman
{
    /// <summary>
    /// Configuration class for <see cref="StuntmanOptions"/>
    /// </summary>
    public static class StuntmanServiceCollectionExtensions
    {
        public static void ConfigureStuntman(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var options = new StuntmanOptions();
            options.SetUserPickerAlignment(StuntmanAlignment.Right);
            
            // TODO: Create base user factory class used for database seed
            options.AddUser(new StuntmanUser("2", "zadok@ito.ba")
                .AddName("zadok@ito.ba")
                .AddRole("Admin")
                .AddRole("Support"));

            options.AddUser(new StuntmanUser("1", "mirza@ito.ba")
               .AddName("mirza@ito.ba")
               .AddRole("Admin"));

            // Register options as singleton
            services.AddSingleton(options);

            // Override authentication schema
            services.AddAuthentication(config =>
            {
                config.DefaultSignInScheme = RimDev.Stuntman.Core.Constants.StuntmanAuthenticationType;
                config.DefaultScheme = RimDev.Stuntman.Core.Constants.StuntmanAuthenticationType;
                config.DefaultAuthenticateScheme = RimDev.Stuntman.Core.Constants.StuntmanAuthenticationType;
                config.DefaultChallengeScheme = RimDev.Stuntman.Core.Constants.StuntmanAuthenticationType;
            });

            services.AddStuntman(options);
        }
    }
}