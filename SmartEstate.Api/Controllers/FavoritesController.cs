using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartEstate.App.Features.Favorites;
using SmartEstate.Shared.Errors;
using SmartEstate.Shared.Time;

namespace SmartEstate.Api.Controllers;

[ApiController]
[Route("api/users/me/favorites")]
[Authorize]
public sealed class FavoritesController : ControllerBase
{
    private readonly FavoritesService _svc;

    public FavoritesController(FavoritesService svc)
    {
        _svc = svc;
    }

    [HttpPost("{listingId:guid}")]
    public async Task<IActionResult> Add([FromRoute] Guid listingId, CancellationToken ct)
    {
        var isAdmin = User.IsInRole("Admin");
        var result = await _svc.AddAsync(listingId, isAdmin, ct);
        return ToActionResult(result);
    }

    [HttpDelete("{listingId:guid}")]
    public async Task<IActionResult> Remove([FromRoute] Guid listingId, CancellationToken ct)
    {
        var result = await _svc.RemoveAsync(listingId, ct);
        return ToActionResult(result);
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        var result = await _svc.ListAsync(page, pageSize, ct);
        if (!result.IsSuccess) return BadRequest(result.Error);

        return Ok(result.Value);
    }

    private IActionResult ToActionResult(SmartEstate.Shared.Results.Result result)
    {
        if (result.IsSuccess) return Ok();

        return result.Error?.Code switch
        {
            ErrorCodes.Validation => BadRequest(result.Error),
            ErrorCodes.Unauthorized => Unauthorized(result.Error),
            ErrorCodes.Forbidden => Forbid(),
            ErrorCodes.NotFound => NotFound(result.Error),
            ErrorCodes.Conflict => Conflict(result.Error),
            _ => StatusCode(500, result.Error ?? new AppError(ErrorCodes.Unexpected, "Unexpected error"))
        };
    }
}
