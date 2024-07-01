namespace AccessRightsValidation;

public interface IAccessRightsValidator<TDescriptor, TAction, in TUser>
    where TDescriptor : IResourceDescriptor<TAction, TUser>
    where TAction : Enum
{
    ValueTask<ValidationResult> Validate(TUser user, CancellationToken cancellationToken = default);
}