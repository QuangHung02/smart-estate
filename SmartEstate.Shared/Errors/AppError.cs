namespace SmartEstate.Shared.Errors;

public sealed record AppError(
    string Code,
    string Message,
    IReadOnlyDictionary<string, object?>? Meta = null
);
