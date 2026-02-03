using Microsoft.EntityFrameworkCore;
using SmartEstate.App.Features.Search.Dtos;
using SmartEstate.Domain.Enums;
using SmartEstate.Infrastructure.Persistence;
using SmartEstate.Shared.Errors;
using SmartEstate.Shared.Paging;
using SmartEstate.Shared.Results;

namespace SmartEstate.App.Features.Search;

public sealed class SearchService
{
    private readonly SmartEstateDbContext _db;

    public SearchService(SmartEstateDbContext db)
    {
        _db = db;
    }

    public async Task<Result<PagedResult<SearchItemResponse>>> SearchAsync(SearchRequest req, CancellationToken ct = default)
    {
        var page = req.Page <= 0 ? 1 : req.Page;
        var pageSize = req.PageSize <= 0 ? 20 : Math.Min(req.PageSize, 100);

        // base query: public listings only
        var q = _db.Listings
            .AsNoTracking()
            .Where(x => !x.IsDeleted
                && x.ModerationStatus == ModerationStatus.Approved
                && x.LifecycleStatus == ListingLifecycleStatus.Active);

        // keyword (simple LIKE)
        if (!string.IsNullOrWhiteSpace(req.Keyword))
        {
            var k = req.Keyword.Trim();
            q = q.Where(x =>
                x.Title.Contains(k) ||
                x.Description.Contains(k));
        }

        // location filters (owned columns in SQL server are flattened)
        if (!string.IsNullOrWhiteSpace(req.City))
        {
            var city = req.City.Trim();
            q = q.Where(x => x.Address.City != null && x.Address.City == city);
        }

        if (!string.IsNullOrWhiteSpace(req.District))
        {
            var district = req.District.Trim();
            q = q.Where(x => x.Address.District != null && x.Address.District == district);
        }

        if (!string.IsNullOrWhiteSpace(req.Ward))
        {
            var ward = req.Ward.Trim();
            q = q.Where(x => x.Address.Ward != null && x.Address.Ward == ward);
        }

        if (req.PropertyType is not null)
            q = q.Where(x => x.PropertyType == req.PropertyType.Value);

        // numeric filters
        if (req.MinPrice is not null)
            q = q.Where(x => x.Price.Amount >= req.MinPrice.Value);

        if (req.MaxPrice is not null)
            q = q.Where(x => x.Price.Amount <= req.MaxPrice.Value);

        if (req.MinAreaM2 is not null)
            q = q.Where(x => x.AreaM2 != null && x.AreaM2.Value >= req.MinAreaM2.Value);

        if (req.MaxAreaM2 is not null)
            q = q.Where(x => x.AreaM2 != null && x.AreaM2.Value <= req.MaxAreaM2.Value);

        if (req.MinBedrooms is not null)
            q = q.Where(x => x.Bedrooms != null && x.Bedrooms.Value >= req.MinBedrooms.Value);

        if (req.MinBathrooms is not null)
            q = q.Where(x => x.Bathrooms != null && x.Bathrooms.Value >= req.MinBathrooms.Value);

        // map bounds (only if all provided)
        var hasBounds =
            req.MinLat is not null && req.MaxLat is not null &&
            req.MinLng is not null && req.MaxLng is not null;

        if (hasBounds)
        {
            var minLat = req.MinLat!.Value;
            var maxLat = req.MaxLat!.Value;
            var minLng = req.MinLng!.Value;
            var maxLng = req.MaxLng!.Value;

            // only listings with location
            q = q.Where(x => x.Location != null
                && x.Location.Lat >= minLat && x.Location.Lat <= maxLat
                && x.Location.Lng >= minLng && x.Location.Lng <= maxLng);
        }

        // sorting
        q = req.Sort?.ToLowerInvariant() switch
        {
            "price_asc" => q.OrderBy(x => x.Price.Amount).ThenByDescending(x => x.CreatedAt),
            "price_desc" => q.OrderByDescending(x => x.Price.Amount).ThenByDescending(x => x.CreatedAt),
            _ => q.OrderByDescending(x => x.CreatedAt)
        };

        // total
        var total = await q.CountAsync(ct);

        // items (cover image = sortOrder smallest)
        var items = await q
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new SearchItemResponse(
                x.Id,
                x.Title,
                x.PropertyType,
                x.Price.Amount,
                x.Price.Currency,
                x.AreaM2,
                x.Bedrooms,
                x.Bathrooms,
                x.Address.City,
                x.Address.District,
                x.Address.Ward,
                x.Location != null ? x.Location.Lat : (double?)null,
                x.Location != null ? x.Location.Lng : (double?)null,
                x.Images
                    .Where(i => !i.IsDeleted)
                    .OrderBy(i => i.SortOrder)
                    .Select(i => i.Url)
                    .FirstOrDefault()
            ))
            .ToListAsync(ct);

        return Result<PagedResult<SearchItemResponse>>.Ok(new PagedResult<SearchItemResponse>(items, total, page, pageSize));
    }
}
