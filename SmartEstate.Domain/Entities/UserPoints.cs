using SmartEstate.Domain.Common;

namespace SmartEstate.Domain.Entities;

public class UserPoints : AuditableEntity
{
    public Guid UserId { get; private set; }
    public int MonthlyPoints { get; private set; }
    public string MonthlyMonthKey { get; private set; } = "";
    public int PermanentPoints { get; private set; }

    public User User { get; set; } = default!;

    public static UserPoints Create(Guid userId, string monthKey)
    {
        return new UserPoints
        {
            UserId = userId,
            MonthlyMonthKey = monthKey,
            MonthlyPoints = 0,
            PermanentPoints = 0
        };
    }

    public void EnsureMonth(string monthKey)
    {
        if (MonthlyMonthKey == monthKey) return;

        MonthlyMonthKey = monthKey;
        MonthlyPoints = 0;
    }

    public bool TrySpend(int points, string monthKey)
    {
        if (points <= 0) return true;

        EnsureMonth(monthKey);

        var total = MonthlyPoints + PermanentPoints;
        if (total < points) return false;

        var remaining = points;

        var spendMonthly = Math.Min(MonthlyPoints, remaining);
        MonthlyPoints -= spendMonthly;
        remaining -= spendMonthly;

        if (remaining > 0)
        {
            PermanentPoints -= remaining;
        }

        return true;
    }

    public void AddPermanent(int points)
    {
        if (points <= 0) return;
        PermanentPoints += points;
    }

    public void AddMonthly(int points, string monthKey)
    {
        if (points <= 0) return;
        EnsureMonth(monthKey);
        MonthlyPoints += points;
    }
}

