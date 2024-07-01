namespace AccessRightsValidation;

public interface IValidationContext<TDescriptor, TAction, out TUser>
    where TDescriptor : IResourceDescriptor<TAction, TUser>
{
    TUser Actor { get; }
    TData GetCustomData<TData>();
}