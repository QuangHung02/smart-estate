using SmartEstate.Shared.Errors;

namespace SmartEstate.Shared.Results;

public class Result
{
    public bool IsSuccess { get; }
    public AppError? Error { get; }

    protected Result(bool isSuccess, AppError? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Ok() => new(true, null);

    public static Result Fail(string code, string message, IReadOnlyDictionary<string, object?>? meta = null)
        => new(false, new AppError(code, message, meta));

    public static Result Fail(AppError error) => new(false, error);
}
