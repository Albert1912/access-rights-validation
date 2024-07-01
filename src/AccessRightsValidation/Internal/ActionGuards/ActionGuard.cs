namespace AccessRightsValidation.Internal.ActionGuards;

internal class ActionGuard<TDescriptor, TAction, TUser> : BaseActionGuard<TDescriptor, TAction, TUser>
    where TDescriptor : IResourceDescriptor<TAction, TUser>
    where TAction : Enum
{
    private readonly Predicate<IValidationContext<TDescriptor, TAction, TUser>> _guard;

    public ActionGuard(
        Predicate<IValidationContext<TDescriptor, TAction, TUser>> guard,
        Func<IValidationContext<TDescriptor, TAction, TUser>, string>? onValidationFailed)
        : base(onValidationFailed)
    {
        _guard = guard;
    }

    protected override Task<bool> ExecuteGuard(
        IValidationContext<TDescriptor, TAction, TUser> validationContext,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_guard(validationContext));
    }
}