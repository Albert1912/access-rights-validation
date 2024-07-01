namespace AccessRightsValidation.Configuration;

public interface IResourceActionConfigurationBuilder<TDescriptor, TAction, out TUser>
    where TDescriptor : IResourceDescriptor<TAction, TUser>
    where TAction : Enum
{
    IResourceActionConfigurationBuilder<TDescriptor, TAction, TUser> RequireCustomData<TData>();

    IResourceActionConfigurationBuilder<TDescriptor, TAction, TUser> AddGuard(
        Predicate<IValidationContext<TDescriptor, TAction, TUser>> guard,
        Func<IValidationContext<TDescriptor, TAction, TUser>, string>? onValidationFailed = null);

    IResourceActionConfigurationBuilder<TDescriptor, TAction, TUser> AddGuard(
        Func<IValidationContext<TDescriptor, TAction, TUser>, CancellationToken, Task<bool>> guard,
        Func<IValidationContext<TDescriptor, TAction, TUser>, string>? onValidationFailed = null);
}