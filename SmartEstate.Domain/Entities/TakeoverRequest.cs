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
    public bool IsFeePaid { get; private set; }
    public DateTimeOffset? PaidAt { get; private set; }

    public TakeoverStatus Status { get; private set; } = TakeoverStatus.Pending;

    public DateTimeOffset? AcceptedAt { get; private set; }
    public DateTimeOffset? RejectedAt { get; private set; }
    public DateTimeOffset? CancelledAt { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }

    public string? Note { get; private set; } // optional note from seller

    // Navigation
    public Listing Listing { get; set; } = default!;
    public User SellerUser { get; set; } = default!;
    public User BrokerUser { get; set; } = default!;

    public static TakeoverRequest Create(
        Guid listingId,
        Guid sellerUserId,
        Guid brokerUserId,
        TakeoverPayer payer,
        string? note)
    {
        if (sellerUserId == brokerUserId) throw new DomainException("seller and broker cannot be the same user");

        return new TakeoverRequest
        {
            ListingId = listingId,
            SellerUserId = sellerUserId,
            BrokerUserId = brokerUserId,
            Payer = payer,
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

    public void MarkFeePaid(DateTimeOffset at)
    {
        IsFeePaid = true;
        PaidAt = at;
    }

    public void Complete(DateTimeOffset at)
    {
        if (Status != TakeoverStatus.Accepted)
            throw new DomainException("only accepted request can be completed");
        if (!IsFeePaid)
            throw new DomainException("fee must be paid to complete takeover");

        Status = TakeoverStatus.Completed;
        CompletedAt = at;
    }

    public Guid GetPayerUserId()
        => Payer == TakeoverPayer.Seller ? SellerUserId : BrokerUserId;
}
