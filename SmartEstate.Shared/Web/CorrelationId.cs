namespace SmartEstate.Shared.Web;

public static class CorrelationId
{
    public const string HeaderName = "x-correlation-id";

    public static string NewId() => Guid.NewGuid().ToString("N");
}
