using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartEstate.App.Features.Points;
using SmartEstate.App.Features.Points.Dtos;
using SmartEstate.Shared.Errors;
using SmartEstate.Shared.Results;

namespace SmartEstate.Api.Controllers;

[ApiController]
[Route("api/points")]
[Authorize]
public sealed class PointsController : ControllerBase
{
    private readonly PointPurchaseService _purchases;

    public PointsController(PointPurchaseService purchases)
    {
        _purchases = purchases;
    }

    [HttpGet("packages")]
    [ProducesResponseType(typeof(List<PointPackageDto>), 200)]
    public async Task<IActionResult> GetPackages(CancellationToken ct)
    {
        var result = await _purchases.GetPackagesAsync(ct);
        return ToActionResult(result);
    }

    [HttpPost("purchases")]
    [ProducesResponseType(typeof(PointPurchaseInitResponse), 200)]
    [ProducesResponseType(typeof(AppError), 400)]
    [ProducesResponseType(typeof(AppError), 401)]
    public async Task<IActionResult> CreatePurchase([FromBody] CreatePointPurchaseRequest req, CancellationToken ct)
    {
        var result = await _purchases.CreatePurchaseAsync(req, ct);
        return ToActionResult(result);
    }

    [HttpPost("/api/payments/points/{paymentId:guid}/paid")]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public async Task<IActionResult> MarkPointPaymentPaid([FromRoute] Guid paymentId, [FromBody] object? rawPayload, CancellationToken ct)
    {
        var raw = rawPayload?.ToString();
        var result = await _purchases.MarkPaymentPaidAsync(paymentId, raw, ct);
        return ToActionResult(result);
    }

    private IActionResult ToActionResult(Result result)
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

    private IActionResult ToActionResult<T>(Result<T> result)
    {
        if (result.IsSuccess) return Ok(result.Value);

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

