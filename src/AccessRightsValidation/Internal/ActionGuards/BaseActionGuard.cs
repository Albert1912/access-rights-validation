namespace AccessRightsValidation.Internal.ActionGuards;

internal abstract class BaseActionGuard<TDescriptor, TAction, TUser>
    where TDescriptor : IResourceDescriptor<TAction, TUser>
    where TAction : Enum
{
    private readonly Func<IValidationContext<TDescriptor, TAction, TUser>, string> _onValidationFailed;

    protected BaseActionGuard(
        Func<IValidationContext<TDescriptor, TAction, TUser>, string>? onValidationFailed)
    {
        _onValidationFailed = onValidationFailed ?? (_ => string.Empty);
    }

    public async Task<ValidationError?> Execute(
        IValidationContext<TDescriptor, TAction, TUser> validationContext,
        CancellationToken cancellationToken = default)
    {
        var guardIsValid = await ExecuteGuard(validationContext, cancellationToken);
        return guardIsValid
            ? null
            : new ValidationError(_onValidationFailed(validationContext));
    }

    protected abstract Task<bool> ExecuteGuard(
        IValidationContext<TDescriptor, TAction, TUser> validationContext,
        CancellationToken cancellationToken = default);
}