namespace AccessRightsValidation.Configuration;

public interface IResourceConfigurationBuilder<TDescriptor, TAction, out TUser>
    where TDescriptor : IResourceDescriptor<TAction, TUser>
    where TAction : Enum
{
    IResourceActionConfigurationBuilder<TDescriptor, TAction, TUser> ConfigureAction(TAction action);
}