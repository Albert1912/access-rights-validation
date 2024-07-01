namespace AccessRightsValidation.Internal;

internal class ValidationContext<TDescriptor, TAction, TUser> : IValidationContext<TDescriptor, TAction, TUser>
    where TDescriptor : IResourceDescriptor<TAction, TUser>
    where TAction : Enum
{
    private readonly Dictionary<Type, List<object>> _customData;

    public TUser Actor { get; }

    public ValidationContext(TUser actor, Dictionary<Type, List<object>> customData)
    {
        Actor = actor;
        _customData = customData;
    }

    public TData GetCustomData<TData>()
    {
        return (TData)_customData[typeof(TData)][^1];
    }

    public IEnumerable<TData> GetAllCustomData<TData>()
    {
        return _customData[typeof(TData)].Cast<TData>();
    }
}