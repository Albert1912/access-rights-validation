namespace AccessRightsValidation.ExampleData.Resources;

public class User
{
    public required Guid Id { get; init; }
    public required Role Role { get; set; }
}