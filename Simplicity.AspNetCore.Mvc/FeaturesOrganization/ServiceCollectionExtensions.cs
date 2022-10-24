using Ardalis.GuardClauses;
using Simplicity.AspNetCore.Mvc.FeaturesOrganization;

/// <summary>
/// Credits to <see cref="https://github.com/OdeToCode/AddFeatureFolders"/>
/// </summary>
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Use feature folders with custom options
        /// </summary>
        public static FeaturesBuilder AddFeatureFolders(this IMvcBuilder builder, FeatureFolderOptions options)
        {
            Guard.Against.Null(builder, nameof(builder));
            Guard.Against.Null(options, nameof(options));

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

            return new FeaturesBuilder(builder);
        }

        /// <summary>
        /// Use areas with feature folders and custom options
        /// </summary>
        /// <returns>Returns captured IMvcBuilder instance.</returns>
        public static IMvcBuilder AddAreaFeatureFolders(this FeaturesBuilder featureMvcBuilder, AreaFeatureFolderOptions options)
        {
            Guard.Against.Null(featureMvcBuilder, nameof(featureMvcBuilder));
            Guard.Against.Null(options, nameof(options));

            featureMvcBuilder.MvcBuilder.AddRazorOptions(o =>
            {
                o.AreaViewLocationFormats.Clear();
                o.AreaViewLocationFormats.Add(options.DefaultAreaViewLocation);
                o.AreaViewLocationFormats.Add(options.AreaFolderName + @"\{2}\{1}\{0}.cshtml");
                o.AreaViewLocationFormats.Add(options.AreaFolderName + @"\{2}\Shared\{0}.cshtml");
                o.AreaViewLocationFormats.Add(options.AreaFolderName + @"\Shared\{0}.cshtml");
                o.AreaViewLocationFormats.Add(options.FeatureFolderName + @"\Shared\{0}.cshtml");
            });

            // Note that featureMvcBuilder can be returned in future if there are more extension methods
            return featureMvcBuilder.MvcBuilder;
        }

        /// <summary>
        /// Use feature folders with the default options. Controllers and view will be located
        /// under a folder named Features. Shared views are located in Features\Shared.
        /// </summary>
        public static FeaturesBuilder AddFeatureFolders(this IMvcBuilder builder) =>
            builder.AddFeatureFolders(new FeatureFolderOptions());

        /// <summary>
        /// Use areas with feature folders with the default options. Controllers and views will
        /// be located under a folder named Areas with an area specific folder. Shared views are
        /// located in Areas\Shared and then in Features\Shared
        /// </summary>
        public static IMvcBuilder AddAreaFeatureFolders(this FeaturesBuilder featureMvcBuilder) =>
            AddAreaFeatureFolders(featureMvcBuilder, new AreaFeatureFolderOptions());
    }
}