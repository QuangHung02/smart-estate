namespace SmartEstate.Shared.Time;

public interface ICurrentUser
{
    Guid? UserId { get; }
}
