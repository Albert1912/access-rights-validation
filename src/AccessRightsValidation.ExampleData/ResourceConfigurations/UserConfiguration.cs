using AccessRightsValidation.Configuration;
using AccessRightsValidation.ExampleData.ResourceDescriptors;
using AccessRightsValidation.ExampleData.Resources;

namespace AccessRightsValidation.ExampleData.ResourceConfigurations;

public class UserConfiguration : IResourceConfiguration<UserDescriptor, ResourceAction, User>
{
    public void Configure(IResourceConfigurationBuilder<UserDescriptor, ResourceAction, User> builder)
    {
        builder
            .ConfigureAction(ResourceAction.Create)
            .AddGuard(context => context.Actor.Role == Role.Admin);

        builder
            .ConfigureAction(ResourceAction.Read)
            .AddGuard(_ => true);

        builder
            .ConfigureAction(ResourceAction.Update)
            .RequireCustomData<User>()
            .AddGuard(context => context.GetCustomData<User>().Id == context.Actor.Id);

        builder
            .ConfigureAction(ResourceAction.Delete)
            .AddGuard(context => context.Actor.Role == Role.Admin);
    }
}