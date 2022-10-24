using Microsoft.AspNetCore.Mvc.ApplicationModels;

/// <summary>
/// Credits to <see cref="https://github.com/OdeToCode/AddFeatureFolders"/>
/// </summary>
namespace Simplicity.AspNetCore.Mvc.FeaturesOrganization;

public class FeatureControllerModelConvention : IControllerModelConvention
{
    private readonly string _folderName;
    private readonly Func<ControllerModel, string> _nameDerivationStrategy;

    public FeatureControllerModelConvention(FeatureFolderOptions options)
    {
        _folderName = options.FeatureFolderName;
        _nameDerivationStrategy = options.DeriveFeatureFolderNameStrategy ?? DeriveFeatureFolderName;
    }

    public void Apply(ControllerModel controller)
    {
        controller.Properties
            .Add(FeaturesConst.FeaturesKey, _nameDerivationStrategy(controller));
    }

    private string DeriveFeatureFolderName(ControllerModel model)
    {
        var @namespace = model.ControllerType.Namespace;
        var result = @namespace.Split('.')
                               .SkipWhile(s => s != _folderName)
                               .Aggregate("", Path.Combine);

        return result;
    }
}
