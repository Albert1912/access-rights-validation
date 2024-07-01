namespace AccessRightsValidation.ExampleData.Resources;

public class Post
{
    public required Guid Id { get; init; }
    public required Guid Owner { get; init; }
}