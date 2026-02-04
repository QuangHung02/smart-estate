using Microsoft.AspNetCore.Mvc;
using SmartEstate.App.Features.Search;
using SmartEstate.App.Features.Search.Dtos;
using SmartEstate.Shared.Errors;

namespace SmartEstate.Api.Controllers;

[ApiController]
[Route("api/search")]
public sealed class SearchController : ControllerBase
{
    private readonly SearchService _svc;

    public SearchController(SearchService svc)
    {
        _svc = svc;
    }

    // GET /api/search/listings?...querystring
    [HttpGet("listings")]
    public async Task<IActionResult> SearchListings(
        [FromQuery] string? keyword,
        [FromQuery] string? city,
        [FromQuery] string? district,
        [FromQuery] string? ward,
        [FromQuery] int? propertyType,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] double? minAreaM2,
        [FromQuery] double? maxAreaM2,
        [FromQuery] int? minBedrooms,
        [FromQuery] int? minBathrooms,
        [FromQuery] double? minLat,
        [FromQuery] double? maxLat,
        [FromQuery] double? minLng,
        [FromQuery] double? maxLng,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string sort = "newest",
        CancellationToken ct = default)
    {
        var pt = propertyType is null ? null : (SmartEstate.Domain.Enums.PropertyType?)propertyType.Value;

        var req = new SearchRequest(
            Keyword: keyword,
            City: city,
            District: district,
            Ward: ward,
            PropertyType: pt,
            MinPrice: minPrice,
            MaxPrice: maxPrice,
            MinAreaM2: minAreaM2,
            MaxAreaM2: maxAreaM2,
            MinBedrooms: minBedrooms,
            MinBathrooms: minBathrooms,
            MinLat: minLat,
            MaxLat: maxLat,
            MinLng: minLng,
            MaxLng: maxLng,
            Page: page,
            PageSize: pageSize,
            Sort: sort
        );

        var result = await _svc.SearchAsync(req, ct);
        if (!result.IsSuccess)
            return BadRequest(result.Error ?? new AppError(ErrorCodes.Unexpected, "Unexpected error"));

        return Ok(result.Value);
    }
}
