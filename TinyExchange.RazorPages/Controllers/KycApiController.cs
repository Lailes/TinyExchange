using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinyExchange.RazorPages.Database.Managers.Auth;
using TinyExchange.RazorPages.Infrastructure.Authentication;
using TinyExchange.RazorPages.Models.AuthModels;

namespace TinyExchange.RazorPages.Controllers;

[Authorize(Roles = SystemRoles.KycManager, Policy = KycClaimSettings.PolicyName)]
[Route("api/kyc")]
public class KycApiController : Controller
{
    private readonly IKycManager _kycManager;

    public KycApiController(IKycManager kycManager) => _kycManager = kycManager;

    [HttpPost("confirm/{requestId}")]
    public async Task ConfirmKyc(int requestId) =>
        Response.StatusCode = await _kycManager.ChangeStateKyc(requestId, KycState.Confirmed) switch
        {
            ChangeKycStateResult.Ok => StatusCodes.Status200OK,
            ChangeKycStateResult.NotFound => StatusCodes.Status400BadRequest,
            _ => throw new ArgumentOutOfRangeException()
        };

    [HttpPost("cancel/{requestId}")]
    public async Task RejectKyc(int requestId) =>
        Response.StatusCode = await _kycManager.ChangeStateKyc(requestId, KycState.Rejected) switch
        {
            ChangeKycStateResult.Ok => StatusCodes.Status200OK,
            ChangeKycStateResult.NotFound => StatusCodes.Status400BadRequest,
            _ => throw new ArgumentOutOfRangeException()
        };
}