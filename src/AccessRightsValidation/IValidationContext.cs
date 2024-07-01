namespace AccessRightsValidation;

public interface IValidationContext<TDescriptor, TAction, out TUser>
    where TDescriptor : IResourceDescriptor<TAction, TUser>
    where TAction : Enum
{
    TUser Actor { get; }
    TData GetCustomData<TData>();
}