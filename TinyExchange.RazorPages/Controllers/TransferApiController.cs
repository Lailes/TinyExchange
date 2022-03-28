using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinyExchange.RazorPages.Database.Managers.Amount;
using TinyExchange.RazorPages.Models.AuthModels;

namespace TinyExchange.RazorPages.Controllers;

[Authorize(Roles = $"{SystemRoles.User},{SystemRoles.FoundsManager}")]
[Route("api")]
public class TransferApiController : Controller
{
    [HttpPost("debits/cancel")]
    public async Task CancelTransfer([FromServices] IAmountManager amountManager, [FromBody] CancelTransferInfo cancelInfo) =>
        Response.StatusCode = await amountManager.CancelDebitTransferAsync(cancelInfo.TransferId, cancelInfo.CancelerId) switch
        {
            DebitCancelResult.Ok => StatusCodes.Status200OK,
            DebitCancelResult.NotFound => StatusCodes.Status400BadRequest,
            DebitCancelResult.NotAllowed => StatusCodes.Status405MethodNotAllowed,
            _ => throw new ArgumentOutOfRangeException()
        };

    [HttpPost("withdrawals/cancel")]
    public async Task CancelWithdrawal([FromServices] IAmountManager amountManager, [FromBody] CancelTransferInfo cancelInfo) =>
        Response.StatusCode = await amountManager.CancelWithdrawalTransferAsync(cancelInfo.TransferId, cancelInfo.CancelerId) switch
            {
                WithdrawalCancelResult.Ok => StatusCodes.Status200OK,
                WithdrawalCancelResult.NotFound => StatusCodes.Status400BadRequest,
                WithdrawalCancelResult.NotAllowed => StatusCodes.Status405MethodNotAllowed,
                _ => throw new ArgumentOutOfRangeException()
            };
}

public class CancelTransferInfo
{
    public int TransferId { get; set; }
    public int CancelerId { get; set; }
}