using AccessRightsValidation.Internal.Configuration;

namespace AccessRightsValidation.Internal;

internal class AccessRightsValidatorBuilder<TDescriptor, TAction, TUser>
    : IAccessRightsValidatorBuilder<TDescriptor, TAction, TUser>
    where TDescriptor : IResourceDescriptor<TAction, TUser>
    where TAction : Enum
{
    private readonly Dictionary<Type, List<object>> _customData = [];
    private readonly ResourceConfiguration<TDescriptor, TAction, TUser> _resourceConfiguration;

    private ActionConfiguration<TDescriptor, TAction, TUser>? _validateForAction;

    public AccessRightsValidatorBuilder(ResourceConfiguration<TDescriptor, TAction, TUser> resourceConfiguration)
    {
        _resourceConfiguration = resourceConfiguration;
    }

    public IAccessRightsValidatorBuilder<TDescriptor, TAction, TUser> SetCustomData<TData>(TData data)
        where TData : class
    {
        if (_customData.TryGetValue(typeof(TData), out var collection) is false)
        {
            collection = new List<object>();
            _customData.Add(typeof(TData), collection);
        }

        collection.Add(data);

        return this;
    }

    public IAccessRightsValidatorBuilder<TDescriptor, TAction, TUser> ForAction(TAction action)
    {
        if (_resourceConfiguration.ConfiguredActions.ContainsKey(action) is false)
        {
            throw new Exception();
        }

        _validateForAction ??= _resourceConfiguration.ConfiguredActions[action];

        return this;
    }

    public IAccessRightsValidator<TDescriptor, TAction, TUser> Build()
    {
        if (_validateForAction is null)
        {
            throw new Exception();
        }

        if (_validateForAction
            .RequiredCustomDataTypes
            .Any(customDataType => _customData.ContainsKey(customDataType) is false))
        {
            throw new Exception();
        }

        return new AccessRightsValidator<TDescriptor, TAction, TUser>(
            _customData
                .Where(x => _validateForAction.RequiredCustomDataTypes.Contains(x.Key))
                .Select(x => new KeyValuePair<Type, List<object>>(x.Key, x.Value.ToList()))
                .ToDictionary(),
            _validateForAction.ActionGuards);
    }
}