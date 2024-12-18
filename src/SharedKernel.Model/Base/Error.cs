using SharedKernel.Model.Enum;

namespace SharedKernel.Model.Base;

public class Error(string code, string description, ErrorType type)
{
    public string Code { get; } = code;

    public string Description { get; } = description;

    public ErrorType Type { get; } = type;

    public static readonly Error None = new(
        string.Empty, string.Empty, ErrorType.Failure);

    public static readonly Error NullValue = new(
        "General.Null",
        "Null value was provided",
        ErrorType.Failure);

    public static Error Failure(string code = "Error.Failure", string description = "An error occurred") =>
        new(code, description, ErrorType.Failure);

    public static Error NotFound(string code = "Error.NotFound", string description = "The requested resource was not found") =>
        new(code, description, ErrorType.NotFound);

    public static Error Problem(string code = "Error.Problem", string description = "An unexpected problem occurred") =>
        new(code, description, ErrorType.Problem);

    public static Error Conflict(string code = "Error.Conflict", string description = "A conflict occurred") =>
        new(code, description, ErrorType.Conflict);
}

public class ValidationError(
    Error[] errors
) : Error("Error.Validation", "One or more validation errors occurred", ErrorType.Validation)
{
    public Error[] Errors { get; } = errors;
    
    public static ValidationError FromResults(IEnumerable<Result> results) =>
        new(results.Where(r => r.IsFailure).Select(r => r.Error).ToArray());
}