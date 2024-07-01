namespace AccessRightsValidation.Internal.ActionGuards;

internal class ActionAsyncGuard<TDescriptor, TAction, TUser> : BaseActionGuard<TDescriptor, TAction, TUser>
    where TDescriptor : IResourceDescriptor<TAction, TUser>
    where TAction : Enum
{
    private readonly Func<IValidationContext<TDescriptor, TAction, TUser>, CancellationToken, Task<bool>> _guard;

    public ActionAsyncGuard(
        Func<IValidationContext<TDescriptor, TAction, TUser>, CancellationToken, Task<bool>> guard,
        Func<IValidationContext<TDescriptor, TAction, TUser>, string>? onValidationFailed)
        : base(onValidationFailed)
    {
        _guard = guard;
    }

    protected override Task<bool> ExecuteGuard(
        IValidationContext<TDescriptor, TAction, TUser> validationContext,
        CancellationToken cancellationToken = default)
    {
        return _guard(validationContext, cancellationToken);
    }
}