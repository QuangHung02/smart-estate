namespace SmartEstate.App.Features.BrokerApplications.Dtos;

public sealed record CreateBrokerApplicationRequest(string? DocUrl);

public sealed record BrokerApplicationResponse(
    Guid Id,
    Guid UserId,
    string? DocUrl,
    int Status,
    bool IsActivationPaid,
    DateTimeOffset? ActivationPaidAt,
    Guid? ReviewedByAdminId,
    DateTimeOffset? ReviewedAt
);

