using SmartEstate.Domain.Common;
using SmartEstate.Domain.Enums;
using SmartEstate.Domain.ValueObjects;

namespace SmartEstate.Domain.Entities;

public class TakeoverRequest : AuditableEntity
{
    public Guid ListingId { get; private set; }

    public Guid SellerUserId { get; private set; }      // requester (seller)
    public Guid BrokerUserId { get; private set; }      // target broker

    public TakeoverPayer Payer { get; private set; } = TakeoverPayer.Seller;
    public Money Fee { get; private set; } = new Money(0);

    public TakeoverStatus Status { get; private set; } = TakeoverStatus.Pending;

    public DateTimeOffset? AcceptedAt { get; private set; }
    public DateTimeOffset? RejectedAt { get; private set; }
    public DateTimeOffset? CancelledAt { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }

    public Guid? PaymentId { get; private set; }

    public string? Note { get; private set; } // optional note from seller

    // Navigation
    public Listing Listing { get; set; } = default!;
    public User SellerUser { get; set; } = default!;
    public User BrokerUser { get; set; } = default!;
    public Payment? Payment { get; set; }

    public static TakeoverRequest Create(
        Guid listingId,
        Guid sellerUserId,
        Guid brokerUserId,
        TakeoverPayer payer,
        decimal feeAmount,
        string feeCurrency,
        string? note)
    {
        if (feeAmount < 0) throw new DomainException("fee must be >= 0");
        if (sellerUserId == brokerUserId) throw new DomainException("seller and broker cannot be the same user");

        return new TakeoverRequest
        {
            ListingId = listingId,
            SellerUserId = sellerUserId,
            BrokerUserId = brokerUserId,
            Payer = payer,
            Fee = new Money(feeAmount, feeCurrency),
            Status = TakeoverStatus.Pending,
            Note = string.IsNullOrWhiteSpace(note) ? null : note.Trim()
        };
    }

    public void Accept(DateTimeOffset at)
    {
        if (Status != TakeoverStatus.Pending)
            throw new DomainException("only pending request can be accepted");

        Status = TakeoverStatus.Accepted;
        AcceptedAt = at;
    }

    public void Reject(DateTimeOffset at)
    {
        if (Status != TakeoverStatus.Pending)
            throw new DomainException("only pending request can be rejected");

        Status = TakeoverStatus.Rejected;
        RejectedAt = at;
    }

    public void Cancel(DateTimeOffset at)
    {
        if (Status != TakeoverStatus.Pending)
            throw new DomainException("only pending request can be cancelled");

        Status = TakeoverStatus.Cancelled;
        CancelledAt = at;
    }

    public void AttachPayment(Guid paymentId)
    {
        if (Status != TakeoverStatus.Accepted)
            throw new DomainException("payment can only be attached after accepted");

        PaymentId = paymentId;
    }

    public void Complete(DateTimeOffset at)
    {
        if (Status != TakeoverStatus.Accepted)
            throw new DomainException("only accepted request can be completed");
        if (PaymentId is null)
            throw new DomainException("payment required to complete takeover");

        Status = TakeoverStatus.Completed;
        CompletedAt = at;
    }

    public Guid GetPayerUserId()
        => Payer == TakeoverPayer.Seller ? SellerUserId : BrokerUserId;
}
