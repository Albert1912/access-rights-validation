using AccessRightsValidation.Configuration;
using AccessRightsValidation.ExampleData.ResourceDescriptors;
using AccessRightsValidation.ExampleData.Resources;

namespace AccessRightsValidation.ExampleData.ResourceConfigurations;

public class PostConfiguration : IResourceConfiguration<PostDescriptor, ResourceAction, User>
{
    public void Configure(IResourceConfigurationBuilder<PostDescriptor, ResourceAction, User> builder)
    {
        builder
            .ConfigureAction(ResourceAction.Create)
            .AddGuard(context => context.Actor.Role != Role.Viewer);

        builder
            .ConfigureAction(ResourceAction.Read)
            .AddGuard(_ => true);

        builder
            .ConfigureAction(ResourceAction.Update)
            .RequireCustomData<Post>()
            .AddGuard(context => context.GetCustomData<Post>().Owner == context.Actor.Id);

        builder
            .ConfigureAction(ResourceAction.Delete)
            .RequireCustomData<Post>()
            .AddGuard(context => context.GetCustomData<Post>().Owner == context.Actor.Id);
    }
}