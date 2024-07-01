namespace AccessRightsValidation.Internal.ActionGuards;

internal class ActionAsyncGuard<TDescriptor, TAction, TUser> : BaseActionGuard<TDescriptor, TAction, TUser>
    where TDescriptor : IResourceDescriptor<TAction, TUser>
    where TAction : Enum
{
    private readonly Func<IValidationContext<TDescriptor, TAction, TUser>, Task<bool>> _guard;

    public ActionAsyncGuard(Func<IValidationContext<TDescriptor, TAction, TUser>, Task<bool>> guard)
    {
        _guard = guard;
    }

    public override Task<bool> Execute(IValidationContext<TDescriptor, TAction, TUser> validationContext)
    {
        return _guard(validationContext);
    }
}