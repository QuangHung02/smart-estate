using Microsoft.EntityFrameworkCore;
using SmartEstate.App.Features.Reports.Dtos;
using SmartEstate.Domain.Enums;
using SmartEstate.Infrastructure.Persistence;
using SmartEstate.Shared.Results;

namespace SmartEstate.App.Features.Reports;

public sealed class PaymentReportingService
{
    private readonly SmartEstateDbContext _db;

    public PaymentReportingService(SmartEstateDbContext db)
    {
        _db = db;
    }

    public async Task<Result<PointPurchaseTotalsResponse>> GetPointPurchaseTotalsAsync(DateTimeOffset from, DateTimeOffset to, CancellationToken ct = default)
    {
        var payments = await _db.Payments
            .AsNoTracking()
            .Where(x => !x.IsDeleted
                && x.Status == PaymentStatus.Paid
                && x.PointPurchaseId != null
                && x.CreatedAt >= from
                && x.CreatedAt <= to)
            .ToListAsync(ct);

        var count = payments.Count;
        var currency = payments.FirstOrDefault()?.Amount.Currency ?? "VND";
        var total = payments.Sum(x => x.Amount.Amount);

        return Result<PointPurchaseTotalsResponse>.Ok(new PointPurchaseTotalsResponse(count, total, currency));
    }
}

