namespace AccessRightsValidation.Internal.ActionGuards;

internal abstract class BaseActionGuard<TDescriptor, TAction, TUser>
    where TDescriptor : IResourceDescriptor<TAction, TUser>
    where TAction : Enum
{
    public abstract Task<bool> Execute(IValidationContext<TDescriptor, TAction, TUser> validationContext);
}