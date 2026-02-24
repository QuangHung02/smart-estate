using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartEstate.App.Features.BrokerApplications;
using SmartEstate.App.Features.BrokerApplications.Dtos;
using SmartEstate.Shared.Errors;
using SmartEstate.Shared.Results;

namespace SmartEstate.Api.Controllers;

[ApiController]
[Route("api/broker-applications")]
[Authorize]
public sealed class BrokerApplicationsController : ControllerBase
{
    private readonly BrokerApplicationService _svc;

    public BrokerApplicationsController(BrokerApplicationService svc)
    {
        _svc = svc;
    }

    [HttpPost]
    [ProducesResponseType(typeof(BrokerApplicationResponse), 200)]
    [ProducesResponseType(typeof(AppError), 400)]
    [ProducesResponseType(typeof(AppError), 401)]
    public async Task<IActionResult> Create([FromBody] CreateBrokerApplicationRequest req, CancellationToken ct)
    {
        var result = await _svc.CreateAsync(req, ct);
        return ToActionResult(result);
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

