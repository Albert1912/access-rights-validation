namespace AccessRightsValidation;

public interface IAccessRightsValidatorBuilderFactory
{
    IAccessRightsValidatorBuilder<TDescriptor, TAction, TUser> Create<TDescriptor, TAction, TUser>()
        where TDescriptor : IResourceDescriptor<TAction, TUser>
        where TAction : Enum;
}