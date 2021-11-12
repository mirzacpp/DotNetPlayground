using System;
using Weakling.WebUI.Infrastructure.Features;

/// <summary>
/// Credits to <see cref="https://github.com/OdeToCode/AddFeatureFolders"/>
/// </summary>
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     Use feature folders with custom options
        /// </summary>
        public static IMvcBuilder AddFeatureFolders(this IMvcBuilder builder, FeatureFolderOptions options)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (options is null)
            {
                throw new ArgumentException(nameof(options));
            }

            var expander = new FeatureViewLocationExpander(options);

            builder.AddMvcOptions(o => o.Conventions.Add(new FeatureControllerModelConvention(options)))
                .AddRazorOptions(o =>
                {
                    o.ViewLocationFormats.Clear();
                    o.ViewLocationFormats.Add(options.FeatureNamePlaceholder + @"\{0}.cshtml");
                    o.ViewLocationFormats.Add(options.FeatureFolderName + @"\Shared\{0}.cshtml");
                    o.ViewLocationFormats.Add(options.DefaultViewLocation);

                    o.ViewLocationExpanders.Add(expander);
                });

            return builder;
        }

        /// <summary>
        ///     Use areas with feature folders and custom options
        /// </summary>
        /// <remarks>
        /// <see cref="AddFeatureFolders(IMvcBuilder)"/> must be invoked before this method.
        /// </remarks>
        public static IMvcBuilder AddAreaFeatureFolders(this IMvcBuilder builder, AreaFeatureFolderOptions options)
        {
            builder.AddRazorOptions(o =>
            {
                o.AreaViewLocationFormats.Clear();
                o.AreaViewLocationFormats.Add(options.DefaultAreaViewLocation);
                o.AreaViewLocationFormats.Add(options.AreaFolderName + @"\{2}\{1}\{0}.cshtml");
                o.AreaViewLocationFormats.Add(options.AreaFolderName + @"\{2}\Shared\{0}.cshtml");
                o.AreaViewLocationFormats.Add(options.AreaFolderName + @"\Shared\{0}.cshtml");
                o.AreaViewLocationFormats.Add(options.FeatureFolderName + @"\Shared\{0}.cshtml");
            });

            return builder;
        }

        /// <summary>
        ///     Use feature folders with the default options. Controllers and view will be located
        ///     under a folder named Features. Shared views are located in Features\Shared.
        /// </summary>
        public static IMvcBuilder AddFeatureFolders(this IMvcBuilder builder) =>
            builder.AddFeatureFolders(new FeatureFolderOptions());

        /// <summary>
        ///     Use areas with feature folders with the default options. Controllers and views will
        ///     be located under a folder named Areas with an area specific folder. Shared views are
        ///     located in Areas\Shared and then in Features\Shared 
        /// </summary>
        public static IMvcBuilder AddAreaFeatureFolders(this IMvcBuilder builder) =>
            AddAreaFeatureFolders(builder, new AreaFeatureFolderOptions());
    }
}