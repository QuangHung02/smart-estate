using Microsoft.EntityFrameworkCore;
using SmartEstate.Infrastructure.Persistence;
using SmartEstate.Shared.Time;

namespace SmartEstate.Api.Jobs;

public sealed class AwaitingPaymentCleanupService : BackgroundService
{
    private readonly IServiceProvider _sp;
    private readonly TimeSpan _interval = TimeSpan.FromHours(6);

    public AwaitingPaymentCleanupService(IServiceProvider sp)
    {
        _sp = sp;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // small initial delay to allow app to warm up
        await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<SmartEstateDbContext>();
                var clock = scope.ServiceProvider.GetRequiredService<IClock>();
                var now = clock.UtcNow;
                var threshold = now.AddDays(-3);

                var expired = await db.Listings
                    .Where(l => !l.IsDeleted
                        && l.ModerationStatus == Domain.Enums.ModerationStatus.AwaitingPayment
                        && ((l.UpdatedAt ?? l.CreatedAt) < threshold))
                    .ToListAsync(stoppingToken);

                if (expired.Count > 0)
                {
                    db.RemoveRange(expired);
                    await db.SaveChangesAsync(true, stoppingToken);
                }
            }
            catch
            {
                // swallow to keep background loop alive; optionally add logging here
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}
