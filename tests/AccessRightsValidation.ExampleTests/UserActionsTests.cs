using AccessRightsValidation.DependencyInjection;
using AccessRightsValidation.ExampleData;
using AccessRightsValidation.ExampleData.ResourceDescriptors;
using AccessRightsValidation.ExampleData.Resources;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace AccessRightsValidation.ExampleTests;

public class UserActionsTests
{
    private readonly IAccessRightsValidatorBuilderFactory _builderFactory;

    public UserActionsTests()
    {
        var serviceProvider = new ServiceCollection()
            .AddAccessRightsValidation(options =>
                options
                    .AddResourceDescriptorsAssembly(typeof(Marker).Assembly)
                    .AddResourceConfigurationsAssembly(typeof(Marker).Assembly))
            .BuildServiceProvider();

        _builderFactory = serviceProvider.GetRequiredService<IAccessRightsValidatorBuilderFactory>();
    }

    [Fact]
    public async Task UserWithRoleAdmin_Should_BeAbleToCreateUsers()
    {
        // arrange
        var admin = new User
        {
            Id = Guid.NewGuid(),
            Role = Role.Admin
        };

        var validator = _builderFactory
            .Create<UserDescriptor, ResourceAction, User>()
            .ForAction(ResourceAction.Create)
            .Build();

        // act
        var validationResult = await validator.Validate(admin);

        //assert
        validationResult.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task UserWithoutRoleAdmin_Should_NotBeAbleToCreateUsers()
    {
        // arrange
        var editor = new User
        {
            Id = Guid.NewGuid(),
            Role = Role.Editor
        };

        var validator = _builderFactory
            .Create<UserDescriptor, ResourceAction, User>()
            .ForAction(ResourceAction.Create)
            .Build();

        // act
        var validationResult = await validator.Validate(editor);

        //assert
        validationResult.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task AllUsers_Should_BeAbleToReadOtherUsers()
    {
        // arrange
        var admin = new User
        {
            Id = Guid.NewGuid(),
            Role = Role.Admin
        };

        var editor = new User
        {
            Id = Guid.NewGuid(),
            Role = Role.Editor
        };

        var viewer = new User
        {
            Id = Guid.NewGuid(),
            Role = Role.Viewer
        };

        var validator = _builderFactory
            .Create<UserDescriptor, ResourceAction, User>()
            .ForAction(ResourceAction.Read)
            .Build();

        // act
        var validationResultAdmin = await validator.Validate(admin);
        var validationResultEditor = await validator.Validate(editor);
        var validationResultViewer = await validator.Validate(viewer);

        //assert
        validationResultAdmin.IsSuccess.Should().BeTrue();
        validationResultEditor.IsSuccess.Should().BeTrue();
        validationResultViewer.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task User_Should_BeAbleToUpdateOnlyYourself()
    {
        // arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Role = Role.Editor
        };

        var otherUser = new User
        {
            Id = Guid.NewGuid(),
            Role = Role.Viewer
        };

        var validator = _builderFactory
            .Create<UserDescriptor, ResourceAction, User>()
            .ForAction(ResourceAction.Update)
            .SetCustomData(user)
            .Build();

        // act
        var updateByYourselfResult = await validator.Validate(user);
        var updateByOtherResult = await validator.Validate(otherUser);

        //assert
        updateByYourselfResult.IsSuccess.Should().BeTrue();
        updateByOtherResult.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void UpdateUser_Should_RequireCustomData()
    {
        // arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Role = Role.Editor
        };

        // act
        var buildValidatorWithCustomData = () => _builderFactory
            .Create<UserDescriptor, ResourceAction, User>()
            .ForAction(ResourceAction.Update)
            .SetCustomData(user)
            .Build();

        var buildValidatorWithoutCustomData = () => _builderFactory
            .Create<UserDescriptor, ResourceAction, User>()
            .ForAction(ResourceAction.Update)
            .Build();

        //assert
        buildValidatorWithCustomData.Should().NotThrow();
        buildValidatorWithoutCustomData.Should().Throw<Exception>();
    }

    [Fact]
    public async Task OnlyUserWithRoleAdmin_Should_BeAbleToDeleteUsers()
    {
        // arrange
        var admin = new User
        {
            Id = Guid.NewGuid(),
            Role = Role.Admin
        };

        var editor = new User
        {
            Id = Guid.NewGuid(),
            Role = Role.Editor
        };

        var validator = _builderFactory
            .Create<UserDescriptor, ResourceAction, User>()
            .ForAction(ResourceAction.Delete)
            .Build();

        // act
        var adminValidationResult = await validator.Validate(admin);
        var editorValidationResult = await validator.Validate(editor);

        //assert
        adminValidationResult.IsSuccess.Should().BeTrue();
        editorValidationResult.IsSuccess.Should().BeFalse();
    }
}