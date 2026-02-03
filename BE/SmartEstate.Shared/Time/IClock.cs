namespace SmartEstate.Shared.Time;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}
