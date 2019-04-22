using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace Cleaners.Data.Extensions
{
    /// <summary>
    /// ModelBuilder extensions
    /// </summary>
    public static class ModelBuilderExtensions
    {
        #region Methods

        /// <summary>
        /// Applies all entity mappings found in project
        /// </summary>
        /// <param name="builder"></param>
        public static void ApplyEntityConfigurations(this ModelBuilder builder)
        {
            var types = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.GetInterfaces()
                .Any(gi => gi.IsGenericType && gi.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
                .ToList();

            foreach (var type in types)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                builder.ApplyConfiguration(configurationInstance);
            }
        }

        #endregion Methods
    }
}