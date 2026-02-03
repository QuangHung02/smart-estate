using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartEstate.App.Features.BrokerTakeover;
using SmartEstate.App.Features.BrokerTakeover.Dtos;
using SmartEstate.Shared.Errors;

namespace SmartEstate.Api.Controllers;

[ApiController]
[Route("api/takeovers")]
public sealed class TakeoversController : ControllerBase
{
    private readonly TakeoverService _svc;

    public TakeoversController(TakeoverService svc)
    {
        _svc = svc;
    }

    // Seller requests takeover
    [HttpPost]
    [Authorize(Roles = "Seller,Broker,Admin")]
    public async Task<IActionResult> Request([FromBody] RequestTakeoverRequest req, CancellationToken ct)
    {
        var isAdmin = User.IsInRole("Admin");
        var result = await _svc.RequestAsync(req, isAdmin, ct);
        return result.IsSuccess ? Ok(result.Value) : MapError(result.Error);
    }

    // Broker accepts/rejects
    [HttpPost("{id:guid}/decide")]
    [Authorize(Roles = "Broker,Admin")]
    public async Task<IActionResult> Decide([FromRoute] Guid id, [FromBody] DecideTakeoverRequest req, CancellationToken ct)
    {
        var isAdmin = User.IsInRole("Admin");
        var result = await _svc.DecideAsync(id, req.Accept, isAdmin, ct);
        return result.IsSuccess ? Ok(result.Value) : MapError(result.Error);
    }

    // Webhook/confirm payment (demo)
    [HttpPost("/api/payments/{paymentId:guid}/paid")]
    [AllowAnonymous]
    public async Task<IActionResult> MarkPaid([FromRoute] Guid paymentId, [FromBody] object? rawPayload, CancellationToken ct)
    {
        var raw = rawPayload?.ToString();
        var result = await _svc.MarkPaymentPaidAsync(paymentId, raw, ct);
        return result.IsSuccess ? Ok() : MapError(result.Error);
    }

    // Seller unassign broker
    [HttpPost("/api/listings/{listingId:guid}/unassign-broker")]
    [Authorize(Roles = "Seller,Admin")]
    public async Task<IActionResult> UnassignBroker([FromRoute] Guid listingId, CancellationToken ct)
    {
        var isAdmin = User.IsInRole("Admin");
        var result = await _svc.UnassignBrokerAsync(listingId, isAdmin, ct);
        return result.IsSuccess ? Ok() : MapError(result.Error);
    }

    private IActionResult MapError(AppError? e)
    {
        if (e is null) return StatusCode(500, new AppError(ErrorCodes.Unexpected, "Unexpected error"));

        return e.Code switch
        {
            ErrorCodes.Validation => BadRequest(e),
            ErrorCodes.Unauthorized => Unauthorized(e),
            ErrorCodes.Forbidden => Forbid(),
            ErrorCodes.NotFound => NotFound(e),
            ErrorCodes.Conflict => Conflict(e),
            _ => StatusCode(500, e)
        };
    }
}
