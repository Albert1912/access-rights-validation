using AccessRightsValidation.Internal.ActionGuards;

namespace AccessRightsValidation.Internal;

internal class AccessRightsValidator<TDescriptor, TAction, TUser> : IAccessRightsValidator<TDescriptor, TAction, TUser>
    where TDescriptor : IResourceDescriptor<TAction, TUser>
    where TAction : Enum
{
    private readonly Dictionary<Type, List<object>> _customData;
    private readonly IEnumerable<BaseActionGuard<TDescriptor, TAction, TUser>> _actionGuards;

    public AccessRightsValidator(
        Dictionary<Type, List<object>> customData,
        IEnumerable<BaseActionGuard<TDescriptor, TAction, TUser>> actionGuards)
    {
        _customData = customData;
        _actionGuards = actionGuards;
    }

    public async ValueTask<ValidationResult> Validate(TUser user, CancellationToken cancellationToken = default)
    {
        var context = new ValidationContext<TDescriptor, TAction, TUser>(user, _customData);
        var errors = new List<ValidationError>();

        foreach (var actionGuard in _actionGuards)
        {
            var guardResult = await actionGuard.Execute(context, cancellationToken);
            if (guardResult is not null)
            {
                errors.Add(guardResult);
            }
        }

        return new ValidationResult(errors);
    }
}