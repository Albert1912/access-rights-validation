namespace AccessRightsValidation;

public class ValidationResult
{
    private readonly List<ValidationError> _errors;

    public bool IsSuccess => _errors.Count == 0;
    public IReadOnlyList<ValidationError> Errors => _errors;

    public ValidationResult(IEnumerable<ValidationError> errors)
    {
        _errors = errors.ToList();
    }
}