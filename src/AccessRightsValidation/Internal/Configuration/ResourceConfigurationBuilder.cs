using AccessRightsValidation.Configuration;

namespace AccessRightsValidation.Internal.Configuration;

internal class ResourceConfigurationBuilder<TDescriptor, TAction, TUser>
    : IResourceConfigurationBuilder<TDescriptor, TAction, TUser>
    where TDescriptor : IResourceDescriptor<TAction, TUser>
    where TAction : Enum
{
    private readonly Dictionary<TAction, ResourceActionConfigurationBuilder<TDescriptor, TAction, TUser>>
        _configuredActions = [];

    public IResourceActionConfigurationBuilder<TDescriptor, TAction, TUser> ConfigureAction(TAction action)
    {
        if (_configuredActions.TryGetValue(action, out var actionConfigurationBuilder))
        {
            return actionConfigurationBuilder;
        }

        actionConfigurationBuilder = new ResourceActionConfigurationBuilder<TDescriptor, TAction, TUser>();

        _configuredActions.Add(action, actionConfigurationBuilder);

        return actionConfigurationBuilder;
    }

    public ResourceConfiguration<TDescriptor, TAction, TUser> Build()
    {
        return new ResourceConfiguration<TDescriptor, TAction, TUser>(
            _configuredActions
                .Select(x => new
                {
                    x.Key,
                    ActionConfiguration = x.Value.Build()
                })
                .ToDictionary(x => x.Key, x => x.ActionConfiguration));
    }
}