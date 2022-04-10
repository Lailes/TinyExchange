using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinyExchange.RazorPages.Database.Managers.Amount;
using TinyExchange.RazorPages.Infrastructure.Authentication;
using TinyExchange.RazorPages.Models.AmountModels;
using TinyExchange.RazorPages.Models.AuthModels;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace TinyExchange.RazorPages.Controllers;

[Route("api")]
public class TransferApiController : Controller
{
    [HttpPost("debits/cancel")]
    [Authorize(Roles = $"{SystemRoles.User},{SystemRoles.FundsManager}", Policy = KycClaimSettings.PolicyName)]
    public async Task CancelTransfer([FromServices] IAmountManager amountManager, int transferId, int userId) =>
        Response.StatusCode = await amountManager.CancelDebitAsync(transferId, userId) switch
        {
            DebitCancelResult.Ok => StatusCodes.Status200OK,
            DebitCancelResult.NotFound => StatusCodes.Status400BadRequest,
            DebitCancelResult.NotAllowed => StatusCodes.Status405MethodNotAllowed,
            _ => throw new ArgumentOutOfRangeException()
        };

    [HttpPost("withdrawals/cancel")]
    [Authorize(Roles = $"{SystemRoles.User},{SystemRoles.FundsManager}", Policy = KycClaimSettings.PolicyName)]
    public async Task CancelWithdrawal([FromServices] IAmountManager amountManager, int transferId, int userId) =>
        Response.StatusCode = await amountManager.CancelWithdrawalAsync(transferId, userId) switch
            {
                WithdrawalCancelResult.Ok => StatusCodes.Status200OK,
                WithdrawalCancelResult.NotFound => StatusCodes.Status400BadRequest,
                WithdrawalCancelResult.NotAllowed => StatusCodes.Status405MethodNotAllowed,
                _ => throw new ArgumentOutOfRangeException()
            };

    [HttpPost("debits/confirm")]
    [Authorize(Roles = SystemRoles.FundsManager, Policy = KycClaimSettings.PolicyName)]
    public async Task ConfirmDebit([FromServices] IAmountManager amountManager, int transferId, int userId) =>
        Response.StatusCode =
            await amountManager.ConfirmDebitAsync(transferId, userId) switch
            {
                ConfirmDebitResult.Ok => StatusCodes.Status200OK,
                ConfirmDebitResult.NotFound => StatusCodes.Status400BadRequest,
                ConfirmDebitResult.NotAllowed => StatusCodes.Status405MethodNotAllowed,
                _ => throw new ArgumentOutOfRangeException()
            };


    [HttpPost("withdrawals/confirm")]
    [Authorize(Roles = SystemRoles.FundsManager, Policy = KycClaimSettings.PolicyName)]
    public async Task ConfirmWithdrawal([FromServices] IAmountManager amountManager, int transferId, int userId) =>
        Response.StatusCode = await amountManager.ConfirmWithdrawalAsync(transferId, userId) switch
            {
                ConfirmWithdrawalResult.Ok => StatusCodes.Status200OK,
                ConfirmWithdrawalResult.NotFound => StatusCodes.Status400BadRequest,
                ConfirmWithdrawalResult.NotAllowed => StatusCodes.Status405MethodNotAllowed,
                _ => throw new ArgumentOutOfRangeException()
            };
}