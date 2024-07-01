using AccessRightsValidation.Configuration;
using AccessRightsValidation.Internal.ActionGuards;

namespace AccessRightsValidation.Internal.Configuration;

internal class ResourceActionConfigurationBuilder<TDescriptor, TAction, TUser>
    : IResourceActionConfigurationBuilder<TDescriptor, TAction, TUser>
    where TDescriptor : IResourceDescriptor<TAction, TUser>
    where TAction : Enum
{
    private readonly List<BaseActionGuard<TDescriptor, TAction, TUser>> _guards = [];
    private readonly HashSet<Type> _requiredCustomDataTypes = [];

    public IResourceActionConfigurationBuilder<TDescriptor, TAction, TUser> RequireCustomData<TData>()
    {
        _requiredCustomDataTypes.Add(typeof(TData));

        return this;
    }

    public IResourceActionConfigurationBuilder<TDescriptor, TAction, TUser> AddGuard(
        Predicate<IValidationContext<TDescriptor, TAction, TUser>> guard,
        Func<IValidationContext<TDescriptor, TAction, TUser>, string>? onValidationFailed = null)
    {
        _guards.Add(new ActionGuard<TDescriptor, TAction, TUser>(guard, onValidationFailed));

        return this;
    }

    public IResourceActionConfigurationBuilder<TDescriptor, TAction, TUser> AddGuard(
        Func<IValidationContext<TDescriptor, TAction, TUser>, CancellationToken, Task<bool>> guard,
        Func<IValidationContext<TDescriptor, TAction, TUser>, string>? onValidationFailed = null)
    {
        _guards.Add(new ActionAsyncGuard<TDescriptor, TAction, TUser>(guard, onValidationFailed));

        return this;
    }

    public ActionConfiguration<TDescriptor, TAction, TUser> Build()
    {
        return new ActionConfiguration<TDescriptor, TAction, TUser>(_guards, _requiredCustomDataTypes);
    }
}