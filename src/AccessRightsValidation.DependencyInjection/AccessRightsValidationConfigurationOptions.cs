using System.Reflection;

namespace AccessRightsValidation.DependencyInjection;

public class AccessRightsValidationConfigurationOptions
{
    internal List<Assembly> ScanResourceDescriptorsAssemblies { get; } = [];
    internal List<Assembly> ScanResourceConfigurationAssemblies { get; } = [];

    internal AccessRightsValidationConfigurationOptions()
    {
    }

    public AccessRightsValidationConfigurationOptions AddResourceDescriptorsAssembly(Assembly assembly)
    {
        ScanResourceDescriptorsAssemblies.Add(assembly);

        return this;
    }

    public AccessRightsValidationConfigurationOptions AddResourceConfigurationsAssembly(Assembly assembly)
    {
        ScanResourceConfigurationAssemblies.Add(assembly);

        return this;
    }
}