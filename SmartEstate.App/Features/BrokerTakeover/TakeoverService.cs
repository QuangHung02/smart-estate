using Microsoft.EntityFrameworkCore;
using SmartEstate.App.Common.Abstractions;
using SmartEstate.App.Features.BrokerTakeover.Dtos;
using SmartEstate.Domain.Entities;
using SmartEstate.Domain.Enums;
using SmartEstate.Infrastructure.Persistence;
using SmartEstate.Shared.Errors;
using SmartEstate.Shared.Results;
using SmartEstate.Shared.Time;

namespace SmartEstate.App.Features.BrokerTakeover;

public sealed class TakeoverService
{
    private readonly SmartEstateDbContext _db;
    private readonly ICurrentUser _currentUser;
    private readonly IClock _clock;
    private readonly IPaymentGateway _payments;

    public TakeoverService(SmartEstateDbContext db, ICurrentUser currentUser, IClock clock, IPaymentGateway payments)
    {
        _db = db;
        _currentUser = currentUser;
        _clock = clock;
        _payments = payments;
    }

    // Seller creates takeover request for a listing
    public async Task<Result<TakeoverResponse>> RequestAsync(RequestTakeoverRequest req, bool isAdmin, CancellationToken ct = default)
    {
        var userId = _currentUser.UserId;
        if (userId is null) return Result<TakeoverResponse>.Fail(ErrorCodes.Unauthorized, "Unauthorized.");

        // listing must exist
        var listing = await _db.Listings.FirstOrDefaultAsync(x => x.Id == req.ListingId && !x.IsDeleted, ct);
        if (listing is null) return Result<TakeoverResponse>.Fail(ErrorCodes.NotFound, "Listing not found.");

        // only responsible seller/admin can request takeover
        if (!isAdmin && listing.ResponsibleUserId != userId.Value)
            return Result<TakeoverResponse>.Fail(ErrorCodes.Forbidden, "No permission to request takeover on this listing.");

        // broker must exist and be Broker role
        var broker = await _db.Users.FirstOrDefaultAsync(x => x.Id == req.BrokerUserId && !x.IsDeleted && x.IsActive, ct);
        if (broker is null) return Result<TakeoverResponse>.Fail(ErrorCodes.NotFound, "Broker user not found.");
        if (broker.Role != UserRole.Broker && broker.Role != UserRole.Admin)
            return Result<TakeoverResponse>.Fail(ErrorCodes.Validation, "Target user is not a broker.");

        // prevent duplicate active requests (pending/accepted) for same listing + broker
        var exists = await _db.TakeoverRequests.AnyAsync(x =>
            x.ListingId == req.ListingId &&
            x.BrokerUserId == req.BrokerUserId &&
            !x.IsDeleted &&
            (x.Status == TakeoverStatus.Pending || x.Status == TakeoverStatus.Accepted), ct);

        if (exists)
            return Result<TakeoverResponse>.Fail(ErrorCodes.Conflict, "A takeover request already exists.");

        // domain create
        var takeover = TakeoverRequest.Create(
            listingId: req.ListingId,
            sellerUserId: userId.Value,
            brokerUserId: req.BrokerUserId,
            payer: req.Payer,
            feeAmount: req.FeeAmount,
            feeCurrency: req.FeeCurrency,
            note: req.Note
        );

        _db.TakeoverRequests.Add(takeover);
        await _db.SaveChangesAsync(true, ct);

        return Result<TakeoverResponse>.Ok(new TakeoverResponse(
            takeover.Id,
            takeover.ListingId,
            takeover.SellerUserId,
            takeover.BrokerUserId,
            takeover.Payer,
            takeover.Fee.Amount,
            takeover.Fee.Currency,
            takeover.Status,
            takeover.PaymentId,
            null
        ));
    }

    // Broker accepts/rejects. If accept => create payment (pending) and return payUrl
    public async Task<Result<TakeoverResponse>> DecideAsync(Guid takeoverId, bool accept, bool isAdmin, CancellationToken ct = default)
    {
        var userId = _currentUser.UserId;
        if (userId is null) return Result<TakeoverResponse>.Fail(ErrorCodes.Unauthorized, "Unauthorized.");

        var takeover = await _db.TakeoverRequests
            .FirstOrDefaultAsync(x => x.Id == takeoverId && !x.IsDeleted, ct);

        if (takeover is null) return Result<TakeoverResponse>.Fail(ErrorCodes.NotFound, "Takeover request not found.");

        // only broker (target) or admin can decide
        if (!isAdmin && takeover.BrokerUserId != userId.Value)
            return Result<TakeoverResponse>.Fail(ErrorCodes.Forbidden, "No permission to decide this takeover.");

        if (!accept)
        {
            takeover.Reject(_clock.UtcNow);
            await _db.SaveChangesAsync(true, ct);

            return Result<TakeoverResponse>.Ok(new TakeoverResponse(
                takeover.Id, takeover.ListingId, takeover.SellerUserId, takeover.BrokerUserId,
                takeover.Payer, takeover.Fee.Amount, takeover.Fee.Currency,
                takeover.Status, takeover.PaymentId, null
            ));
        }

        // accept => domain transition
        takeover.Accept(_clock.UtcNow);

        // init payment
        var payerUserId = takeover.GetPayerUserId();
        var paymentInit = await _payments.CreatePaymentAsync(
            payerUserId: payerUserId,
            amount: takeover.Fee.Amount,
            currency: takeover.Fee.Currency,
            description: $"Takeover fee for listing {takeover.ListingId}",
            ct: ct
        );

        var payment = Payment.CreateTakeoverFee(
            payerUserId: payerUserId,
            listingId: takeover.ListingId,
            takeoverRequestId: takeover.Id,
            amount: takeover.Fee.Amount,
            currency: takeover.Fee.Currency,
            provider: paymentInit.Provider,
            providerRef: paymentInit.ProviderRef,
            payUrl: paymentInit.PayUrl
        );

        _db.Payments.Add(payment);
        await _db.SaveChangesAsync(true, ct);

        // attach payment to takeover (domain)
        takeover.AttachPayment(payment.Id);
        await _db.SaveChangesAsync(true, ct);

        return Result<TakeoverResponse>.Ok(new TakeoverResponse(
            takeover.Id, takeover.ListingId, takeover.SellerUserId, takeover.BrokerUserId,
            takeover.Payer, takeover.Fee.Amount, takeover.Fee.Currency,
            takeover.Status, takeover.PaymentId, payment.PayUrl
        ));
    }

    // Payment webhook/confirm => mark paid + complete takeover + assign broker
    public async Task<Result> MarkPaymentPaidAsync(Guid paymentId, string? rawPayloadJson, CancellationToken ct = default)
    {
        var payment = await _db.Payments.FirstOrDefaultAsync(x => x.Id == paymentId && !x.IsDeleted, ct);
        if (payment is null) return Result.Fail(ErrorCodes.NotFound, "Payment not found.");

        if (payment.Status == PaymentStatus.Paid) return Result.Ok();

        payment.MarkPaid(rawPayloadJson);

        if (payment.TakeoverRequestId is null || payment.ListingId is null)
            return Result.Fail(ErrorCodes.Validation, "Invalid payment linkage.");

        var takeover = await _db.TakeoverRequests
            .FirstOrDefaultAsync(x => x.Id == payment.TakeoverRequestId.Value && !x.IsDeleted, ct);
        if (takeover is null) return Result.Fail(ErrorCodes.NotFound, "Takeover request not found.");

        var listing = await _db.Listings.FirstOrDefaultAsync(x => x.Id == payment.ListingId.Value && !x.IsDeleted, ct);
        if (listing is null) return Result.Fail(ErrorCodes.NotFound, "Listing not found.");

        // complete takeover (domain) + assign broker (domain)
        takeover.Complete(_clock.UtcNow);
        listing.AssignBroker(takeover.BrokerUserId);

        await _db.SaveChangesAsync(true, ct);
        return Result.Ok();
    }

    // Seller unassign broker => take back responsibility
    public async Task<Result> UnassignBrokerAsync(Guid listingId, bool isAdmin, CancellationToken ct = default)
    {
        var userId = _currentUser.UserId;
        if (userId is null) return Result.Fail(ErrorCodes.Unauthorized, "Unauthorized.");

        var listing = await _db.Listings.FirstOrDefaultAsync(x => x.Id == listingId && !x.IsDeleted, ct);
        if (listing is null) return Result.Fail(ErrorCodes.NotFound, "Listing not found.");

        if (!isAdmin && listing.CreatedByUserId != userId.Value)
            return Result.Fail(ErrorCodes.Forbidden, "Only seller can unassign broker.");

        listing.UnassignBroker();
        await _db.SaveChangesAsync(true, ct);
        return Result.Ok();
    }
}
