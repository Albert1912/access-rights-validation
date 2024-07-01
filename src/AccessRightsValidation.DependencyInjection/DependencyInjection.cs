using System.Reflection;
using AccessRightsValidation.Configuration;
using AccessRightsValidation.Internal;
using AccessRightsValidation.Internal.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AccessRightsValidation.DependencyInjection;

public static class DependencyInjection
{
    private enum MockAction;

    private record MockResourceDescriptor : IResourceDescriptor<MockAction, object>;

    private static MethodInfo? _configureMethod;

    private static MethodInfo ApplyConfigurationMethod(Type resourceDescriptor, Type actionType, Type userType) =>
        (_configureMethod ??=
            new Func<IResourceConfiguration<MockResourceDescriptor, MockAction, object>, ResourceConfiguration>(
                    ApplyConfiguration)
                .GetMethodInfo()
                .GetGenericMethodDefinition())
        .MakeGenericMethod(resourceDescriptor, actionType, userType);

    public static IServiceCollection AddAccessRightsValidation(
        this IServiceCollection services,
        Action<AccessRightsValidationConfigurationOptions> configure)
    {
        var options = new AccessRightsValidationConfigurationOptions();
        configure(options);

        return services
            .ConfigureResources(options);
    }

    private static IServiceCollection ConfigureResources(
        this IServiceCollection services,
        AccessRightsValidationConfigurationOptions options)
    {
        var baseResourceDescriptorType = typeof(IResourceDescriptor<,>);
        var baseResourceConfigurationType = typeof(IResourceConfiguration<,,>);

        var resourceDescriptorTypes = new HashSet<Type>();
        var resourceConfigurationTypes = new Dictionary<Type, TypeInfo>();

        foreach (var assembly in options.ScanResourceDescriptorsAssemblies)
        {
            foreach (var resourceDescriptorType in assembly
                         .DefinedTypes
                         .Where(t => t.IsAbstract is false
                                     && t
                                         .ImplementedInterfaces
                                         .Any(i => i.IsGenericType
                                                   && i.GetGenericTypeDefinition() == baseResourceDescriptorType)))
            {
                resourceDescriptorTypes.Add(resourceDescriptorType);
            }
        }

        foreach (var assembly in options.ScanResourceConfigurationAssemblies)
        {
            foreach (var resourceConfigurationType in assembly
                         .DefinedTypes
                         .Where(t => t.IsAbstract is false
                                     && t
                                         .ImplementedInterfaces
                                         .Any(i => i.IsGenericType
                                                   && i.GetGenericTypeDefinition() == baseResourceConfigurationType)))
            {
                var configurationInterface = resourceConfigurationType
                    .ImplementedInterfaces
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == baseResourceConfigurationType);

                var resourceDescriptorType = configurationInterface.GetGenericArguments()[0];

                resourceConfigurationTypes.Add(resourceDescriptorType, resourceConfigurationType);
            }
        }

        var resourceConfigurations = new Dictionary<(Type, Type, Type), ResourceConfiguration>();

        foreach (var resourceDescriptorType in resourceDescriptorTypes)
        {
            var configurationType = resourceConfigurationTypes[resourceDescriptorType];
            var configurationInterface = configurationType
                .ImplementedInterfaces
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == baseResourceConfigurationType);

            var actionType = configurationInterface.GetGenericArguments()[1];
            var userType = configurationInterface.GetGenericArguments()[2];

            var configuration = Activator.CreateInstance(configurationType)!;

            var resourceConfiguration = ApplyConfigurationMethod(resourceDescriptorType, actionType, userType)
                .Invoke(null, [configuration])!;

            resourceConfigurations.Add(
                (resourceDescriptorType, actionType, userType),
                (ResourceConfiguration)resourceConfiguration);
        }

        var factory = new AccessRightsValidatorBuilderFactory(resourceConfigurations);

        services.AddSingleton<IAccessRightsValidatorBuilderFactory>(factory);

        return services;
    }

    private static ResourceConfiguration ApplyConfiguration<TDescriptor, TAction, TUser>(
        IResourceConfiguration<TDescriptor, TAction, TUser> configuration)
        where TDescriptor : IResourceDescriptor<TAction, TUser>
        where TAction : Enum
    {
        var builder = new ResourceConfigurationBuilder<TDescriptor, TAction, TUser>();

        configuration.Configure(builder);

        return builder.Build();
    }
}