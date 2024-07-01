namespace AccessRightsValidation;

public interface IAccessRightsValidatorBuilder<TDescriptor, TAction, in TUser>
    where TDescriptor : IResourceDescriptor<TAction, TUser>
    where TAction : Enum
{
    IAccessRightsValidatorBuilder<TDescriptor, TAction, TUser> SetCustomData<TData>(TData data) where TData : class;

    IAccessRightsValidatorBuilder<TDescriptor, TAction, TUser> ForAction(TAction action);

    IAccessRightsValidator<TDescriptor, TAction, TUser> Build();
}