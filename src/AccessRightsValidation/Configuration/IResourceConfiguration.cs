namespace AccessRightsValidation.Configuration;

public interface IResourceConfiguration<TDescriptor, TAction, in TUser>
    where TDescriptor : IResourceDescriptor<TAction, TUser>
    where TAction : Enum
{
    public void Configure(IResourceConfigurationBuilder<TDescriptor, TAction, TUser> builder);
}