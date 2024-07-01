namespace AccessRightsValidation.Internal;

internal class ValidationContext<TDescriptor, TAction, TUser> : IValidationContext<TDescriptor, TAction, TUser>
    where TDescriptor : IResourceDescriptor<TAction, TUser>
    where TAction : Enum
{
    private readonly Dictionary<Type, object> _customData;

    public TUser Actor { get; }

    public ValidationContext(TUser actor, Dictionary<Type, object> customData)
    {
        Actor = actor;
        _customData = customData;
    }

    public TData GetCustomData<TData>()
    {
        return (TData)_customData[typeof(TData)];
    }
}