using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartEstate.App.Features.Auth;
using SmartEstate.App.Features.Auth.Dtos;
using SmartEstate.Shared.Errors;
using SmartEstate.Shared.Time;

namespace SmartEstate.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize] // phải login
public sealed class UsersController : ControllerBase
{
    private readonly AuthService _auth;
    private readonly ICurrentUser _currentUser;

    public UsersController(AuthService auth, ICurrentUser currentUser)
    {
        _auth = auth;
        _currentUser = currentUser;
    }

    // GET /api/users/me
    [HttpGet("me")]
    public async Task<IActionResult> Me(CancellationToken ct)
    {
        var userId = _currentUser.UserId;
        if (userId is null)
            return Unauthorized(new AppError(ErrorCodes.Unauthorized, "Unauthorized."));

        var result = await _auth.GetMyProfileAsync(userId.Value, ct);
        return ToActionResult(result);
    }

    // PUT /api/users/me
    [HttpPut("me")]
    public async Task<IActionResult> UpdateMe([FromBody] UpdateProfileRequest req, CancellationToken ct)
    {
        var userId = _currentUser.UserId;
        if (userId is null)
            return Unauthorized(new AppError(ErrorCodes.Unauthorized, "Unauthorized."));

        var result = await _auth.UpdateMyProfileAsync(userId.Value, req, ct);
        return ToActionResult(result);
    }

    private IActionResult ToActionResult<T>(SmartEstate.Shared.Results.Result<T> result)
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
