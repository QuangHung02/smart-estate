namespace SmartEstate.Shared.Paging;

public sealed record Sort(string Field, string Direction = "desc")
{
    public bool IsAsc => Direction.Equals("asc", StringComparison.OrdinalIgnoreCase);
}
