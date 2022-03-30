using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinyExchange.RazorPages.Database.Managers.Amount;
using TinyExchange.RazorPages.Models.AuthModels;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace TinyExchange.RazorPages.Controllers;

[Route("api")]
public class TransferApiController : Controller
{
    [HttpPost("debits/cancel")]
    [Authorize(Roles = $"{SystemRoles.User},{SystemRoles.FoundsManager}")]
    public async Task CancelTransfer([FromServices] IAmountManager amountManager, [FromBody] TransferStateChangeInfo stateChangeInfo) =>
        Response.StatusCode = await amountManager.CancelDebitAsync(stateChangeInfo.TransferId, stateChangeInfo.CancelerId) switch
        {
            DebitCancelResult.Ok => StatusCodes.Status200OK,
            DebitCancelResult.NotFound => StatusCodes.Status400BadRequest,
            DebitCancelResult.NotAllowed => StatusCodes.Status405MethodNotAllowed,
            _ => throw new ArgumentOutOfRangeException()
        };

    [HttpPost("withdrawals/cancel")]
    [Authorize(Roles = $"{SystemRoles.User},{SystemRoles.FoundsManager}")]
    public async Task CancelWithdrawal([FromServices] IAmountManager amountManager, [FromBody] TransferStateChangeInfo stateChangeInfo) =>
        Response.StatusCode = await amountManager.CancelWithdrawalAsync(stateChangeInfo.TransferId, stateChangeInfo.CancelerId) switch
            {
                WithdrawalCancelResult.Ok => StatusCodes.Status200OK,
                WithdrawalCancelResult.NotFound => StatusCodes.Status400BadRequest,
                WithdrawalCancelResult.NotAllowed => StatusCodes.Status405MethodNotAllowed,
                _ => throw new ArgumentOutOfRangeException()
            };

    [HttpPost("debits/confirm")]
    [Authorize(Roles = SystemRoles.FoundsManager)]
    public async Task ConfirmDebit([FromServices] IAmountManager amountManager,
        [FromBody] TransferStateChangeInfo stateChangeInfo) =>
        Response.StatusCode =
            await amountManager.ConfirmDebitAsync(stateChangeInfo.TransferId, stateChangeInfo.CancelerId) switch
            {
                ConfirmDebitResult.Ok => StatusCodes.Status200OK,
                ConfirmDebitResult.NotFound => StatusCodes.Status405MethodNotAllowed,
                _ => throw new ArgumentOutOfRangeException()
            };


    [HttpPost("withdrawals/confirm")]
    [Authorize(Roles = SystemRoles.FoundsManager)]
    public async Task ConfirmWithdrawal([FromServices] IAmountManager amountManager,
        [FromBody] TransferStateChangeInfo stateChangeInfo) =>
        Response.StatusCode =
            await amountManager.ConfirmWithdrawalAsync(stateChangeInfo.TransferId, stateChangeInfo.CancelerId) switch
            {
                ConfirmWithdrawalResult.Ok => StatusCodes.Status200OK,
                ConfirmWithdrawalResult.NotFound => StatusCodes.Status405MethodNotAllowed,
                _ => throw new ArgumentOutOfRangeException()
            };
}

public class TransferStateChangeInfo
{
    public int TransferId { get; set; }
    public int CancelerId { get; set; }
}