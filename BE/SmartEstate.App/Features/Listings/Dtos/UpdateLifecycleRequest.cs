using SmartEstate.Domain.Enums;

namespace SmartEstate.App.Features.Listings.Dtos;

public sealed record UpdateLifecycleRequest(ListingLifecycleStatus Status);
