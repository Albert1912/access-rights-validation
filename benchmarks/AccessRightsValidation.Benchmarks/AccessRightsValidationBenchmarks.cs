using AccessRightsValidation.DependencyInjection;
using AccessRightsValidation.ExampleData;
using AccessRightsValidation.ExampleData.ResourceDescriptors;
using AccessRightsValidation.ExampleData.Resources;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace AccessRightsValidation.Benchmarks;

[MemoryDiagnoser]
public class AccessRightsValidationBenchmarks
{
    private readonly IAccessRightsValidatorBuilderFactory _builderFactory;
    private readonly User _admin;
    private readonly IAccessRightsValidator<UserDescriptor, ResourceAction, User> _validator;

    public AccessRightsValidationBenchmarks()
    {
        var serviceProvider = new ServiceCollection()
            .AddAccessRightsValidation(options =>
                options
                    .AddResourceDescriptorsAssembly(typeof(Marker).Assembly)
                    .AddResourceConfigurationsAssembly(typeof(Marker).Assembly))
            .BuildServiceProvider();

        _builderFactory = serviceProvider.GetRequiredService<IAccessRightsValidatorBuilderFactory>();
        _admin = new User
        {
            Id = Guid.NewGuid(),
            Role = Role.Admin
        };

        _validator = _builderFactory
            .Create<UserDescriptor, ResourceAction, User>()
            .ForAction(ResourceAction.Create)
            .Build();
    }

    [Benchmark]
    public ValueTask<ValidationResult> CreateValidatorAndValidate()
    {
        var validator = _builderFactory
            .Create<UserDescriptor, ResourceAction, User>()
            .ForAction(ResourceAction.Create)
            .Build();

        return validator.Validate(_admin);
    }

    [Benchmark]
    public ValueTask<ValidationResult> Validate()
    {
        return _validator.Validate(_admin);
    }
}