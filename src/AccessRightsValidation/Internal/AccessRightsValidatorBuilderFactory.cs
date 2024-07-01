namespace AccessRightsValidation.Internal;

internal class AccessRightsValidatorBuilderFactory : IAccessRightsValidatorBuilderFactory
{
    private readonly Dictionary<(Type, Type, Type), ResourceConfiguration> _resourceConfigurations;

    public AccessRightsValidatorBuilderFactory(
        Dictionary<(Type, Type, Type), ResourceConfiguration> resourceConfigurations)
    {
        _resourceConfigurations = resourceConfigurations;
    }

    public IAccessRightsValidatorBuilder<TDescriptor, TAction, TUser> Create<TDescriptor, TAction, TUser>()
        where TDescriptor : IResourceDescriptor<TAction, TUser>
        where TAction : Enum
    {
        if (_resourceConfigurations.TryGetValue(
                (typeof(TDescriptor), typeof(TAction), typeof(TUser)),
                out var configuration) is false)
        {
            throw new Exception();
        }

        return new AccessRightsValidatorBuilder<TDescriptor, TAction, TUser>(
            (ResourceConfiguration<TDescriptor, TAction, TUser>)configuration);
    }
}