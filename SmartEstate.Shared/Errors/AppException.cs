namespace SmartEstate.Shared.Errors;

public class AppException : Exception
{
    public AppError Error { get; }

    public int? HttpStatus { get; }

    public AppException(AppError error, int? httpStatus = null, Exception? inner = null)
        : base(error.Message, inner)
    {
        Error = error;
        HttpStatus = httpStatus;
    }
}
