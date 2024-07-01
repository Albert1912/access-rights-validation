using AccessRightsValidation.Internal.ActionGuards;

namespace AccessRightsValidation.Internal.Configuration;

internal class ActionConfiguration<TDescriptor, TAction, TUser>
    where TDescriptor : IResourceDescriptor<TAction, TUser>
    where TAction : Enum
{
    public IEnumerable<BaseActionGuard<TDescriptor, TAction, TUser>> ActionGuards { get; }
    public HashSet<Type> RequiredCustomDataTypes { get; }

    public ActionConfiguration(
        IEnumerable<BaseActionGuard<TDescriptor, TAction, TUser>> actionGuards,
        HashSet<Type> requiredCustomDataTypes)
    {
        ActionGuards = actionGuards;
        RequiredCustomDataTypes = requiredCustomDataTypes;
    }
}